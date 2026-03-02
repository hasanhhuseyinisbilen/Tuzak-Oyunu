using UnityEngine;

public class DusenDiken : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; 
    }

    public void StartFalling()
    {
        if (isFalling) return;

        isFalling = true;
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;  
        }
    }
}
