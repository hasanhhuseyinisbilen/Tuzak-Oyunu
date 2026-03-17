using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class TimedFallingBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isTriggered = false;

    [Header("Düşüş Ayarları")]
    [SerializeField] private float delayBeforeFall = 1f;
    [SerializeField] private float fallGravityScale = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Başlangıçta havada sabit tutmak için Kinematic yapıyoruz
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        // Sağa sola kaymasın ve dönmesin
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered) return;

        if (collision.collider.CompareTag("Player"))
        {
            // Temasın üstten olup olmadığını kontrol etmek iyi olabilir ama basitlik adına direkt tetikliyoruz
            isTriggered = true;
            StartCoroutine(FallAfterDelay());
        }
    }

    private IEnumerator FallAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFall);

        // Fiziği aç
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravityScale;
        rb.WakeUp();

        Debug.Log(gameObject.name + " süresi doldu ve düşüyor!");
    }
}
