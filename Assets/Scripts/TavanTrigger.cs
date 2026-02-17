using UnityEngine;

public class TavanTrigger : MonoBehaviour
{
    [Header("Ceiling Group")]
    [SerializeField] private GameObject ceilingParent;

    private TavanBlogu[] pieces;
    private bool triggered = false;

    private void Start()
    {
        if (ceilingParent != null)
        {
            pieces = ceilingParent.GetComponentsInChildren<TavanBlogu>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (pieces == null) return;

        foreach (var piece in pieces)
        {
            piece.StartFalling();
        }
    }
}
