using UnityEngine;
using System.Collections.Generic;

public class SawPingPongTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Testereler")]
    [SerializeField] private List<SawPingPong> pingPongSaws = new List<SawPingPong>();
    
    [Header("Ayarlar")]
    [SerializeField] private bool triggerOnlyOnce = true;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && triggerOnlyOnce) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            
            foreach (var saw in pingPongSaws)
            {
                if (saw != null)
                {
                    saw.Activate();
                }
            }
            
            Debug.Log("Ping-Pong Testereler Harekete Geçti: " + gameObject.name);
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
        foreach (var saw in pingPongSaws)
        {
            if (saw != null)
            {
                saw.ResetPosition();
            }
        }
    }
}
