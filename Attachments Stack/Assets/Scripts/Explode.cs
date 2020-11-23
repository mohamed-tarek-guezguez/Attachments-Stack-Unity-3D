using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> myRigidBodies = default;
    [SerializeField] private float explosionPower = 20f;
    [SerializeField] private Vector3 explosionOffset = default;

    void Start()
    {
        for (int i = 0; i < myRigidBodies.Count; i++)
        {
            myRigidBodies[i].AddExplosionForce(explosionPower, transform.position + explosionOffset, 2);
        }
    }
}
