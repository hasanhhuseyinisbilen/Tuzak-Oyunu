using UnityEngine;

public class StormTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool stopOnExit = true;
    [SerializeField] private bool once = false;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (once && triggered) return;

        if (other.CompareTag("Player"))
        {
            if (SnowStormController.Instance != null)
            {
                SnowStormController.Instance.StartStorm();
                triggered = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (stopOnExit && other.CompareTag("Player"))
        {
            if (SnowStormController.Instance != null)
            {
                SnowStormController.Instance.StopStorm();
            }
        }
    }
}
