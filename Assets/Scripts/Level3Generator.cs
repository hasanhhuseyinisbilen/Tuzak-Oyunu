using UnityEngine;

public class Level3Generator : MonoBehaviour
{
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
    [SerializeField] private float boxGap = 4f;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private float waterYOffset = -2f;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || groundSpikePrefab == null || boxPrefab == null || 
            specialBoxPrefab == null || lastBoxPrefab == null || iglooPrefab == null || 
            finishIglooPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null || waterPrefab == null)
        {
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

        int totalItems = startGroundCount + middleBoxCount + endGroundCount;

        if (waterPrefab != null)
        {
            float waterStartX = startGroundCount * (groundHalfWidth * 2f);
            float boxSectionWidth = middleBoxCount * (boxGap + (boxHalfWidth * 2f));
            float waterHalfW = GetHalfWidth(waterPrefab);
            float waterFullW = waterHalfW * 2f;
            float waterHalfH = GetHalfHeight(waterPrefab);
            float waterFullH = waterHalfH * 2f;
            
            int waterTilesCount = Mathf.CeilToInt(boxSectionWidth / waterFullW) + 1; 

            for (int k = 0; k < waterTilesCount; k++)
            {
                float wx = waterStartX + (k * waterFullW) + waterHalfW;
                
                // Ust sira (Animasyonlu)
                GameObject waterTop = Instantiate(waterPrefab, new Vector3(wx, waterYOffset, 0), Quaternion.identity);
                waterTop.AddComponent<WaterAnimation>();

                // Alt sira
                Instantiate(waterPrefab, new Vector3(wx, waterYOffset - waterFullH, 0), Quaternion.identity);
            }
        }

        for (int i = 0; i < totalItems; i++)
        {
            float xPos = 0;

            switch (i)
            {
                case int n when (n < startGroundCount):
                    xPos = currentX + groundHalfWidth;
                    Instantiate(groundSpikePrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (n == 0)
                    {
                        GenerateWalls(currentX, true);
                        float igHalf = GetHalfHeight(iglooPrefab);
                        Instantiate(iglooPrefab, new Vector3(xPos, topY + igHalf, 0), Quaternion.Euler(0, 180, 0));
                        float pHalf = GetHalfHeight(playerPrefab);
                        float pX = xPos + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                        Instantiate(playerPrefab, new Vector3(pX, topY + pHalf, 0), Quaternion.identity);
                    }
                    currentX += groundHalfWidth * 2;
                    break;

                case int n when (n >= startGroundCount && n < startGroundCount + middleBoxCount):
                    currentX += boxGap;
                    
                    xPos = currentX + boxHalfWidth;
                    int boxIdx = n - startGroundCount;
                    
                    if (boxIdx == middleBoxCount - 2)
                        Instantiate(specialBoxPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    else if (boxIdx == middleBoxCount - 1)
                        Instantiate(lastBoxPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    else
                        Instantiate(boxPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    
                    currentX += boxHalfWidth * 2;
                    break;

                case int n when (n >= startGroundCount + middleBoxCount):
                    if (n == startGroundCount + middleBoxCount) currentX += boxGap;
                    
                    xPos = currentX + groundHalfWidth;
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    
                    if (n == totalItems - 1)
                    {
                        float fHalf = GetHalfHeight(finishIglooPrefab);
                        Instantiate(finishIglooPrefab, new Vector3(xPos, topY + fHalf, 0), Quaternion.identity);
                        GenerateWalls(currentX + (groundHalfWidth * 2), false);
                    }
                    currentX += groundHalfWidth * 2;
                    break;
            }
        }

        float cHalf = GetHalfWidth(ceilingPrefab);
        int cCount = Mathf.CeilToInt(currentX / (cHalf * 2));
        for (int j = 0; j < cCount; j++)
        {
            float cx = (j * cHalf * 2) + cHalf;
            Instantiate(ceilingPrefab, new Vector3(cx, ceilingYOffset, 0), Quaternion.identity);
        }
    }

    private void GenerateWalls(float xOrigin, bool isLeft)
    {
        float wHalfW = GetHalfWidth(wallPrefab);
        float wHalfH = GetHalfHeight(wallPrefab);
        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float px = isLeft ? xOrigin - (col * wHalfW * 2) - wHalfW : xOrigin + (col * wHalfW * 2) + wHalfW;
                float py = wallYOffset + (row * wHalfH * 2) + wHalfH;
                Instantiate(wallPrefab, new Vector3(px, py, 0), Quaternion.identity);
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
