using UnityEngine;

public class TavanTetikleyici : MonoBehaviour
{
    [Header("Ceiling Group")]
    public GameObject ceilingParent;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            
            if (ceilingParent != null)
            {

                TavanBlogu[] pieces = ceilingParent.GetComponentsInChildren<TavanBlogu>();
                foreach (var piece in pieces)
                {
                    piece.StartFalling();
                }
            }
        }
    }
}
