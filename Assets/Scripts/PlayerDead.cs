using UnityEngine;
using System.Collections;

public class PlayerDead : MonoBehaviour
{
    [Header("DeadZone Ayarları")]
    [SerializeField] private float deathY = -10f;

    private bool isDead = false;

    private Rigidbody2D rb;
    private SnowyLevelManager levelManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = FindObjectOfType<SnowyLevelManager>();
    }

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

        if (collision.gameObject.CompareTag("Trap") ||
            collision.gameObject.CompareTag("Icicle") ||
            collision.gameObject.CompareTag("FlyBox") ||
            collision.gameObject.CompareTag("Diken"))
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Trap") ||
            other.CompareTag("Icicle") ||
            other.CompareTag("FlyBox") ||
            other.CompareTag("Diken"))
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        if (isDead) return;

        isDead = true;

        if (LivesManager.Instance != null)
            LivesManager.Instance.LoseLife();

        if (levelManager != null)
            levelManager.RestartLevel();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        StartCoroutine(ResetDeathFlagRoutine());
    }

    IEnumerator ResetDeathFlagRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        isDead = false;
    }
}
