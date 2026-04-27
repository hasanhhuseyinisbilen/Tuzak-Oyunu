using UnityEngine;

public class SawPingPong : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private bool horizontal = true;
    
    [Header("Rotasyon")]
    [SerializeField] private float rotationSpeed = 360f;

    private bool isActive = false;
    private Vector3 startPos;
    private float timer = 0f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            float pingPong = Mathf.PingPong(timer * speed, moveDistance);
            
            if (horizontal)
            {
                transform.position = startPos + new Vector3(pingPong, 0, 0);
            }
            else
            {
                transform.position = startPos + new Vector3(0, pingPong, 0);
            }

            // Testere dönüş efekti
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }

    public void Activate()
    {
        isActive = true;
        timer = 0f; // Aktivasyon anında süreyi sıfırla
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void ResetPosition()
    {
        isActive = false;
        timer = 0f;
        transform.position = startPos;
    }
}
