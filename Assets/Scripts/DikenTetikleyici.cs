using UnityEngine;

public class DikenTetikleyici : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private DusenDiken targetSpike;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            
            if (targetSpike != null)
            {
                targetSpike.StartFalling();
            }
        }
    }
}
