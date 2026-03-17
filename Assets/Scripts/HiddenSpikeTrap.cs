using UnityEngine;

public class HiddenSpikeTrap : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D hazardCollider;
    private bool hasActivated = false;

    [Header("Bileşenler (Opsiyonel - Otomatik Bulunur)")]
    [SerializeField] private SpriteRenderer targetSprite;
    [SerializeField] private PolygonCollider2D targetCollider;

    private void Awake()
    {
        // Bileşenleri al
        spriteRenderer = targetSprite != null ? targetSprite : GetComponent<SpriteRenderer>();
        hazardCollider = targetCollider != null ? targetCollider : GetComponent<PolygonCollider2D>();

        // Başlangıçta gizle ve çarpışmayı kapat
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (hazardCollider != null) hazardCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasActivated) return;

        if (other.CompareTag("Player"))
        {
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        hasActivated = true;

        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (hazardCollider != null) hazardCollider.enabled = true;

        Debug.Log("Gizli Diken Aktifleşti: " + gameObject.name);
    }

    // Tuzağı sıfırlamak gerekirse
    public void ResetTrap()
    {
        hasActivated = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (hazardCollider != null) hazardCollider.enabled = false;
    }
}
