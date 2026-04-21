using UnityEngine;

public class SwingingChain : MonoBehaviour
{
    [SerializeField] private float swingSpeed = 2f;
    [SerializeField] private float maxAngle = 45f;
    [SerializeField] private float timeOffset = 0f;

    void Update()
    {
        float angle = Mathf.Sin((Time.time + timeOffset) * swingSpeed) * maxAngle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
