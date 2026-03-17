using UnityEngine;

public class Level14Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 10;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject ceilingSpikePrefab; 
    [SerializeField] private float ceilingYOffset = 5f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject startPlatformPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject spikyBoxPrefab; 
    [SerializeField] private float boxDistance = 0f;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || ceilingSpikePrefab == null || 
            wallPrefab == null || playerPrefab == null || startIglooPrefab == null || 
            finishIglooPrefab == null || startPlatformPrefab == null || boxPrefab == null || 
            spikyBoxPrefab == null)
        {
            Debug.LogError("DİKKAT: Level14Generator içinde eksik prefab var!");
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

        float platHalfHeight = GetHalfHeight(startPlatformPrefab);
        float platformTopSurfaceY = topY + (platHalfHeight * 2);

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            // Tavan ve Tavan Dikenleri
            if (ceilingPrefab != null)
            {
                Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

                if (i >= 6 && i < 11)
                {
                    float spikeHalfW = GetHalfWidth(ceilingSpikePrefab);
                    float spikeHalfH = GetHalfHeight(ceilingSpikePrefab);
                    float ceilingHalfH = GetHalfHeight(ceilingPrefab);
                    
                    float blockStartX = i * groundHalfWidth * 2;
                    int spikesInBlock = Mathf.FloorToInt((groundHalfWidth * 2) / (spikeHalfW * 2));

                    for (int s = 0; s < spikesInBlock; s++)
                    {
                        float spikeX = blockStartX + (s * spikeHalfW * 2) + spikeHalfW;
                        float ceilingBottomY = ceilingYOffset - ceilingHalfH;
                        float finalSpikeY = ceilingBottomY - spikeHalfH;

                        Instantiate(ceilingSpikePrefab, new Vector3(spikeX, finalSpikeY, 0), Quaternion.identity);
                    }
                }
            }

            if (i == 0)
            {
                GenerateWalls(0, true);

                // Platform
                float platHalfWidth = GetHalfWidth(startPlatformPrefab);
                Instantiate(startPlatformPrefab, new Vector3(platHalfWidth, topY + platHalfHeight, 0), Quaternion.identity);

                // Platform yanındaki Kutu
                float boxHalfWidth = GetHalfWidth(boxPrefab);
                float boxHalfHeight = GetHalfHeight(boxPrefab);
                float boxX = (platHalfWidth * 2) + boxHalfWidth + boxDistance;
                float boxY = platformTopSurfaceY - boxHalfHeight;
                Instantiate(boxPrefab, new Vector3(boxX, boxY, 0), Quaternion.identity);

                // Player
                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerX = platHalfWidth + GetHalfWidth(startIglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerX, platformTopSurfaceY + playerHalfHeight, 0), Quaternion.identity);

                // Igloo
                float iglooHalfHeight = GetHalfHeight(startIglooPrefab);
                Instantiate(startIglooPrefab, new Vector3(platHalfWidth, platformTopSurfaceY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));
            }

            if (i == groundCount - 1)
            {
                float rightWallX = groundCount * groundHalfWidth * 2;
                GenerateWalls(rightWallX, false);

                // Bitiş İglosu
                float finishHalfHeight = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, topY + finishHalfHeight, 0), Quaternion.identity);

                // Dikenli Kutu (Duvarın içinde)
                float boxHalfWidth = GetHalfWidth(spikyBoxPrefab);
                float boxHalfHeight = GetHalfHeight(spikyBoxPrefab);
                Instantiate(spikyBoxPrefab, new Vector3(rightWallX + boxHalfWidth, topY + boxHalfHeight, 0), Quaternion.identity);
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
