using UnityEngine;

public class SnowBallDropTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody2D snowBallRb;
    private bool dropped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dropped) return;
        if (!other.CompareTag("Player")) return;

        dropped = true;

        snowBallRb.bodyType = RigidbodyType2D.Dynamic;
    }
}
