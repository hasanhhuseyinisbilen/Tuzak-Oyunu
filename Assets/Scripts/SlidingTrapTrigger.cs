using UnityEngine;

public class SlidingTrapTrigger : MonoBehaviour
{
    [SerializeField] private TargetedSlidingTrap trapToTrigger;
    private bool triggered = false;

    public void Setup(TargetedSlidingTrap trap)
    {
        trapToTrigger = trap;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player") && trapToTrigger != null)
        {
            triggered = true;
            trapToTrigger.Trigger();
        }
    }
}
