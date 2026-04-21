using UnityEngine;

public class SimpleCeilingMover : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 0.5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Use MovePosition for maximum performance (Safe since we set triggers in Generator)
            rb.MovePosition(rb.position + Vector2.down * fallSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.position += Vector3.down * fallSpeed * Time.fixedDeltaTime;
        }
    }

    public void SetSpeed(float speed)
    {
        fallSpeed = speed;
    }
}
