using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject Platform = default;
    [SerializeField] private float distanceBetweenPlatforms = 0.35f;
    [SerializeField] private Transform CenterPillar = default;
    [SerializeField] private Transform FinishLine = default;
    [SerializeField] private float levelMinLength = 10f;
    [SerializeField] private float levelMaxLength = 20f;
    [SerializeField] private Material normalMat = default;
    [SerializeField] private Material damageMat = default;
    [SerializeField] private AudioClip winAC = default;

    [Header("UI")]
    [SerializeField] private GameObject lostUI = default;
    [SerializeField] private GameObject WONUI = default;

    private AudioSource myAC;

    void Awake()
    {
        Instance = this;
        myAC = GetComponent<AudioSource>();
        myAC.clip = winAC;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        float levelLength = Random.Range(levelMinLength, levelMaxLength);

        int numberOfPlatforms = Mathf.CeilToInt(levelLength / distanceBetweenPlatforms);

        levelLength = numberOfPlatforms * distanceBetweenPlatforms;

        CenterPillar.localScale = new Vector3(1, levelLength + 1, 1);

        FinishLine.position = new Vector3(0, -levelLength, 0);

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            GameObject p = Instantiate(Platform, new Vector3(0, -distanceBetweenPlatforms * i, 0), Quaternion.identity);
            Platform platformScript = p.GetComponent<Platform>();

            List<int> allTileIndecies = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<int> damageTileIndecies = new List<int>();

            int damageTilesCount = Random.Range(0, 5);

            for (int j = 0; j < damageTilesCount; j++)
            {
                int randomIndex = Random.Range(0, allTileIndecies.Count);

                damageTileIndecies.Add(allTileIndecies[randomIndex]);

                allTileIndecies.RemoveAt(randomIndex);
            }

            platformScript.Initialize(damageTileIndecies, normalMat, damageMat);
        }
    }

    public void Lost()
    {
        lostUI.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        GameScore.Instance.ResetScore();
    }

    public void Won()
    {
        WONUI.SetActive(true);

        myAC.Play();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
