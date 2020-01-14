using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Boy,
    Girl
}

public class TestRigidbodyController : MonoBehaviour
{
    public PlayerType playerType;

    [Header("Movement")]
    [SerializeField] private float speed;

    private Rigidbody myRigidbody;

    Camera myCamera;


    private void Awake()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCamera = Camera.main;
    }

    private void Update()
    {
        float horizontal = 1;//hInput.GetAxis(playerType.ToString() + "Horizontal");
        float vertical = -1.0f;//hInput.GetAxis(playerType.ToString() + "Vertical");


       
        //camera forward and right vectors:
        var forward = myCamera.transform.forward;
        var right = myCamera.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * vertical + right * horizontal;

        myRigidbody.velocity = desiredMoveDirection * speed;
    }
}
