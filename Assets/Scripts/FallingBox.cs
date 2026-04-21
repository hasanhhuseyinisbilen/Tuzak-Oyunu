using UnityEngine;

public class FallingBox : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float fallGravityScale = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void StartFalling()
    {
        if (rb != null && rb.bodyType != RigidbodyType2D.Dynamic)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = fallGravityScale;
        }
    }

    public void Fall() // Keep for compatibility
    {
        StartFalling();
    }
}
