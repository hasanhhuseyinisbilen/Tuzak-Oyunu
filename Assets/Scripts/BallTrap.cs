using UnityEngine;

public class BallTrap : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool rotateOnStart = false;
    private bool isRotating = false;

    private void Start()
    {
        isRotating = rotateOnStart;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isRotating = true;
        }
    }

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
