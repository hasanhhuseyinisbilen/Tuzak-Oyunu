using UnityEngine;

public class SawTrigger : MonoBehaviour
{
    [SerializeField] private SawMoveLeft saw;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        saw.move = true;
    }
}
