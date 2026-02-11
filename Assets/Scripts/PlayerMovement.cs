using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public InputAction move;
    public InputAction jump;

    Rigidbody2D rb;

    float moveInput;
    bool jumpRequested;
    bool isGrounded;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheckTransform;
    

    float jumpBufferTime = 0.1f;
    float jumpBufferCounter;


    float coyoteTime = 0.1f;
    float coyoteTimeCounter;

    void OnEnable()
    {
        move.Enable();
        jump.Enable();
    }

    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>(); 
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput = move.ReadValue<float>();

    
        if (moveInput > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (groundLayer.value == 0) groundLayer = ~0;


        Bounds bounds = col.bounds;
        
        float extraHeight = 0.1f;
        Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y - extraHeight / 2);
        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, extraHeight);


        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, groundLayer);
        
        isGrounded = false;
        foreach (var h in hits)
        {
            if (h.gameObject != gameObject && !h.isTrigger)
            {
                isGrounded = true;
                break;
            }
        }

    
 
        Debug.DrawLine(new Vector3(bounds.min.x, bounds.min.y, 0), new Vector3(bounds.max.x, bounds.min.y, 0));
        Debug.DrawLine(new Vector3(bounds.min.x, bounds.min.y - extraHeight, 0), new Vector3(bounds.max.x, bounds.min.y - extraHeight, 0));
        
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (jump.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

    
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f; 
        }
    }
}
