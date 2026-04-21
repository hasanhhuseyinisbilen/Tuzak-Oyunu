using UnityEngine;

public class Level6Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 10;

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
    [SerializeField] private GameObject bgPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || 
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

        if (bgPrefab != null)
        {
            float bgW = GetHalfWidth(bgPrefab) * 2;
            float bgH = GetHalfHeight(bgPrefab) * 2;
            float totalLevelWidth = groundCount * (groundHalfWidth * 2);
            
            // Kullanıcı isteği üzerine yatayda sabit 2 adet arka plan
            int xCount = 2;
            // Dikeyde yer altını (-15) ve gökyüzünü (+25) kapsayacak şekilde yakalıyoruz
            int yCount = Mathf.CeilToInt(40f / bgH); 

            for (int k = 0; k < xCount; k++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    // -10f'den başlayarak yatayda diziyoruz
                    float bgX = -10f + (k * bgW) + (bgW / 2f);
                    // -15'ten başlayarak dikeyde diziyoruz
                    float bgY = -15f + (j * bgH) + (bgH / 2f);
                    
                    // Seams (ek yerleri) için her ikinci arka planı ters çeviriyoruz
                    Quaternion rotation = (k % 2 == 1) ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                    
                    GameObject bg = Instantiate(bgPrefab, new Vector3(bgX, bgY, 10f), rotation);
                    bg.name = $"BG_{k}_{j}";
                    bg.transform.parent = this.transform;
                }
            }
        }

        float platformTopSurfaceY = topY + (GetHalfHeight(startPlatformPrefab) * 2);

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    GenerateWalls(0, true);

                    float platHalfWidth = GetHalfWidth(startPlatformPrefab);
                    float platHalfHeight = GetHalfHeight(startPlatformPrefab);
                    Instantiate(startPlatformPrefab, new Vector3(platHalfWidth, topY + platHalfHeight, 0), Quaternion.identity);

                    float iglooHalfHeight = GetHalfHeight(iglooPrefab);
                    Instantiate(iglooPrefab, new Vector3(platHalfWidth, platformTopSurfaceY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                    float playerHalfHeight = GetHalfHeight(playerPrefab);
                    float playerXPos = platHalfWidth + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(playerXPos, platformTopSurfaceY + playerHalfHeight, 0), Quaternion.identity);
                    break;

                case 3:
                    float trapHalfHeight = GetHalfHeight(trapPrefab);
                    float trapY = platformTopSurfaceY + trapHalfHeight + trapYOffset;
                    Instantiate(trapPrefab, new Vector3(xPos, trapY, 0), Quaternion.identity);
                    break;

                case 6:
                    float spikeHalfHeight = GetHalfHeight(ground6SpikePrefab);
                    Instantiate(ground6SpikePrefab, new Vector3(xPos, topY + spikeHalfHeight, 0), Quaternion.identity);
                    break;

                default:
                    if (i == groundCount - 1)
                    {
                        float finishIglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                        Instantiate(finishIglooPrefab, new Vector3(xPos, topY + finishIglooHalfHeight, 0), Quaternion.identity);
                        GenerateWalls(groundCount * groundHalfWidth * 2, false);
                    }
                    break;
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
