using UnityEngine;

public class PolygonCrusher : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D[] polyColliders;
    private bool isTriggered = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

        if (polyColliders != null)
        {
            foreach (var pc in polyColliders) if (pc != null) pc.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Tetiklendi!");
            isTriggered = true;
            if (polyColliders != null)
            {
                foreach (var pc in polyColliders) if (pc != null) pc.enabled = true;
            }
        }
    }
}
