using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerDead : MonoBehaviour
{
    [Header("DeadZone Ayarları")]
    [SerializeField] private float deathY = -10f;

    public bool isDead { get; private set; } = false;
    private Rigidbody2D rb;

    private const string TAG_TRAP = "Trap";
    private const string TAG_ICICLE = "Icicle";
    private const string TAG_FLYBOX = "FlyBox";
    private const string TAG_DIKEN = "Diken";
    private const string TAG_GAS = "Gas";

    [Header("Buz Efekti Ayarları")]
    [SerializeField] private GameObject iceEffectPrefab;
    [SerializeField] private AudioClip freezeSound;
    
    [Header("Jungle Efekti Ayarları")]
    [SerializeField] private GameObject vineEffectPrefab;
    [SerializeField] private AudioClip vineSound;

    [SerializeField] private float restartDelay = 0.4f;

    private AudioSource deathAudioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        AudioSource[] allSources = GetComponents<AudioSource>();
        foreach (AudioSource source in allSources)
        {
            source.playOnAwake = false;
            source.Stop();
        }

        deathAudioSource = gameObject.AddComponent<AudioSource>();
        deathAudioSource.playOnAwake = false;
        deathAudioSource.spatialBlend = 0f;
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
            collision.gameObject.CompareTag(TAG_DIKEN) ||
            collision.gameObject.CompareTag(TAG_GAS))
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
            other.CompareTag(TAG_DIKEN) ||
            other.CompareTag(TAG_GAS))
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        if (isDead || Time.timeSinceLevelLoad < 0.1f) return;

        if (LivesManager.Instance != null)
        {
            LivesManager.Instance.UseLife();
        }

        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine()
    {
        isDead = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        bool isJungleLevel = false;

        if (LevelManager.Instance != null && LevelManager.Instance.currentLevelIndex >= 20)
        {
            isJungleLevel = true;
        }
        
        if (!isJungleLevel)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                string name = obj.name.ToLower();
                if (name.Contains("level2") || name.Contains("level3") || name.Contains("jungle"))
                {
                    isJungleLevel = true;
                    break;
                }
            }
        }

        if (isJungleLevel)
        {
            if (vineSound != null && deathAudioSource != null)
                deathAudioSource.PlayOneShot(vineSound);

            if (vineEffectPrefab != null)
            {
                GameObject vine = Instantiate(vineEffectPrefab, transform.position, transform.rotation);
                ApplyEffectScale(vine);
            }
        }
        else
        {
            if (freezeSound != null && deathAudioSource != null)
                deathAudioSource.PlayOneShot(freezeSound);

            if (iceEffectPrefab != null)
            {
                GameObject ice = Instantiate(iceEffectPrefab, transform.position, transform.rotation);
                ApplyEffectScale(ice);
            }
        }

        yield return new WaitForSeconds(restartDelay);

        if (LivesManager.Instance != null && LivesManager.Instance.currentLives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    private void ApplyEffectScale(GameObject effect)
    {
        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 localScale = effect.transform.localScale;
        effect.transform.localScale = new Vector3(Mathf.Abs(localScale.x) * direction, localScale.y, localScale.z);
    }
}
