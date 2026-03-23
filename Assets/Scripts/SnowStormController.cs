using UnityEngine;

public class SnowStormController : MonoBehaviour
{
    public static SnowStormController Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private ParticleSystem snowyParticles;

    [Header("Wind Settings")]
    [SerializeField] private float windForce = 5f;
    [SerializeField] private bool isStormActive = false;

    private Rigidbody2D playerRb;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (snowyParticles != null) snowyParticles.Stop();
        FindPlayer();
    }

    private void FixedUpdate()
    {
        if (isStormActive)
        {
            if (playerRb == null) FindPlayer();

            if (playerRb != null)
            {
                playerRb.AddForce(Vector2.left * windForce, ForceMode2D.Force);
            }
        }
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerRb = playerObj.GetComponent<Rigidbody2D>();
        }
    }

    public void StartStorm()
    {
        isStormActive = true;
        if (snowyParticles != null) snowyParticles.Play();

    }

    public void StopStorm()
    {
        isStormActive = false;
        if (snowyParticles != null) snowyParticles.Stop();

    }
}
