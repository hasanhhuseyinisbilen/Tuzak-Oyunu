using UnityEngine;

public class BuzSarkiti : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool hasFallen = false;
    [SerializeField] private float minFallDistance = 0.5f;
    [SerializeField] private float fallGravityScale = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void Start()
    {
        startPos = transform.position;
    }

    public void StartFalling()
    {
        if (rb != null && rb.bodyType != RigidbodyType2D.Dynamic)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = fallGravityScale;
        }
    }

    void Update()
    {
        if (transform.position.y < startPos.y - minFallDistance)
        {
            hasFallen = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasFallen)
        {
            Destroy(gameObject);
        }
    }
}
