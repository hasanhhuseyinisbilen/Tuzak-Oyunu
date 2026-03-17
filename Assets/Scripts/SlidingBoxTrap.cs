using UnityEngine;
using System.Collections;

public class SlidingBoxTrap : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float slideDistance = 3f;
    [SerializeField] private float slideSpeed = 10f;
    [SerializeField] private float returnSpeed = 3f;
    [SerializeField] private float waitTime = 0.5f;

    private Vector3 originalPosition;
    private bool isMoving = false;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void TriggerTrap()
    {
        if (isMoving) return;
        StartCoroutine(SlideRoutine());
    }

    private IEnumerator SlideRoutine()
    {
        isMoving = true;

        // Sağa Kayış
        Vector3 targetPos = originalPosition + Vector3.right * slideDistance;
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, slideSpeed * Time.deltaTime);
            yield return null;
        }

        // Bekleme
        yield return new WaitForSeconds(waitTime);

        // Geri Dönüş
        while (Vector3.Distance(transform.position, originalPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = originalPosition;
        isMoving = false;
    }
}
