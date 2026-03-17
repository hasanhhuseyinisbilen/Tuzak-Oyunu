using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Zıplama Ayarları")]
    [SerializeField] private float jumpForce = 15f;
    
    [Header("Görsel Efekt (Opsiyonel)")]
    [SerializeField] private float scaleAmount = 0.8f;
    [SerializeField] private float scaleDuration = 0.1f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Oyuncu üstten mi çarptı kontrolü yapabiliriz ama genelde temas yeterlidir
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Mevcut dikey hızı sıfırla ve fırlat (Böylece her zaman aynı yüksekliğe zıplar)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                
                Debug.Log("Trambolin Tetiklendi! Zıplama Gücü: " + jumpForce);
                
                // Küçük bir ezilme efekti
                StopAllCoroutines();
                StartCoroutine(BounceEffect());
            }
        }
    }

    private System.Collections.IEnumerator BounceEffect()
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleAmount, originalScale.z);
        yield return new WaitForSeconds(scaleDuration);
        transform.localScale = originalScale;
    }
}
