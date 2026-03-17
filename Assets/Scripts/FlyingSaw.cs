using UnityEngine;

public class FlyingSaw : MonoBehaviour
{
    [Header("Uçuş Ayarları")]
    [SerializeField] private float flySpeed = 20f;
    [SerializeField] private float maxDistance = 30f; // Ne kadar yükseğe çıkacağı
    [SerializeField] private float rotationSpeed = 300f; // Uçarken dönme hızı

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
    }

    private void Update()
    {
        if (isFlying)
        {
            // Yukarı doğru hızla hareket et
            transform.Translate(Vector3.up * flySpeed * Time.deltaTime, Space.World);
            
            // Havada dönsün
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // Çok uzaklaşırsa sil (performans için)
            if (Vector3.Distance(startPos, transform.position) > maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
