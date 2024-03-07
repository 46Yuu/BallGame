using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject ball;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject pivotX;
    [SerializeField] private GameObject pivotY;
    [SerializeField] private GameObject mainBody;

    [Header("Player Input")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerActions _playerActions;
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private InputActionMap _actionMap;
    [SerializeField] private int _playerIndex;

    [SerializeField] private Animator anim;

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
    private Vector3 rotateInput;
    private float runSpeed;
    private float rotateSpeed;
    private float jumpInput;
    [SerializeField] private float jumpForce;

    private float energy = 100;

    private bool isRunning = false;

    private float actualSpeed;

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
        _playerActions = new PlayerActions();
        _inputActions = _playerInput.actions;
        _actionMap = _inputActions.FindActionMap("gameplay");
        _playerIndex = _playerInput.playerIndex;
        anim = GetComponentInChildren<Animator>();
        cam = GetComponentInChildren<Camera>();
        pivotX = transform.Find("PivotX").gameObject;
        pivotY = pivotX.transform.Find("PivotY").gameObject;
        mainBody = transform.Find("Mesh Object").gameObject;
    }
    // Start is called before the first frame update
    public void Start()
    {
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("Jumped");
        isLandingHash = Animator.StringToHash("IsLanded");
        Emote1Hash = Animator.StringToHash("Emote1");
        Emote2Hash = Animator.StringToHash("Emote2");
        ball = GameController.GetInstance().Ball;
        maxSpeed = 25;
        moveSpeed = 700;
        jumpForce = 100;
        rotateSpeed = 0.75f;
        fallSpeed = 50.0f;
        runSpeed = moveSpeed * 1.8f;
       /* if (GameController.GetInstance().numberOfPlayers == 2)
        {
            switch (myNumber)
            {
                case PlayerNbr.Player_1:
                    cam.rect = new Rect(0, 0, 0.5f, 1);
                    break;
                case PlayerNbr.Player_2:
                    cam.rect = new Rect(0.5f, 0, 0.5f, 1);
                    break;
            }
        }
        else
        {
            switch (myNumber)
            {
                case PlayerNbr.Player_1:
                    cam.rect = new Rect(0, 0, 0.5f, 0.5f);
                    break;
                case PlayerNbr.Player_2:
                    cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
                case PlayerNbr.Player_3:
                    cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    break;
                case PlayerNbr.Player_4:
                    cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
            }
        }*/
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
        if (moveInput != Vector2.zero)
        {
            anim.SetBool(isWalkingHash, true);
            /*float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            mainBody.transform.rotation = Quaternion.Euler(-90, angle, 0) ;*/
            mainBody.transform.rotation = Quaternion.LookRotation(pivotX.transform.rotation * new Vector3(moveInput.x, 0, moveInput.y), Vector3.up);
        }
        else
        {
            anim.SetBool(isWalkingHash, false);
        }
        rotateInput = _actionMap.FindAction("Camera").ReadValue<Vector2>();
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
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
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
        pivotY.transform.Rotate(new Vector3(-rotateInput.y,0,0) * rotateSpeed);
        pivotX.transform.Rotate(new Vector3(0, rotateInput.x, 0) * rotateSpeed);
       
        if (_actionMap.FindAction("Jump").WasPerformedThisFrame())
        {
            Jump();
        }
        if (_actionMap.FindAction("Sprint").IsPressed())
        {
            isRunning = true;
            anim.SetBool(isRunningHash, true);
        }
        else
        {
            isRunning = false;
            anim.SetBool(isRunningHash, false);
        }
        if (_actionMap.FindAction("Emote").WasPerformedThisFrame())
        {
            anim.SetTrigger(Emote2Hash);
        }
        if(ball != null)
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
            rb.AddForce(pivotX.transform.forward * moveInput.y * moveSpeed + pivotX.transform.right * moveInput.x * moveSpeed, ForceMode.Force);
        }
        else if (isAirBorn)
        {
            rb.AddForce((pivotX.transform.forward * moveInput.y * moveSpeed + pivotX.transform.right * moveInput.x * moveSpeed) * 0.1f, ForceMode.Force);
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
