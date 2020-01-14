using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalItem : MonoBehaviour
{
    private Rigidbody myRigidbody;
    public bool isPickedUp;

    private void Awake()
    { 
        SetInitialReferences();
        isPickedUp = false;
    }

    private void SetInitialReferences()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody collisionRigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (collisionRigidbody != null)
        {
            collisionRigidbody.AddForce(myRigidbody.velocity.normalized * myRigidbody.velocity.magnitude / 2);
        }
    }
}
