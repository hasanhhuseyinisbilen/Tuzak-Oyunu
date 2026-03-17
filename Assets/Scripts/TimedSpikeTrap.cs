using UnityEngine;
using System.Collections;

public class TimedSpikeTrap : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private bool hasTriggered = false;

    [Header("Zaman Ayarı")]
    [SerializeField] private float activeDuration = 2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Başlangıçta bileşenleri kapat
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (polygonCollider != null) polygonCollider.enabled = false;
    }

    public void ActivateTrap()
    {
        if (hasTriggered) return;
        
        hasTriggered = true;
        StartCoroutine(TrapSequence());
    }

    private IEnumerator TrapSequence()
    {
        // Tuzağı aç
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (polygonCollider != null) polygonCollider.enabled = true;
        
        Debug.Log(gameObject.name + " aktifleşti!");

        // Bekle
        yield return new WaitForSeconds(activeDuration);

        // Tuzağı kapat
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (polygonCollider != null) polygonCollider.enabled = false;
        
        Debug.Log(gameObject.name + " kapandı!");
    }
}
