using UnityEngine;
using System.Collections;

public class BoxOpenTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] boxSprites;
    [SerializeField] private BoxCollider2D[] boxColliders;
   
    [SerializeField] private SpriteRenderer[] polygonSprites;
    [SerializeField] private PolygonCollider2D[] polygonColliders;

    [SerializeField] private float polygonDelay = 1.5f;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggered) return;

        triggered = true;

        foreach (var sr in boxSprites)
            if (sr) sr.enabled = true;

        foreach (var bc in boxColliders)
            if (bc) bc.enabled = true;

        StartCoroutine(OpenPolygonAfterDelay());
    }

    private IEnumerator OpenPolygonAfterDelay()
    {
        yield return new WaitForSeconds(polygonDelay);

        foreach (var sr in polygonSprites)
            if (sr) sr.enabled = true;

        foreach (var pc in polygonColliders)
            if (pc) pc.enabled = true;
    }
}
