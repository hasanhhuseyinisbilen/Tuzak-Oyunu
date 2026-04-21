using UnityEngine;

public class Level8Generator : MonoBehaviour
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

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null || startIglooPrefab == null || finishIglooPrefab == null || 
            startPlatformPrefab == null || boxPrefab == null)
        {
            Debug.LogError("DİKKAT: Level8Generator içinde eksik prefab var!");
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
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    GenerateWalls(0, true);

                    float pWHalfWidth = GetHalfWidth(startPlatformPrefab);
                    Instantiate(startPlatformPrefab, new Vector3(pWHalfWidth, topY + platHalfHeight, 0), Quaternion.identity);

                    float boxHalfWidth = GetHalfWidth(boxPrefab);
                    float boxHalfHeight = GetHalfHeight(boxPrefab);
                    float boxX = (pWHalfWidth * 2) + boxHalfWidth;
                    float boxY = platformTopSurfaceY - boxHalfHeight;
                    Instantiate(boxPrefab, new Vector3(boxX, boxY, 0), Quaternion.identity);

                    float iglooHalfHeight = GetHalfHeight(startIglooPrefab);
                    Instantiate(startIglooPrefab, new Vector3(pWHalfWidth, platformTopSurfaceY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                    float playerHalfHeight = GetHalfHeight(playerPrefab);
                    float playerXPos = pWHalfWidth + GetHalfWidth(startIglooPrefab) + GetHalfWidth(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(playerXPos, platformTopSurfaceY + playerHalfHeight, 0), Quaternion.identity);
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
