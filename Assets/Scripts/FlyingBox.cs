using UnityEngine;

public class FlyingBox : MonoBehaviour
{
    [Header("Uçuş Ayarları")]
    [SerializeField] private float flySpeed = 15f;
    [SerializeField] private float maxDistance = 25f;

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
        Debug.Log("Kutu uçuşa geçti: " + gameObject.name);
    }

    private void Update()
    {
        if (isFlying)
        {
            transform.Translate(Vector3.up * flySpeed * Time.deltaTime);

            if (Vector3.Distance(startPos, transform.position) > maxDistance)
            {
                isFlying = false;
                // Destroy(gameObject); // Opsiyonel
            }
        }
    }
}
