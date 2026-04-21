using UnityEngine;
using System;

public class Level30StopButton : MonoBehaviour
{
    public static event Action OnStopSaws;
    [SerializeField] private float buttonPressOffset = 0.2f;
    private bool isPressed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleInteraction(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleInteraction(other.gameObject);
    }

    private void HandleInteraction(GameObject obj)
    {
        if (obj.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            transform.position += new Vector3(0, -buttonPressOffset, 0);
            
            Level30VerticalMover[] movers30 = FindObjectsOfType<Level30VerticalMover>();
            foreach (var m in movers30)
            {
                m.StopMovement();
            }

            VerticalMover[] moversOrig = FindObjectsOfType<VerticalMover>();
            foreach (var m in moversOrig)
            {
                m.enabled = false;
            }

            OnStopSaws?.Invoke();
        }
    }
}
