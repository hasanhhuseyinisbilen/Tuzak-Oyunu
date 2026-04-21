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
            transform.Translate(Vector2.left * slideSpeed * Time.deltaTime);

            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TriggerSlide()
    {
        if (!isSliding)
        {
            isSliding = true;
        }
    }

    public void ResetPosition()
    {
        isSliding = false;
        transform.position = startPos;
    }
}
