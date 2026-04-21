using UnityEngine;

public class RisingBoxElevator : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 targetPosition;
    
    private bool isActivated = false;
    private Transform playerTransform;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    private void Update()
    {
        if (isActivated)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
      
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isActivated = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isActivated = true;
            Debug.Log("Elevator Box aktive edildi!");
        }

        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isActivated = false;
            Debug.Log("Elevator Box zemine ulasti ve durdu.");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }
}
