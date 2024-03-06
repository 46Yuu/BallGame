using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject ball;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActions _playerActions;
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private int _playerIndex;

    [SerializeField] private Animator anim;

    int isWalkingHash;

    public string playerName;
    //[SerializeField] private Camera cam;
    public Vector3 velocity;

    private float baseDrag = 5;
    private bool isMoving = false;
    private bool isAirBorn = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isFalling = false;

    [SerializeField] bool canDoubleJump = false;
    private float moveInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallSpeed;
    private Vector3 rotateInput;
    private float rotateSpeed;
    private float jumpInput;
    [SerializeField] private float jumpForce;

    private int maxSpeed;
    [SerializeField] private float dashTimer;
    [SerializeField] private float dashCoolDown;
    [SerializeField] private bool isGrounded = false;

    enum PlayerNbr { Player_1, Player_2, Player_3, Player_4 };

    [SerializeField] private PlayerNbr myNumber;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        arrow = transform.Find("Arrow").gameObject;
        _playerInput = GetComponent<PlayerInput>();
        _playerActions = new InputActions();
        _inputActions = _playerInput.actions;
        _playerIndex = _playerInput.playerIndex;
        anim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("IsWalking");
        ball = GameController.GetInstance().Ball;
        maxSpeed = 25;
        moveSpeed = 1200;
        jumpForce = 100;
        rotateSpeed = 0.75f;
        fallSpeed = 50.0f;
        //cam = GetComponent<Camera>();
    }
    void Update()
    {
        if (rb.velocity.y > 0 && !isGrounded)
        {
            isJumping = true;
            isFalling = false;
        }
        else if (rb.velocity.y < 0 && isJumping && !isGrounded)
        {
            isJumping = false;
            isFalling = true;
        }
        if (myNumber == PlayerNbr.Player_1)
        {
            rotateInput = new Vector3(0, Input.GetAxis("Player1H"), 0);
            moveInput = Input.GetAxis("Player1V");
            if(moveInput != 0)
            {
                anim.SetBool(isWalkingHash, true);
            }
            else
            {
                anim.SetBool(isWalkingHash, false);
            }
        }
        else if (myNumber == PlayerNbr.Player_2)
        {
            rotateInput = new Vector3(0, Input.GetAxis("Player2H"), 0);
            moveInput = Input.GetAxis("Player2V");
            //jumpInput = Input.GetAxis("JumpP2");
        }
        if (isGrounded)
        {
            isJumping = false;
            isFalling = false;
            rb.drag = baseDrag;
        }
        else
        {
            rb.drag = 0;
        }
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
            isAirBorn = false;
        }
        else
        {
            isGrounded = false;
            isAirBorn = true;
        }
        transform.Rotate(rotateInput * rotateSpeed, Space.Self);
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        SetArrowDirection();
    }
    void FixedUpdate()
    {
        velocity = rb.velocity;
        if (isGrounded)
        {
            rb.AddForce(transform.forward * moveInput * moveSpeed, ForceMode.Force);
        }
        else if (isAirBorn)
        {
            rb.AddForce(transform.forward * moveInput * moveSpeed * 0.1f, ForceMode.Force);
        }
        if (isFalling)
        {
            rb.AddForce(Vector3.down * fallSpeed, ForceMode.Force);
        }
    }

    void Jump()
    {
        if (isGrounded || canDoubleJump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            if (!canDoubleJump)
            {
                canDoubleJump = true;
            }
            else
            {
                canDoubleJump = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Ground"))
        {
            isGrounded = false;
        }
    }

    private void SetArrowDirection()
    {
        arrow.transform.rotation = Quaternion.LookRotation(ball.transform.position - transform.position, Vector3.up) * Quaternion.Euler(90, 0, 0);
    }
}
