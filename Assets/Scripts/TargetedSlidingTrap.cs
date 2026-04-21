using UnityEngine;

public class TargetedSlidingTrap : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private bool rotateOnlyAfterArriving = true;
    
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isTriggered = false;
    private bool positionReached = false;

    public void Setup(Vector3 targetPos, Quaternion targetRot, bool rotateAfter = true)
    {
        targetPosition = targetPos;
        targetRotation = targetRot;
        rotateOnlyAfterArriving = rotateAfter;
    }

    public void Trigger()
    {
        isTriggered = true;
    }

    void Update()
    {
        if (isTriggered)
        {
            // Pozisyona yumuşak geçiş
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                positionReached = true;
            }

            // Rotasyona geçiş (Eğer varıldıysa veya anlık dönme isteniyorsa)
            if (!rotateOnlyAfterArriving || positionReached)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    transform.rotation = targetRotation;
                    if (positionReached) isTriggered = false; // Her iki hedef de tamamlandı
                }
            }
        }
    }
}
