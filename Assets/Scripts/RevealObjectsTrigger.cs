using UnityEngine;

public class RevealObjectsTrigger : MonoBehaviour
{
    [Header("Açılacak Objeler (Dizi)")]
    [Tooltip("İçine attığın objelerin gizli olan SpriteRenderer ve Collider2D bileşenlerini açar.")]
    [SerializeField] private GameObject[] targetObjects;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

   
        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;
            
  
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.enabled = true;
                    }

                    Collider2D col = obj.GetComponent<Collider2D>();
                    if (col != null)
                    {
                        col.enabled = true;
                    }
                }
            }
        }
    }
}
