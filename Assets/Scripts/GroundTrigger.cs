using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    [Header("Düşecek Zemin Ayarı")]
    [SerializeField] private DusenZemin targetGround;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        if (targetGround != null)
        {
            triggered = true;
            targetGround.StartFalling();
            Debug.Log("Zemin tetiklendi!");
        }
        else
        {
            Debug.LogWarning("Düşürülecek zemin atanmamış (GroundTrigger)!");
        }
    }
}
