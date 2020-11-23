using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player = default;
    [SerializeField] private Vector3 offset = default;
    [SerializeField] private float speed = 10f;

    private float playerLowestY;

    void Start()
    {
        playerLowestY = player.position.y;
    }

    void LateUpdate()
    {
        if (player.position.y < playerLowestY)
            playerLowestY = player.position.y;

        transform.position = Vector3.Lerp(transform.position,new Vector3(player.position.x + offset.x, playerLowestY + offset.y, player.position.z + offset.z), speed * Time.deltaTime);
    }
}
