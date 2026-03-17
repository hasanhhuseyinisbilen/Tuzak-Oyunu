using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingGround : MonoBehaviour
{
    [Header("Düşüş Ayarları")]
    [SerializeField] private float fallDelay = 0.5f;   // Kendi düşme gecikmesi
    [SerializeField] private float destroyDelay = 3f;  // Yok olma süresi
    [SerializeField] private float sequenceDelay = 1.0f; // Bir sonraki bloğa geçiş süresi

    private FallingGround nextBlock; // Kodla atanacak
    private Rigidbody2D rb;
    private bool isTriggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetNextBlock(FallingGround next)
    {
        nextBlock = next;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            StartFalling();
        }
    }

    public void StartFalling()
    {
        if (isTriggered) return;
        isTriggered = true;
        StartCoroutine(FallCoroutine());
    }

    private IEnumerator FallCoroutine()
    {
        // 1. Bekle ve düş
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);

        // 2. Eğer sonraki blok varsa onu tetikle
        if (nextBlock != null)
        {
            yield return new WaitForSeconds(sequenceDelay);
            nextBlock.StartFalling();
        }
    }
}
