using UnityEngine;

public class SawRotation : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Vertical Movement")]
    [SerializeField] private float moveAmplitude = 1.5f;
    [SerializeField] private float moveSpeed = 2f;       

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);


        float yOffset = Mathf.Sin(Time.time * moveSpeed) * moveAmplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }
}
