using UnityEngine;

public class SnowBallTrigger : MonoBehaviour
{
    [Header("Target Object")]
    [SerializeField] private Transform snowBall;  

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = -360f;

    private bool move = false;

    private void Update()
    {
        if (!move || snowBall == null) return;

   
        snowBall.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);

       
        snowBall.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        move = true;
    }
}
