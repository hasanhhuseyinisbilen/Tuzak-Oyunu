using UnityEngine;

public class BoxRemover : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            {
                Destroy(sr);
            }
            if (TryGetComponent<BoxCollider2D>(out BoxCollider2D bc))
            {
                Destroy(bc);
            }
            

        }
    }
}