using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotationDirection = default;

    void Update()
    {
        transform.Rotate(rotationDirection * Time.deltaTime);
    }
}
