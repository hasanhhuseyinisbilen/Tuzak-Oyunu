using UnityEngine;
using System.Collections.Generic;

public class SpikeSlideTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Dikenler")]
    [SerializeField] private List<SlidingSpike> spikesToTrigger = new List<SlidingSpike>();

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            foreach (var spike in spikesToTrigger)
            {
                if (spike != null)
                {
                    spike.TriggerSlide();
                }
            }
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
