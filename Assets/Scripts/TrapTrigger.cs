using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [Header("Hedef Tuzak")]
    [SerializeField] private SlidingBoxTrap targetTrap;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (targetTrap != null)
            {
                targetTrap.TriggerTrap();
                hasTriggered = true;
                Debug.Log("Tuzak Tetiklendi: " + targetTrap.gameObject.name);
            }
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
