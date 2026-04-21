using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 3f;
    [Tooltip("İşaretlenirse oyun başladığında ilk olarak aşağı inmeye başlar.")]
    public bool startMovingDown = false;

    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        float dirMultiplier = startMovingDown ? -1f : 1f;
        float newY = startY + (dirMultiplier * Mathf.Sin(Time.time * speed) * moveDistance);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
