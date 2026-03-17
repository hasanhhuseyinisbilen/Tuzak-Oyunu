using UnityEngine;
using System.Collections.Generic;

public class SawMoveTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Testereler")]
    [SerializeField] private List<SawMoveRight> rightSawsToTrigger = new List<SawMoveRight>();
    [SerializeField] private List<SawMoveLeft> leftSawsToTrigger = new List<SawMoveLeft>();

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            
            // Sağa gidenleri başlat
            foreach (var saw in rightSawsToTrigger)
            {
                if (saw != null) saw.Activate();
            }

            // Sola gidenleri başlat
            foreach (var saw in leftSawsToTrigger)
            {
                if (saw != null) saw.Activate();
            }

            Debug.Log("Sağ ve Sol Testereler Harekete Geçti!");
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
