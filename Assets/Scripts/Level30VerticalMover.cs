using UnityEngine;

public class Level30VerticalMover : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 2f;
    public bool canMove = true;
    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void OnEnable()
    {
        Level30StopButton.OnStopSaws += StopMovement;
    }

    private void OnDisable()
    {
        Level30StopButton.OnStopSaws -= StopMovement;
    }

    private void Update()
    {
        if (canMove)
        {
            transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time * speed) * distance, transform.position.z);
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }
}
