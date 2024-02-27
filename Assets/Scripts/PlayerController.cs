using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    //[SerializeField] private Camera cam;
    public Vector3 velocity;
    [SerializeField] private float moveForce;
    [SerializeField] private float moveSpeed;
    private int maxSpeed;
    [SerializeField] private Vector3 rotateForce;
    [SerializeField] private float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = 15;
        moveSpeed = 15;
        rotateSpeed = 5;
        rb = GetComponent<Rigidbody>();
        //cam = GetComponent<Camera>();
    }
    void Update()
    {
        rotateForce = new Vector3(0,Input.GetAxis("Horizontal"),0);
        moveForce = Input.GetAxis("Vertical");
        Mathf.Clamp(rb.velocity.magnitude, 0, maxSpeed);
    }
    void FixedUpdate()
    {
        velocity = rb.velocity;
        rb.AddForce(transform.forward * moveForce * moveSpeed, ForceMode.Force);
        transform.Rotate(rotateForce * rotateSpeed ,Space.Self);
    }
}
