using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public InputAction move;
    public InputAction jump;

    public static PlayerMovement2D Instance;
    
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 initialScale;

    [Header("Giriş Durumu")]
    [SerializeField] private float moveInput;
    [SerializeField] private float mobileMoveInput;
    private bool mobileJumpRequested;
    private bool jumpRequested;
    private float launchTimer;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheckTransform;
    public bool isGrounded;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    [Header("Görünüş Ayarları")]
    [SerializeField] private bool faceRightAtStart = true;
    [SerializeField] private bool spriteFacingLeftByDefault = true;

    [Header("Ses Ayarları")]
    [SerializeField] private AudioClip walkClip;
    private AudioSource audioSource;
    private Animator animator;

    void OnEnable()
    {
        move.Enable();
        jump.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>(); 
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        
        initialScale = transform.localScale;

        if (faceRightAtStart)
        {
            transform.localScale = new Vector3(spriteFacingLeftByDefault ? -initialScale.x : initialScale.x, initialScale.y, initialScale.z);
        }

        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            AudioListener playerListener = GetComponent<AudioListener>();
            if (playerListener != null)
            {
                Destroy(playerListener);
            }
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.clip = walkClip;
        audioSource.loop = true;
    }

    void Update()
    {
        PlayerDead deadScript = GetComponent<PlayerDead>();
        if (deadScript != null && deadScript.isDead)
        {
            if (audioSource != null && audioSource.isPlaying && audioSource.clip == walkClip)
            {
                audioSource.Stop();
            }
            if (animator != null) animator.speed = 0;
            return;
        }

        float keyboardInput = move.ReadValue<float>();
        moveInput = Mathf.Clamp(keyboardInput + mobileMoveInput, -1f, 1f);

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            float targetX = (moveInput > 0) ? (spriteFacingLeftByDefault ? -1 : 1) : (spriteFacingLeftByDefault ? 1 : -1);
            transform.localScale = new Vector3(targetX * Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }

        Bounds bounds = col.bounds;
        float checkDepth = 0.4f;
        Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y - checkDepth / 2);
        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, checkDepth);

        LayerMask maskToUse = (groundLayer == 0) ? ~0 : groundLayer;
        Collider2D[] choices = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, maskToUse);
        
        isGrounded = false;
        foreach (var choice in choices)
        {
            if (choice.gameObject != gameObject && !choice.isTrigger)
            {
                isGrounded = true;
                break;
            }
        }

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (jump.WasPressedThisFrame() || mobileJumpRequested)
        {
            jumpBufferCounter = jumpBufferTime;
            mobileJumpRequested = false;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (launchTimer > 0) launchTimer -= Time.deltaTime;

        UpdateAnimations();
        UpdateWalkingSound();
    }

    void FixedUpdate()
    {
        PlayerDead deadScript = GetComponent<PlayerDead>();
        if (deadScript != null && deadScript.isDead) return;

        bool isOnSlippery = false;
        if (isGrounded)
        {
            Bounds bounds = col.bounds;
            Vector2 sBoxSize = new Vector2(bounds.size.x * 0.8f, 0.4f);
            Vector2 sBoxCenter = new Vector2(bounds.center.x, bounds.min.y - 0.2f);
            Collider2D[] results = Physics2D.OverlapBoxAll(sBoxCenter, sBoxSize, 0f);
            
            foreach (var hit in results)
            {
                if (hit.gameObject != gameObject && !hit.isTrigger && hit.CompareTag("Slippery"))
                {
                    isOnSlippery = true;
                    break;
                }
            }
        }

        if (launchTimer <= 0)
        {
            if (!isOnSlippery)
            {
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                float targetX = moveInput * (moveSpeed * 2.0f);
                float currentX = rb.linearVelocity.x;
                float accel = (Mathf.Abs(moveInput) > 0.01f) ? 25f : 5f;
                float newX = Mathf.Lerp(currentX, targetX, accel * Time.fixedDeltaTime);
                rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
            }
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            if (!isOnSlippery)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }

                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
            }
            else
            {
                jumpBufferCounter = 0f;
            }
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("speed", Mathf.Abs(moveInput));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void UpdateWalkingSound()
    {
        if (walkClip == null || audioSource == null) return;

        bool isMoving = isGrounded && Mathf.Abs(moveInput) > 0.1f;

        if (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    public void SetMobileMoveInput(float value)
    {
        mobileMoveInput = value;
    }

    public void MobileJumpDown()
    {
        mobileJumpRequested = true;
    }

    public void ApplyLaunch(Vector2 force, float duration)
    {
        launchTimer = duration;
        rb.linearVelocity = force;
    }
}