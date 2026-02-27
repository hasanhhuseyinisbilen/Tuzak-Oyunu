using UnityEngine;

public class TriggerHorizontal : MonoBehaviour
{
    [SerializeField] private SawMoveLeft saw;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (saw != null)
        {
            saw.Activate();
        }
    }
}
