using UnityEngine;

public class Level3Generator : MonoBehaviour
{
    [Header("Zemin ve Kutu Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject specialBoxPrefab;
    [SerializeField] private GameObject lastBoxPrefab;
    [SerializeField] private GameObject iglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private int startGroundCount = 2;
    [SerializeField] private int middleBoxCount = 7;
    [SerializeField] private int endGroundCount = 2;
    [SerializeField] private float boxGap = 2f;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Oyuncu Ayarları")]
    [SerializeField] private GameObject playerPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || groundSpikePrefab == null || boxPrefab == null || 
            specialBoxPrefab == null || lastBoxPrefab == null || iglooPrefab == null || 
            finishIglooPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null)
        {
            Debug.LogError("DİKKAT: Level3Generator içinde eksik prefab var!");
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
        float currentX = 0;
        float groundHalfWidth = GetHalfWidth(groundSpikePrefab);
        float groundHalfHeight = GetHalfHeight(groundSpikePrefab);
        float boxHalfWidth = GetHalfWidth(boxPrefab);
        float topY = groundHalfHeight;

        // 1. Başlangıç Zeminleri
        for (int i = 0; i < startGroundCount; i++)
        {
            float xPos = currentX + groundHalfWidth;
            Instantiate(groundSpikePrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            if (i == 0)
            {
                // Duvarlar
                GenerateWalls(currentX, true);

                // Igloo
                float iglooHalfHeight = GetHalfHeight(iglooPrefab);
                Instantiate(iglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                // Player
                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerXPos = xPos + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerXPos, topY + playerHalfHeight, 0), Quaternion.identity);
            }

            currentX += groundHalfWidth * 2;
        }

        // 2. Orta Kutular (Aralıklı)
        for (int i = 0; i < middleBoxCount; i++)
        {
            currentX += boxGap;
            float xPos = currentX + boxHalfWidth;
            
            if (i == middleBoxCount - 2)
            {
                Instantiate(specialBoxPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            }
            else if (i == middleBoxCount - 1)
            {
                Instantiate(lastBoxPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(boxPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            }

            currentX += boxHalfWidth * 2;
        }

        // 3. Bitiş Zeminleri
        currentX += boxGap;
        for (int i = 0; i < endGroundCount; i++)
        {
            float xPos = currentX + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            if (i == endGroundCount - 1)
            {
                // Bitiş İglosu
                float iglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.identity);

                // Sağ Duvarlar
                GenerateWalls(currentX + (groundHalfWidth * 2), false);
            }

            currentX += groundHalfWidth * 2;
        }

        // 4. KESİNTİSİZ TAVAN
        float ceilingHalfWidth = GetHalfWidth(ceilingPrefab);
        int ceilingCount = Mathf.CeilToInt(currentX / (ceilingHalfWidth * 2));

        for (int i = 0; i < ceilingCount; i++)
        {
            float xPos = (i * ceilingHalfWidth * 2) + ceilingHalfWidth;
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);
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
