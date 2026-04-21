using UnityEngine;

public class AutoSwingSetup : MonoBehaviour
{
    [SerializeField] private float ropeLength = 3f;
    [SerializeField] private float swingSpeed = 2f;
    [SerializeField] private float maxSwingAngle = 45f;

    private HingeJoint2D hinge;
    private JointMotor2D motor;

    private void Awake()
    {
        // Rigidbody ayarları
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Sabit bir nokta (çivi) oluşturma
        GameObject anchorObj = new GameObject("SwingAnchor_" + name);
        anchorObj.transform.position = transform.position + Vector3.up * ropeLength;
        Rigidbody2D anchorRb = anchorObj.AddComponent<Rigidbody2D>();
        anchorRb.bodyType = RigidbodyType2D.Static;

        // HingeJoint ayarları
        hinge = GetComponent<HingeJoint2D>();
        if (hinge == null) hinge = gameObject.AddComponent<HingeJoint2D>();
        
        hinge.connectedBody = anchorRb;
        hinge.autoConfigureConnectedAnchor = false;
        hinge.anchor = Vector2.zero;
        hinge.connectedAnchor = Vector2.zero;

        // Sallanma motoru ayarları
        hinge.useMotor = true;
        motor = hinge.motor;
    }

    private void Update()
    {
        float currentAngle = transform.localRotation.eulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360;

        if (currentAngle > maxSwingAngle)
        {
            motor.motorSpeed = -swingSpeed * 50; 
        }
        else if (currentAngle < -maxSwingAngle)
        {
            motor.motorSpeed = swingSpeed * 50;
        }

        hinge.motor = motor;
    }
}
