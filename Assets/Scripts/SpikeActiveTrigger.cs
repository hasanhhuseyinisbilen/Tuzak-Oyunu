using UnityEngine;
using System.Collections.Generic;

public class SpikeActiveTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Dikenler")]
    [SerializeField] private List<TimedSpikeTrap> spikesToActivate = new List<TimedSpikeTrap>();

    private bool hasUsed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasUsed) return;

        if (other.CompareTag("Player"))
        {
            hasUsed = true; // Sadece 1 kere çalışacak
            foreach (var spike in spikesToActivate)
            {
                if (spike != null)
                {
                    spike.ActivateTrap();
                }
            }
            Debug.Log("Zamanlı Dikenler Tetiklendi!");
        }
    }
}
