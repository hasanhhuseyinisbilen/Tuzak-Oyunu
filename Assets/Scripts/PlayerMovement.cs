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
    private void OnDisable() {
        move.Disable();
        jump.Disable();
    }

    private Collider2D col;
    private Collider2D[] groundHits = new Collider2D[10];

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




        Bounds bounds = col.bounds;
        
        float extraHeight = 0.1f;
        Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y - extraHeight / 2);
        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, extraHeight);


        int hitCount = Physics2D.OverlapBoxNonAlloc(boxCenter, boxSize, 0, groundHits, groundLayer);
        
        isGrounded = false;
        for (int i = 0; i < hitCount; i++)
        {
            var h = groundHits[i];
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
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

    
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f; 
        }
    }
}
