using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject ball;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerActions _playerActions;
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private InputActionMap _actionMap;
    [SerializeField] private int _playerIndex;

    [SerializeField] private Animator anim;
    [SerializeField] private Slider energySlider;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isLandingHash;
    int Emote1Hash;
    int Emote2Hash;
    public string playerName;
    //[SerializeField] private Camera cam;
    public Vector3 velocity;

    private float baseDrag = 5;
    private bool isMoving = false;
    private bool isAirBorn = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isFalling = false;

    [SerializeField] bool canDoubleJump = false;
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallSpeed;

    private float runSpeed;
    private Vector3 rotateInput;
    private float rotateSpeed;
    private float jumpInput;
    [SerializeField] private float jumpForce;


    private bool isRunning = false;
    private bool canRun = true;

    private float actualSpeed;

    private int maxSpeed;
    [SerializeField] private float dashTimer;
    [SerializeField] private float dashCoolDown;
    [SerializeField] private bool isGrounded = false;

    enum PlayerNbr { Player_1, Player_2, Player_3, Player_4 };

    [SerializeField] private PlayerNbr myNumber;

    [SerializeField] private float maxEnergy;
    private float energy;
    [SerializeField] private float energyRegen;
    [SerializeField] private float energyDrain;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        arrow = transform.Find("Arrow").gameObject;
        _playerInput = GetComponent<PlayerInput>();
        _playerActions = new PlayerActions();
        _inputActions = _playerInput.actions;
        _actionMap = _inputActions.FindActionMap("gameplay");
        _playerIndex = _playerInput.playerIndex;
        anim = GetComponentInChildren<Animator>();
        energySlider = GameObject.FindWithTag("EnergyP1").GetComponent<Slider>();
        energy = maxEnergy;
        energySlider.maxValue = maxEnergy;
    }
    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("Jumped");
        isLandingHash = Animator.StringToHash("IsLanded");
        Emote1Hash = Animator.StringToHash("Emote1");
        Emote2Hash = Animator.StringToHash("Emote2");
        ball = GameController.GetInstance().Ball;
        maxSpeed = 25;
        moveSpeed = 800;
        jumpForce = 100;
        rotateSpeed = 0.75f;
        fallSpeed = 50.0f;
        runSpeed = moveSpeed * 2f;
        //cam = GetComponent<Camera>();
    }
    void Update()
    {
        if (rb.velocity.y > 0 && !isGrounded)
        {
            isJumping = true;
            isFalling = false;
        }
        else if (rb.velocity.y < 0 && !isGrounded)
        {
            isJumping = false;
            isFalling = true;
        }
        moveInput = _actionMap.FindAction("Move").ReadValue<Vector2>();
        if(moveInput != Vector2.zero)
        {
            anim.SetBool(isWalkingHash, true);
        }
        else
        {
            anim.SetBool(isWalkingHash, false);
        }
        //rotateInput = new Vector3(0, Input.GetAxis("Player1H"), 0);
        if (isGrounded)
        {
            isJumping = false;
            isFalling = false;
            //anim.ResetTrigger(isJumpingHash);
            rb.drag = baseDrag;
        }
        else
        {
            rb.drag = 0;
        }
        if (Physics.Raycast(transform.position, Vector3.down, 0.4f, LayerMask.GetMask("Ground")))
        {
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                anim.SetBool(isLandingHash, true);
            }
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
        if(_actionMap.FindAction("Jump").WasPerformedThisFrame())
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.LeftControl) && energy > 0 && canRun)
        {
            energy -= energyDrain;
            energySlider.value = energy;
            isRunning = true;
            GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(GetComponentInChildren<Camera>().fieldOfView, 80, 3f * Time.deltaTime) ;
            anim.SetBool(isRunningHash, true);
        }
        else{
            if (energy < maxEnergy && !isRunning)
            {
                energy += energyRegen;
                energySlider.value = energy;
            }
            if (energy <= maxEnergy/3)
            {
                canRun = false;
            }
            else
            {
                canRun = true;
            } 
            isRunning = false;
            GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(GetComponentInChildren<Camera>().fieldOfView, 60, 3f * Time.deltaTime);
            anim.SetBool(isRunningHash, false);
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger(Emote1Hash);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            anim.SetTrigger(Emote2Hash);
        }
        SetArrowDirection();
    }
    void FixedUpdate()
    {
        velocity = rb.velocity;
        actualSpeed = moveSpeed;
        if (isRunning)
        {
            actualSpeed = runSpeed;
        }
        if (isGrounded)
        {
            rb.AddForce(new Vector3(-moveInput.x * actualSpeed, 0,-moveInput.y * actualSpeed), ForceMode.Force);
        }
        else if (isAirBorn)
        {
            rb.AddForce(new Vector3(-moveInput.x * actualSpeed, 0,-moveInput.y * actualSpeed) * 0.1f, ForceMode.Force);
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
            anim.SetBool(isLandingHash, false);
            anim.SetTrigger(isJumpingHash);
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
    private void OnEnable()
    {
        _actionMap.Enable();
    }
    private void OnDisable()
    {
        _actionMap.Disable();
    }
    private void SetArrowDirection()
    {
        arrow.transform.rotation = Quaternion.LookRotation(ball.transform.position - transform.position, Vector3.up) * Quaternion.Euler(90, 0, 0);
    }
}
