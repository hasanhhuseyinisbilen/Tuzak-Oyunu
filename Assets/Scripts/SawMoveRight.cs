using UnityEngine;

public class SawMoveRight : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = -360f; // Sağa giderken genelde ters dönmesi estetik durur
    private bool isActivated = false;

    void Update()
    {
        if (isActivated)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public void Activate()
    {
        isActivated = true;
    }

    public void Deactivate()
    {
        isActivated = false;
    }

    public void SetSpeed(float newSpeed, float newRotation)
    {
        speed = newSpeed;
        rotationSpeed = newRotation;
    }
}
