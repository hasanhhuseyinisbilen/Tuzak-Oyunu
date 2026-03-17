using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerDead : MonoBehaviour
{
    [Header("DeadZone Ayarları")]
    [SerializeField] private float deathY = -10f;

    private bool isDead = false;

    private Rigidbody2D rb;
    // private SnowyLevelManager levelManager;

    // Tag Constants
    private const string TAG_TRAP = "Trap";
    private const string TAG_ICICLE = "Icicle";
    private const string TAG_FLYBOX = "FlyBox";
    private const string TAG_DIKEN = "Diken";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // levelManager = FindObjectOfType<SnowyLevelManager>();
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

        if (collision.gameObject.CompareTag(TAG_TRAP) ||
            collision.gameObject.CompareTag(TAG_ICICLE) ||
            collision.gameObject.CompareTag(TAG_FLYBOX) ||
            collision.gameObject.CompareTag(TAG_DIKEN))
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag(TAG_TRAP) ||
            other.CompareTag(TAG_ICICLE) ||
            other.CompareTag(TAG_FLYBOX) ||
            other.CompareTag(TAG_DIKEN))
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        if (isDead) return;

        isDead = true;

        /* if (LivesManager.Instance != null)
            LivesManager.Instance.LoseLife(); */

        /* if (levelManager != null)
            levelManager.RestartLevel(); */

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Mevcut bölümü baştan yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
