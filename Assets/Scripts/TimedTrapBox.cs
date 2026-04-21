using UnityEngine;

public class TimedTrapBox : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject hiddenIcicle;
    [SerializeField] private float activationDelay = 3f;

    private float timer = 0f;
    private bool isTriggered = false;
    private bool activated = false;

    private SpriteRenderer icicleSprite;
    private PolygonCollider2D icicleCollider;

    void Start()
    {
        if (hiddenIcicle != null)
        {
            icicleSprite = hiddenIcicle.GetComponent<SpriteRenderer>();
            icicleCollider = hiddenIcicle.GetComponent<PolygonCollider2D>();

            if (icicleSprite != null) icicleSprite.enabled = false;
            if (icicleCollider != null) icicleCollider.enabled = false;
        }
    }

    void Update()
    {
        if (activated || !isTriggered || hiddenIcicle == null) return;

        timer += Time.deltaTime;

        if (timer >= activationDelay)
        {
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        activated = true;
        if (icicleSprite != null) icicleSprite.enabled = true;
        if (icicleCollider != null) icicleCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
        }
    }
}

