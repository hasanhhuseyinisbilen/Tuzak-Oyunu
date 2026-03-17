using UnityEngine;

public class Level4Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 15;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Özel Objeler")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject specialSpikePrefab;
    [SerializeField] private GameObject ground7SpikePrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null || iglooPrefab == null || finishIglooPrefab == null || 
            boxPrefab == null || spikePrefab == null || specialSpikePrefab == null || 
            ground7SpikePrefab == null)
        {
            Debug.LogError("DİKKAT: Level4Generator içinde eksik prefab var!");
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
        GenerateWalls(0, true); // Sol Duvar

        float groundHalfWidth = GetHalfWidth(groundPrefab);
        float groundHalfHeight = GetHalfHeight(groundPrefab);
        float topY = groundHalfHeight;

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            if (i == 0)
            {
                float iglooHalfHeight = GetHalfHeight(iglooPrefab);
                Instantiate(iglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerXPos = xPos + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerXPos, topY + playerHalfHeight, 0), Quaternion.identity);
            }

            if (i == groundCount - 1)
            {
                float iglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.identity);
                GenerateWalls(groundCount * groundHalfWidth * 2, false); // Sağ Duvar
            }

            if (i == 10)
            {
                float spikeHalfHeight = GetHalfHeight(ground7SpikePrefab);
                Instantiate(ground7SpikePrefab, new Vector3(xPos, topY + spikeHalfHeight, 0), Quaternion.identity);
            }

            // Engel Paterni (4. zeminden başlar)
            if (i == 3)
            {
                float currentObstacleX = i * groundHalfWidth * 2;
                
                for (int pattern = 0; pattern < 3; pattern++)
                {
                    // 1 Kutu
                    float bHalfWidth = GetHalfWidth(boxPrefab);
                    float bHalfHeight = GetHalfHeight(boxPrefab);
                    Instantiate(boxPrefab, new Vector3(currentObstacleX + bHalfWidth, topY + bHalfHeight, 0), Quaternion.identity);
                    currentObstacleX += bHalfWidth * 2;

                    // 2 Diken
                    for (int s = 0; s < 2; s++)
                    {
                        GameObject currentSpike = (pattern == 2) ? specialSpikePrefab : spikePrefab;
                        float sHalfWidth = GetHalfWidth(currentSpike);
                        float sHalfHeight = GetHalfHeight(currentSpike);
                        Instantiate(currentSpike, new Vector3(currentObstacleX + sHalfWidth, topY + sHalfHeight, 0), Quaternion.identity);
                        currentObstacleX += sHalfWidth * 2;
                    }
                }

                // Ek Kutu
                float finalBHalfWidth = GetHalfWidth(boxPrefab);
                float finalBHalfHeight = GetHalfHeight(boxPrefab);
                Instantiate(boxPrefab, new Vector3(currentObstacleX + finalBHalfWidth, topY + finalBHalfHeight, 0), Quaternion.identity);
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
