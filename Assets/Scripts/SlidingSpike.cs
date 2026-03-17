using UnityEngine;

public class SlidingSpike : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float slideSpeed = 25f;
    [SerializeField] private float maxDistance = 20f;
    
    private bool isSliding = false;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (isSliding)
        {
            // Sola doğru hareket
            transform.Translate(Vector2.left * slideSpeed * Time.deltaTime);

            // Maksimum mesafeye ulaştığında durdur (Opsiyonel)
            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            {
                isSliding = false;
            }
        }
    }

    public void TriggerSlide()
    {
        if (!isSliding)
        {
            isSliding = true;
            Debug.Log("Yatay Diken Fırladı: " + gameObject.name);
        }
    }

    public void ResetPosition()
    {
        isSliding = false;
        transform.position = startPos;
    }
}
