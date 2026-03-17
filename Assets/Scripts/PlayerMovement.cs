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

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheckTransform;
    

    float jumpBufferTime = 0.1f;
    float jumpBufferCounter;


    float coyoteTime = 0.1f;
    float coyoteTimeCounter;

    [Header("Görünüş Ayarları")]
    [SerializeField] private bool faceRightAtStart = true;
    [SerializeField] private bool spriteFacingLeftByDefault = true;

    private Vector3 initialScale;

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
        
        initialScale = transform.localScale;

        // Başlangıçta sağa mı baksın?
        if (faceRightAtStart)
        {
            transform.localScale = new Vector3(spriteFacingLeftByDefault ? -initialScale.x : initialScale.x, initialScale.y, initialScale.z);
        }

        // --- AUDIO LISTENER KONTROLÜ ---
        // Eğer sahnede kamerada zaten bir listener varsa ve karakterde de varsa, karakterdekini sustur
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            AudioListener playerListener = GetComponent<AudioListener>();
            if (playerListener != null)
            {
                Destroy(playerListener);
                Debug.Log("Fazlalık AudioListener karakterden silindi!");
            }
        }

        // --- GROUND LAYER KONTROLÜ ---
        if (groundLayer == 0)
        {
            Debug.LogWarning("DİKKAT: 'Ground Layer' seçilmemiş! Player (Inspector) üzerinden katman seçmelisin.");
        }
    }

    [Header("Debug")]
    public bool isGrounded; // Inspector'dan görmek için public yaptık

    void Update()
    {
        moveInput = move.ReadValue<float>();

        if (moveInput > 0.1f) // Sağa git
        {
            transform.localScale = new Vector3(spriteFacingLeftByDefault ? -initialScale.x : initialScale.x, initialScale.y, initialScale.z);
        }
        else if (moveInput < -0.1f) // Sola git
        {
            transform.localScale = new Vector3(spriteFacingLeftByDefault ? initialScale.x : -initialScale.x, initialScale.y, initialScale.z);
        }

        // Yer Kontrolü (Ground Check) - LAYERSIZ VERSİYON
        Bounds bounds = col.bounds;
        float extraHeight = 0.1f;
        Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y - extraHeight / 2);
        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, extraHeight);

        // Herhangi bir şeye çarpıyor mu bak (Layer sildik)
        Collider2D[] results = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
        isGrounded = false;
        
        foreach (var hitCollider in results)
        {
            // Kendimize veya tetikleyici (trigger) objelere çarpmıyorsak yerdedir
            if (hitCollider.gameObject != gameObject && !hitCollider.isTrigger)
            {
                isGrounded = true;
                break;
            }
        }

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (jump.WasPressedThisFrame())
        {
            if (!isGrounded) Debug.Log("Zıplama basıldı ama yer algılanmadı!");
            else Debug.Log("Zıplama OK! (YERDE)");
            
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Görsel debug
        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawLine(new Vector3(boxCenter.x - boxSize.x/2, boxCenter.y + boxSize.y/2, 0), new Vector3(boxCenter.x + boxSize.x/2, boxCenter.y + boxSize.y/2, 0), rayColor);
        Debug.DrawLine(new Vector3(boxCenter.x - boxSize.x/2, boxCenter.y - boxSize.y/2, 0), new Vector3(boxCenter.x + boxSize.x/2, boxCenter.y - boxSize.y/2, 0), rayColor);
    }

    void FixedUpdate()
    {
        // Kaygan zemin kontrolü (Tag üzerinden)
        bool isOnSlippery = false;
        if (isGrounded)
        {
            Bounds bounds = col.bounds;
            float extraHeight = 0.15f;
            Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y - extraHeight / 2);
            Vector2 boxSize = new Vector2(bounds.size.x * 0.8f, extraHeight);
            Collider2D[] results = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
            
            foreach (var hit in results)
            {
                if (hit.gameObject != gameObject && !hit.isTrigger && hit.CompareTag("Slippery"))
                {
                    isOnSlippery = true;
                    break;
                }
            }
        }

        if (!isOnSlippery)
        {
            // Orijinal keskin hareket
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            // Kaygan zemin hareketi (SÜPER HIZLI ve PATİNAJLI)
            float slipperySpeedMultiplier = 2.0f; // 2 Kat Hız!
            float targetX = moveInput * (moveSpeed * slipperySpeedMultiplier);
            
            float speedDiff = targetX - rb.linearVelocity.x;
            float movement;

            if (Mathf.Abs(moveInput) > 0.01f)
            {
                // Zıt yöne mi basıyoruz? (Patinaj kontrolü)
                bool isTurning = (moveInput > 0 && rb.linearVelocity.x < -0.1f) || (moveInput < 0 && rb.linearVelocity.x > 0.1f);
                
                if (isTurning)
                {
                    // PATİNAJ: Zıt yöne basınca karakter hemen dönemez, çok yavaş yavaşlar (0.8f çarpanı)
                    movement = speedDiff * 0.8f; 
                }
                else
                {
                    // HIZLANMA: Agresif fırlama (Hemen o hıza ulaşmak ister)
                    movement = speedDiff * 20f; 
                }
            }
            else
            {
                // DURMA: Sabun gibi kayma (0.4f çarpanı)
                movement = speedDiff * 0.4f; 
            }

            rb.AddForce(new Vector2(movement, 0), ForceMode2D.Force);
            
            // Maksimum hız sınırı (Çok uçmaması için)
            float maxSlipperySpeed = moveSpeed * slipperySpeedMultiplier;
            if (Mathf.Abs(rb.linearVelocity.x) > maxSlipperySpeed)
            {
                rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxSlipperySpeed, rb.linearVelocity.y);
            }
        }

    
        // Zıplama Kontrolü: Kaygan zeminde zıplama kapalı!
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isOnSlippery)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f; 
        }
        else if (jumpBufferCounter > 0f && isOnSlippery)
        {
            // Buzda zıplamaya çalışınca ufak bir mesaj veya görsel efekt eklenebilir
            // Şu an sadece zıplamayı yutuyoruz.
            jumpBufferCounter = 0f; 
        }
    }
}
