using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    //[SerializeField] private Camera cam;
    public Vector3 velocity;
    private float moveForce;
    [SerializeField] private float moveSpeed;
    private int maxSpeed;
    [SerializeField] private Vector3 rotateForce;
    [SerializeField] private float rotateSpeed;
    enum PlayerNbr {Player_1, Player_2};
    [SerializeField] private PlayerNbr myNumber;
    [SerializeField] private float sprintTimer;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = 10;
        moveSpeed = 100;
        rotateSpeed = 3;
        rb = GetComponent<Rigidbody>();
        //cam = GetComponent<Camera>();
    }
    void Update()
    {
        if(myNumber == PlayerNbr.Player_1)
        {
            rotateForce = new Vector3(0,Input.GetAxis("Player1H"),0);
            moveForce = Input.GetAxis("Player1V");
        }
        else
        {
            rotateForce = new Vector3(0, Input.GetAxis("Player2H"), 0);
            moveForce = Input.GetAxis("Player2V");
        }
        Mathf.Clamp(rb.velocity.magnitude, 0, maxSpeed);
    }
    void FixedUpdate()
    {
        velocity = rb.velocity;
        rb.AddForce(transform.forward * moveForce * moveSpeed, ForceMode.Force);
        transform.Rotate(rotateForce * rotateSpeed ,Space.Self);
    }
}
