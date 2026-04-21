using UnityEngine;
using System.Collections.Generic;

public class SawSpeedTrigger : MonoBehaviour
{
    [Header("Hızlandırılacak Testereler")]
    [SerializeField] private List<SawMoveRight> rightSawsToBoost = new List<SawMoveRight>();
    [SerializeField] private List<SawMoveLeft> leftSawsToBoost = new List<SawMoveLeft>();
    [SerializeField] private float boostSpeed = 15f;
    [SerializeField] private float boostRotation = -720f;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            
            foreach (var saw in rightSawsToBoost)
            {
                if (saw != null)
                {
                    saw.SetSpeed(boostSpeed, boostRotation);
                    saw.Activate();
                }
            }

            foreach (var saw in leftSawsToBoost)
            {
                if (saw != null)
                {
                    saw.SetSpeed(boostSpeed, boostRotation * -1f); // Sola giden genelde ters döner
                    saw.Activate();
                }
            }
            Debug.Log("Sola ve Sağ giden Testereler Hızlandırıldı!");
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
