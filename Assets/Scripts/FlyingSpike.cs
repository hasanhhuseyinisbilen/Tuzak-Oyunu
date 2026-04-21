using UnityEngine;

public class FlyingSpike : MonoBehaviour
{
    [Header("Uçuş Ayarları")]
    [SerializeField] private float flySpeed = 20f;
    [SerializeField] private float maxDistance = 30f; // Ne kadar yükseğe çıkacağı

    private bool isFlying = false;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public void FlyUp()
    {
        if (isFlying) return;
        isFlying = true;
        Debug.Log(gameObject.name + " uçuşa geçti!");
    }

    private void Update()
    {
        if (isFlying)
        {
            // Yukarı doğru hızla hareket et
            transform.Translate(Vector3.up * flySpeed * Time.deltaTime);

            // Çok uzaklaşırsa durdur veya sil (performans için)
            if (Vector3.Distance(startPos, transform.position) > maxDistance)
            {
                isFlying = false;
           Destroy(gameObject);
            }
        }
    }
}
