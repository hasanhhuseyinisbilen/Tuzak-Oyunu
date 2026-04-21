using UnityEngine;

public class Level32ButtonTrap : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private GameObject[] horizontalTargets;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float buttonPressOffset = 0.2f;
    [SerializeField] private float targetRiseOffset = 3f;
    [SerializeField] private float targetRightOffset = 5f;

    private Vector3 buttonTargetPos;
    private Vector3 verticalTargetPos;
    private Vector3[] horizontalTargetPositions;
    private bool isPressed = false;

    private void Start()
    {
        buttonTargetPos = transform.position;
        
        if (targetObject != null)
        {
            verticalTargetPos = targetObject.transform.position;
        }

        if (horizontalTargets != null && horizontalTargets.Length > 0)
        {
            horizontalTargetPositions = new Vector3[horizontalTargets.Length];
            for (int i = 0; i < horizontalTargets.Length; i++)
            {
                if (horizontalTargets[i] != null)
                {
                    horizontalTargetPositions[i] = horizontalTargets[i].transform.position;
                }
            }
        }
    }

    private void Update()
    {
        if (isPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, buttonTargetPos, moveSpeed * Time.deltaTime);
            
            if (targetObject != null)
            {
                targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, verticalTargetPos, moveSpeed * Time.deltaTime);
            }

            if (horizontalTargets != null)
            {
                for (int i = 0; i < horizontalTargets.Length; i++)
                {
                    if (horizontalTargets[i] != null)
                    {
                        horizontalTargets[i].transform.position = Vector3.MoveTowards(horizontalTargets[i].transform.position, horizontalTargetPositions[i], moveSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            buttonTargetPos = transform.position + new Vector3(0, -buttonPressOffset, 0);
            
            if (targetObject != null)
            {
                verticalTargetPos = targetObject.transform.position + new Vector3(0, targetRiseOffset, 0);
            }

            if (horizontalTargets != null)
            {
                for (int i = 0; i < horizontalTargets.Length; i++)
                {
                    if (horizontalTargets[i] != null)
                    {
                        horizontalTargetPositions[i] = horizontalTargets[i].transform.position + new Vector3(targetRightOffset, 0, 0);
                    }
                }
            }
        }
    }
}
