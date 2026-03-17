using UnityEngine;
using System.Collections.Generic;

public class SpikeFlyTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Dikenler")]
    [SerializeField] private List<FlyingSpike> spikesToTrigger = new List<FlyingSpike>();

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
                    spike.FlyUp();
                }
            }
            
            Debug.Log("Uçan dikenler tetiklendi!");
        }
    }

    // Gerekirse trigger'ı sıfırlamak için
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
