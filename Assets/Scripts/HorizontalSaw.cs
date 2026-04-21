using UnityEngine;

public class HorizontalSaw : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float distance = 4f;
    [SerializeField] private float rotationSpeed = 200f;

    private Vector3 startPos;
    private int direction = 1;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) >= distance)
        {
            direction *= -1;
            transform.position = startPos + Vector3.right * (direction < 0 ? distance : -distance);
        }
    }
}
