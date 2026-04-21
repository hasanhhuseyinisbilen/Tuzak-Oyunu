using UnityEngine;

public class GasTrigger : MonoBehaviour
{
    [SerializeField] private CircleCollider2D gasCollider;

    private void Start()
    {
        if (gasCollider != null)
        {
            gasCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gasCollider != null)
        {
            gasCollider.enabled = true;
        }
    }
}
