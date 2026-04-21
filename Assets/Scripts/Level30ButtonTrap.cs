using UnityEngine;

public class Level30ButtonTrap : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float buttonPressOffset = 0.2f;
    [SerializeField] private float targetRiseOffset = 3f;

    private Vector3 buttonTargetPos;
    private Vector3 objectTargetPos;
    private bool isPressed = false;

    private void Start()
    {
        buttonTargetPos = transform.position;
        if (targetObject != null)
        {
            objectTargetPos = targetObject.transform.position;
        }
    }

    private void Update()
    {
        if (isPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, buttonTargetPos, moveSpeed * Time.deltaTime);
            if (targetObject != null)
            {
                targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, objectTargetPos, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ball")) && !isPressed)
        {
            isPressed = true;
            buttonTargetPos = transform.position + new Vector3(0, -buttonPressOffset, 0);
            if (targetObject != null)
            {
                objectTargetPos = targetObject.transform.position + new Vector3(0, targetRiseOffset, 0);
            }
        }
    }
}
