using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float gravityPower = 25f;
    [SerializeField] private float bouncePower = 7f;
    [SerializeField] private float dashSpeed = 9f;
    [SerializeField] private GameObject shattared = default;
    [SerializeField] private ParticleSystem bounceParticles = default;
    [SerializeField] private GameObject splashParticles = default;
    [SerializeField] private float stretchAndSquashFactor = 2;
    [SerializeField] private AudioClip collisionAC = default;

    private bool controllable = true;
    private Vector3 motionDirection;
    private float diameter;
    private float myRadius;
    private Vector3 lastPosition;
    private bool dashing;
    private bool won;
    private bool destroyed;
    private AudioSource myAudioSource;
    private int platformsSinceLastSound;

    void Start()
    {
        diameter = transform.localScale.x;
        myRadius = diameter / 2f;
        lastPosition = transform.position;

        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.clip = collisionAC;
    }

    void Update()
    {
        if (destroyed) return;

        transform.position += motionDirection * Time.deltaTime;

        if (controllable)
        { 
            if (Input.GetMouseButtonDown(0))
                dashing = true;
            if (Input.GetMouseButtonUp(0))
                dashing = false;
        }

        if (!dashing)
            Idle();
        else
            Dash();

        float yScale = diameter + (transform.position.y - lastPosition.y) * stretchAndSquashFactor;
        transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);

        lastPosition = transform.position;
    }

    void Idle()
    {
        motionDirection.y -= gravityPower * Time.deltaTime;

        if (Physics.Linecast(lastPosition, transform.position - new Vector3(0, myRadius, 0), out RaycastHit hit))
        {
            motionDirection.y = bouncePower;
            transform.position = new Vector3(transform.position.x, hit.point.y + myRadius, transform.position.z);

            if (hit.transform.CompareTag("Finish Line"))
                ReachedFinishLine();

            bounceParticles.transform.position = hit.point;
            bounceParticles.Play();

            Instantiate(splashParticles, hit.point + new Vector3(0, 0.01f, 0), splashParticles.transform.rotation, hit.transform);

            myAudioSource.pitch = Random.Range(0.9f, 1.1f);
            myAudioSource.Play();
        }
    }

    void Dash()
    {
        motionDirection.y = -dashSpeed;

        if (Physics.Linecast(lastPosition, transform.position - new Vector3(0, myRadius, 0), out RaycastHit hit))
        {
            if (hit.transform.CompareTag("DAmage Tile"))
            {
                DestroyMe();
                return;
            }

            if (hit.transform.CompareTag("Normal Tile"))
            {
                hit.transform.parent.GetComponent<Platform>().DestroyPlatform();

                GameScore.Instance.AddScore(1);

                if (platformsSinceLastSound == 3)
                {
                    myAudioSource.pitch += 0.05f;
                    myAudioSource.PlayOneShot(collisionAC);
                    platformsSinceLastSound = 0;
                }
                else
                {
                    platformsSinceLastSound++;
                }
            }

            if (hit.transform.CompareTag("Finish Line"))
                ReachedFinishLine();
        }
    }

    void ReachedFinishLine()
    {
        if (won) return;

        won = true;
        dashing = false;
        controllable = false;

        LevelManager.Instance.Won();
    }

    void DestroyMe()
    {
        destroyed = true;
        Instantiate(shattared, transform.position, transform.rotation);
        gameObject.SetActive(false);

        LevelManager.Instance.Lost();
    }
}
