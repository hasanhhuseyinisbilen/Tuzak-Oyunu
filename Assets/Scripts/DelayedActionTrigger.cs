using UnityEngine;

public class DelayedActionTrigger : MonoBehaviour
{
    public GameObject targetObject;
    private int entryCount = 0;

    private void Start()
    {
        if (targetObject != null)
        {
            if (targetObject.GetComponent<SpriteRenderer>() != null)
                targetObject.GetComponent<SpriteRenderer>().enabled = false;
            
            if (targetObject.GetComponent<PolygonCollider2D>() != null)
                targetObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            entryCount++;
            if (entryCount == 2 && targetObject != null)
            {
                if (targetObject.GetComponent<SpriteRenderer>() != null)
                    targetObject.GetComponent<SpriteRenderer>().enabled = true;
                
                if (targetObject.GetComponent<PolygonCollider2D>() != null)
                    targetObject.GetComponent<PolygonCollider2D>().enabled = true;
            }
        }
    }
}
