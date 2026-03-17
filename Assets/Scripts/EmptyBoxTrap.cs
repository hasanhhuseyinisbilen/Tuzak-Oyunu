using UnityEngine;

public class EmptyBoxTrap : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D[] allColliders;
    private bool isTriggered = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        allColliders = GetComponents<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            
            foreach (var col in allColliders)
            {
                if (col != null) col.enabled = false;
            }
        }
    }

    public void ResetBox()
    {
        isTriggered = false;
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        
        foreach (var col in allColliders)
        {
            if (col != null) col.enabled = true;
        }
    }
}
