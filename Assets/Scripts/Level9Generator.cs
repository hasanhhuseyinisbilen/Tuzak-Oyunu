using UnityEngine;

public class Level9Generator : MonoBehaviour
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
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject startPlatformPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject spikePrefab;
    
    [Header("Level 9 Özel Ayarlar")]
    [SerializeField] private float boxXOffset = 2f; // Kutunun platformun bitişinden olan uzaklığı
    [SerializeField] private float boxYOffset = 0f; // Kutunun dikey ofseti
    [SerializeField] private float finishIglooXOffset = 0f; // Yeni: Bitiş iglosunun kutu üzerindeki yatay ofseti
    [SerializeField] private GameObject trampolinePrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null || startIglooPrefab == null || finishIglooPrefab == null || 
            startPlatformPrefab == null || boxPrefab == null || spikePrefab == null || 
            trampolinePrefab == null)
        {
            Debug.LogError("DİKKAT: Level9Generator içinde eksik prefab var!");
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

        float platformTopSurfaceY = topY;
        float platformEndX = 0f;

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            if (i == 0)
            {
                GenerateWalls(0, true);

                if (startPlatformPrefab != null)
                {
                    float platHalfWidth = GetHalfWidth(startPlatformPrefab);
                    float platHalfHeight = GetHalfHeight(startPlatformPrefab);
                    
                    float platX = platHalfWidth; 
                    float platY = topY + platHalfHeight;
                    
                    Instantiate(startPlatformPrefab, new Vector3(platX, platY, 0), Quaternion.identity);
                    
                    platformTopSurfaceY = platY + platHalfHeight;
                    platformEndX = platX + platHalfWidth;

                    // Kutunun ve Bitiş İglosunun Yerleşimi
                    float boxHalfWidth = GetHalfWidth(boxPrefab);
                    float boxHalfHeight = GetHalfHeight(boxPrefab);
                    
                    float boxX = platformEndX + boxXOffset + boxHalfWidth;
                    float boxY = (platformTopSurfaceY + boxYOffset) - boxHalfHeight;
                    
                    Instantiate(boxPrefab, new Vector3(boxX, boxY, 0), Quaternion.identity);

                    float iglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                    Instantiate(finishIglooPrefab, new Vector3(boxX + finishIglooXOffset, (platformTopSurfaceY + boxYOffset) + iglooHalfHeight, 0), Quaternion.identity);
                }

                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerX = GetHalfWidth(startPlatformPrefab);
                Instantiate(playerPrefab, new Vector3(playerX, platformTopSurfaceY + playerHalfHeight, 0), Quaternion.identity);

                float startIglooHalfHeight = GetHalfHeight(startIglooPrefab);
                Instantiate(startIglooPrefab, new Vector3(playerX, platformTopSurfaceY + startIglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));
            }

            // Sondan 3. ve 4. zeminlerdeki 3'lü diken paterni
            if (i == groundCount - 3 || i == groundCount - 4)
            {
                float sHalfWidth = GetHalfWidth(spikePrefab);
                float sHalfHeight = GetHalfHeight(spikePrefab);
                
                // 3 Dikeni yan yana diz (Pozisyonu ground merkezine göre ayarlı)
                float startSpikeX = xPos - (sHalfWidth * 2); 
                for (int d = 0; d < 3; d++)
                {
                    Instantiate(spikePrefab, new Vector3(startSpikeX + (d * sHalfWidth * 2), topY + sHalfHeight, 0), Quaternion.identity);
                }
            }

            if (i == groundCount - 1)
            {
                GenerateWalls(groundCount * groundHalfWidth * 2, false);

                // Trambolin
                float trampHalfHeight = GetHalfHeight(trampolinePrefab);
                Instantiate(trampolinePrefab, new Vector3(xPos, topY + trampHalfHeight, 0), Quaternion.identity);
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
