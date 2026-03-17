using UnityEngine;

public class Level6Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 10;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject startPlatformPrefab;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float trapYOffset = 0f;
    [SerializeField] private GameObject ground6SpikePrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null || iglooPrefab == null || finishIglooPrefab == null || 
            startPlatformPrefab == null || trapPrefab == null || ground6SpikePrefab == null)
        {
            Debug.LogError("DİKKAT: Level6Generator içinde eksik prefab var!");
            canGenerate = false;
            enabled = false;
        }
    }

    void Start()
    {
        if (canGenerate) GenerateLevel();
    }

    private void GenerateLevel()
    {
        float groundHalfWidth = GetHalfWidth(groundPrefab);
        float groundHalfHeight = GetHalfHeight(groundPrefab);
        float topY = groundHalfHeight;

        // Platform üst yüzey seviyesi
        float platformTopSurfaceY = topY + (GetHalfHeight(startPlatformPrefab) * 2);

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            if (i == 0)
            {
                GenerateWalls(0, true);

                // Platform
                float platHalfWidth = GetHalfWidth(startPlatformPrefab);
                float platHalfHeight = GetHalfHeight(startPlatformPrefab);
                Instantiate(startPlatformPrefab, new Vector3(platHalfWidth, topY + platHalfHeight, 0), Quaternion.identity);

                // Igloo (Platform üstünde)
                float iglooHalfHeight = GetHalfHeight(iglooPrefab);
                Instantiate(iglooPrefab, new Vector3(platHalfWidth, platformTopSurfaceY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                // Player (Igloo önünde)
                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerXPos = platHalfWidth + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerXPos, platformTopSurfaceY + playerHalfHeight, 0), Quaternion.identity);
            }

            if (i == groundCount - 1)
            {
                float iglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.identity);
                GenerateWalls(groundCount * groundHalfWidth * 2, false);
            }

            // Kapan Tuzağı
            if (i == 3)
            {
                float trapHalfHeight = GetHalfHeight(trapPrefab);
                float trapY = platformTopSurfaceY + trapHalfHeight + trapYOffset;
                Instantiate(trapPrefab, new Vector3(xPos, trapY, 0), Quaternion.identity);
            }

            // Özel Diken
            if (i == 6)
            {
                float spikeHalfHeight = GetHalfHeight(ground6SpikePrefab);
                Instantiate(ground6SpikePrefab, new Vector3(xPos, topY + spikeHalfHeight, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateWalls(float xOrigin, bool isLeft)
    {
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xPos = isLeft ? 
                    xOrigin - (col * wallHalfWidth * 2) - wallHalfWidth : 
                    xOrigin + (col * wallHalfWidth * 2) + wallHalfWidth;

                float yPos = wallYOffset + (row * wallHalfHeight * 2) + wallHalfHeight;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null) ? r.bounds.size.y / 2f : 0.5f;
    }

    private float GetHalfWidth(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null) ? r.bounds.size.x / 2f : 0.5f;
    }
}
