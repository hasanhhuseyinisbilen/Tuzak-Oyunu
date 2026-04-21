using UnityEngine;

public class ObjectMoveRight : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool rotate = false;
    [SerializeField] private float rotationSpeed = -360f;

    private bool isActivated = false;

    private void Update()
    {
        if (isActivated)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

            if (rotate)
            {
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void Activate()
    {
        isActivated = true;
        Debug.Log(gameObject.name + " activated!");
    }

    public void Deactivate()
    {
        isActivated = false;
    }
}
