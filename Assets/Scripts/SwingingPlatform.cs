using UnityEngine;

public class SwingingPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float maxAngle = 45.0f;
    
    private HingeJoint2D hinge;
    private JointMotor2D motor;

    private void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        motor = hinge.motor;
    }

    private void Update()
    {
        if (transform.localRotation.eulerAngles.z > maxAngle && transform.localRotation.eulerAngles.z < 180)
        {
            motor.motorSpeed = -speed;
        }
        else if (transform.localRotation.eulerAngles.z < 360 - maxAngle && transform.localRotation.eulerAngles.z > 180)
        {
            motor.motorSpeed = speed;
        }
        
        hinge.motor = motor;
        hinge.useMotor = true;
    }
}
