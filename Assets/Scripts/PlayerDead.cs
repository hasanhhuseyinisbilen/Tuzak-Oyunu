using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDead : MonoBehaviour
{
    [Header("DeadZone Ayarları")]
    [SerializeField] private float deathY = -10f; 
    private bool isDead = false; 

    void Update()
    {
      
        if (!isDead && transform.position.y < deathY)
        {
            RestartLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; 

        if (
            collision.gameObject.CompareTag("Trap") ||
            collision.gameObject.CompareTag("Icicle") ||
            collision.gameObject.CompareTag("FlyBox") ||
            collision.gameObject.CompareTag("Diken")
           )
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return; 

        if (
            other.CompareTag("Trap") ||
            other.CompareTag("Icicle") ||
            other.CompareTag("FlyBox")||
            other.CompareTag("Diken")
           )
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        if (isDead) return; 

        isDead = true; 

  
        if (LivesManager.Instance != null)
        {
            LivesManager.Instance.LoseLife();
        }

        SnowyLevelManager manager = FindObjectOfType<SnowyLevelManager>();
        if (manager != null)
        {
            manager.RestartLevel();
        }

        transform.position = Vector3.zero;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Invoke("ResetDeathFlag", 0.5f);
    }

    void ResetDeathFlag()
    {
        isDead = false;
    }
}
