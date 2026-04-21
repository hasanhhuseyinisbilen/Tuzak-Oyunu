using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    [Header("Tetiklenecek Obje")]
    [SerializeField] private ObjectMoveRight targetMoveScript;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (targetMoveScript != null)
            {
                targetMoveScript.Activate();
                hasTriggered = true;
                Debug.Log("Triggered!");
            }
        }
    }
}
