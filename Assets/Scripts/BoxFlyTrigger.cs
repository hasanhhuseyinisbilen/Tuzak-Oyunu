using UnityEngine;
using System.Collections.Generic;

public class BoxFlyTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Kutular")]
    [SerializeField] private List<FlyingBox> boxesToTrigger = new List<FlyingBox>();

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            foreach (var box in boxesToTrigger)
            {
                if (box != null) box.FlyUp();
            }
            Debug.Log("Uçan kutular tetiklendi!");
        }
    }
}
