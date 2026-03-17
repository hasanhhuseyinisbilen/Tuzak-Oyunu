using UnityEngine;
using System.Collections.Generic;

public class SawFlyTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Testereler")]
    [SerializeField] private List<FlyingSaw> sawsToTrigger = new List<FlyingSaw>();

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            
            foreach (var saw in sawsToTrigger)
            {
                if (saw != null)
                {
                    saw.FlyUp();
                }
            }
            
            Debug.Log("Uçan testereler tetiklendi!");
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
