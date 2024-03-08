using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject ball;
    [SerializeField] public Camera cam;
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
    [SerializeField] public Slider energySlider;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isLandingHash;

    int DashedHash;

    bool usingJetpack = false;
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


    private bool isRunning = false;
    private bool canRun = true;
    private bool isPounding = false;

    private float actualSpeed;

    private int maxSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private bool isGrounded = false;

    public enum PlayerNbr { Player_1, Player_2, Player_3, Player_4 };

    [SerializeField] public PlayerNbr myNumber;

    [SerializeField] private float maxEnergy;
    private float energy;
    [SerializeField] private float energyRegen;
    [SerializeField] private float energyDrain;
    [SerializeField] private ParticleSystem jetpackParticles;
    [SerializeField] private ParticleSystem groundPoundParticles;

    private bool canPlay = false;
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
        jetpackParticles = GetComponentInChildren<ParticleSystem>();
        foreach (ParticleSystem child in GetComponentsInChildren<ParticleSystem>())
        {
            if (child.tag == "PoundParticles")
            {
                groundPoundParticles = child;
            }
        }
        //energySlider = GameObject.FindWithTag("EnergyP1").GetComponent<Slider>();
        energy = maxEnergy;
        cam = GetComponentInChildren<Camera>();
        pivotX = transform.Find("PivotX").gameObject;
        pivotY = pivotX.transform.Find("PivotY").gameObject;
        mainBody = transform.Find("Mesh Object").gameObject;
        cam.enabled = !cam.enabled;
    }
    // Start is called before the first frame update
    public void Init()
    {
        cam.enabled = true;
        canPlay = true;
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("Jumped");
        isLandingHash = Animator.StringToHash("IsLanded");
        DashedHash = Animator.StringToHash("Dashed");
        Emote1Hash = Animator.StringToHash("Emote1");
        Emote2Hash = Animator.StringToHash("Emote2");
        ball = GameController.GetInstance().Ball;
        maxSpeed = 25;
        moveSpeed = 800;
        jumpForce = 100;
        rotateSpeed = 0.75f;
        fallSpeed = 50.0f;
        runSpeed = moveSpeed * 2f;
        switch (myNumber)
        {
            case PlayerNbr.Player_1:
                energySlider = GameObject.FindWithTag("EnergyP1").GetComponent<Slider>();
                break;
            case PlayerNbr.Player_2:
                energySlider = GameObject.FindWithTag("EnergyP2").GetComponent<Slider>();
                break;
            case PlayerNbr.Player_3:
                energySlider = GameObject.FindWithTag("EnergyP3").GetComponent<Slider>();
                break;
            case PlayerNbr.Player_4:
                energySlider = GameObject.FindWithTag("EnergyP4").GetComponent<Slider>();
                break;

        }
        energySlider.maxValue = maxEnergy;
        if (GameController.GetInstance().numberOfPlayers == 2)
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
        }
    }
    void Update()
    {
        if (canPlay)
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
                if(isPounding)
                {
                    groundPoundParticles.Play();
                    isPounding = false;
                }
                isGrounded = true;
                isAirBorn = false;
            }
            else
            {
                isGrounded = false;
                isAirBorn = true;
            }
            pivotY.transform.Rotate(new Vector3(-rotateInput.y, 0, 0) * rotateSpeed);
            pivotX.transform.Rotate(new Vector3(0, rotateInput.x, 0) * rotateSpeed);

            //Dash
            if (_actionMap.FindAction("Dive").WasPerformedThisFrame() && energy - maxEnergy / 2 > 0 && !isAirBorn)
            {
                rb.AddForce(pivotX.transform.forward * dashSpeed, ForceMode.Impulse);
                energy -= maxEnergy / 2;
                anim.SetTrigger(DashedHash);
            }

            //Jump
            if (_actionMap.FindAction("Jump").WasPerformedThisFrame())
            {
                Jump();
            }

            //Jetpack
            if (_actionMap.FindAction("Jump").ReadValue<float>() != 0)
            {
                Jetpack();
            }
            else
            {
                jetpackParticles.Stop();
                usingJetpack = false;
            }

            //GroundPound
            if (_actionMap.FindAction("Pound").WasPerformedThisFrame() && isAirBorn && !isPounding)
            {
                isPounding = true;
                rb.velocity = new Vector3(0, -75, 0);
            }

            //Sprint
            if (_actionMap.FindAction("Sprint").IsPressed() && energy > 0 && canRun)
            {
                energy -= energyDrain;
                energySlider.value = energy;
                isRunning = true;
                GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(GetComponentInChildren<Camera>().fieldOfView, 80, 3f * Time.deltaTime);
                anim.SetBool(isRunningHash, true);
            }
            else
            {
                isRunning = false;
                if (energy < maxEnergy && !isRunning && !usingJetpack)
                {
                    energy += energyRegen;
                    energySlider.value = energy;
                }
                if (energy <= maxEnergy / 3)
                {
                    canRun = false;
                }
                else
                {
                    canRun = true;
                }
                GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(GetComponentInChildren<Camera>().fieldOfView, 60, 3f * Time.deltaTime);
                anim.SetBool(isRunningHash, false);
            }
            if (_actionMap.FindAction("Emote").WasPerformedThisFrame())
            {
                anim.SetTrigger(Emote2Hash);
            }
            if (_actionMap.FindAction("Shoot").WasPerformedThisFrame())
            {
                anim.SetTrigger(Emote1Hash);
            }
            if (_actionMap.FindAction("Reset").WasPerformedThisFrame())
            {
                transform.position = new Vector3(transform.position.x, transform.position.y+15, transform.position.z);
            }
            if (ball != null)
                SetArrowDirection();
        }
    }
    void FixedUpdate()
    {
        if (canPlay)
        {
            velocity = rb.velocity;
            actualSpeed = moveSpeed;
            if (isRunning)
            {
                actualSpeed = runSpeed;
            }
            if (isGrounded)
            {
                rb.AddForce(pivotX.transform.forward * moveInput.y * actualSpeed + pivotX.transform.right * moveInput.x * actualSpeed, ForceMode.Force);
            }
            else if (isAirBorn)
            {
                rb.AddForce((pivotX.transform.forward * moveInput.y * actualSpeed + pivotX.transform.right * moveInput.x * actualSpeed) * 0.2f, ForceMode.Force);
            }
            if (isFalling)
            {
                rb.AddForce(Vector3.down * fallSpeed, ForceMode.Force);
            }
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

    private void Jetpack()
    {
        if (energy > 0 && isAirBorn)
        {
            if (!jetpackParticles.isPlaying)
            {
                jetpackParticles.Play();
            }
            rb.AddForce(transform.up * jumpForce * 0.3f, ForceMode.Force);
            usingJetpack = true;
            energy -= energyDrain;
            energySlider.value = energy;
        }
        else
        {
            jetpackParticles.Stop();
            usingJetpack = false;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && _actionMap.FindAction("Shoot").WasPerformedThisFrame())
        {
            ball.gameObject.GetComponent<Rigidbody>().AddForce((ball.gameObject.transform.position - transform.position).normalized * (energy), ForceMode.Impulse);
            ball.gameObject.GetComponent<BallController>().latesPlayerHit = gameObject;
            energy = 0;
            energySlider.value = energy;
        }
    }
}
