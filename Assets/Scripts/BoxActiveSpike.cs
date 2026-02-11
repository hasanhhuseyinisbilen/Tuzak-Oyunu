using UnityEngine;
using System.Collections;

public class BoxActivatePolygonSpike : MonoBehaviour
{
    [Header("Aktif Edilecek Polygon Dikenler")]
    [SerializeField] private SpriteRenderer[] spikeSprites;
    [SerializeField] private PolygonCollider2D[] spikeColliders;

    [Header("Gecikme")]
    [SerializeField] private float delay = 0.5f;

    private bool activated;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (activated) return;

        activated = true;
        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);

   
        foreach (var sr in spikeSprites)
            if (sr) sr.enabled = true;

        
        foreach (var pc in spikeColliders)
            if (pc) pc.enabled = true;
    }
}
