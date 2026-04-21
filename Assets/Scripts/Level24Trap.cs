using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Level24Trap : MonoBehaviour
{
    [SerializeField] private BoxCollider2D[] boxColliders;
    private bool isTriggered = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

        if (boxColliders != null)
        {
            foreach (var bc in boxColliders) if (bc != null) bc.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered || boxColliders == null || boxColliders.Length == 0) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Level 24 Box Trap Activated!");
            isTriggered = true;
            foreach (var bc in boxColliders) if (bc != null) bc.enabled = true;
        }
    }
}
