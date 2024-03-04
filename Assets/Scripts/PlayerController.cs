using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider collider;


    public string playerName;
    //[SerializeField] private Camera cam;
    public Vector3 velocity;

    private float baseDrag = 5;

    private float moveInput;
    [SerializeField] private float moveSpeed;
    private Vector3 rotateInput;
    private float rotateSpeed;
    private float jumpInput;
    [SerializeField] private float jumpForce;

    private int maxSpeed;
    [SerializeField] private float dashTimer;
    [SerializeField] private float dashCoolDown;
    [SerializeField] private bool isGrounded = false;

    enum PlayerNbr {Player_1, Player_2, Player_3, Player_4};

    [SerializeField] private PlayerNbr myNumber;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = 25;
        moveSpeed = 50;
        rotateSpeed = 0.75f;
        //cam = GetComponent<Camera>();
    }
    void Update()
    {
        if(myNumber == PlayerNbr.Player_1)
        {
            rotateInput = new Vector3(0,Input.GetAxis("Player1H"),0);
            moveInput = Input.GetAxis("Player1V");
            jumpInput = Input.GetAxis("JumpP1");
        }
        else if(myNumber == PlayerNbr.Player_2)
        {
            rotateInput = new Vector3(0, Input.GetAxis("Player2H"), 0);
            moveInput = Input.GetAxis("Player2V");
            jumpInput = Input.GetAxis("JumpP2");
        }
        if (isGrounded)
        {

            rb.drag = baseDrag;
        }
        else
        {
            rb.drag = 0;
        }
        if (Physics.Raycast(transform.position, Vector3.down, collider.height / 2 + 0.01f, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        transform.Rotate(rotateInput * rotateSpeed ,Space.Self);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
    void FixedUpdate()
    {
        velocity = rb.velocity;
        rb.AddForce(transform.forward * moveInput * moveSpeed, ForceMode.Force);
        Jump();
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce * jumpInput, ForceMode.Impulse);
        }
    }
}
