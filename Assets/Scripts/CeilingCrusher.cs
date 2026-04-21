using UnityEngine;

public class CeilingCrusher : MonoBehaviour
{
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (col != null)
            {
                col.isTrigger = false;
            }
        }
    }
}
