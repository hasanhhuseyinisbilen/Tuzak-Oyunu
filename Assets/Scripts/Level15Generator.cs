using UnityEngine;

public class Level15Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject fallingGroundPrefab; 
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private int totalWidthInBlocks = 16;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject sawPrefab; 
    [SerializeField] private float ceilingYOffset = 6f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject abyssSawPrefab; 
    [SerializeField] private GameObject middleSawPrefab; 
    [SerializeField] private float middleSawYHeight = 2f; 

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || fallingGroundPrefab == null || groundSpikePrefab == null || 
            ceilingPrefab == null || sawPrefab == null || wallPrefab == null || 
            playerPrefab == null || startIglooPrefab == null || finishIglooPrefab == null || 
            boxPrefab == null || abyssSawPrefab == null || middleSawPrefab == null)
        {
            Debug.LogError("DİKKAT: Level15Generator içinde eksik prefab var!");
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

        // 1. Düşen Zemin Tuzağı (0-5)
        FallingGround lastFallingBlock = null;

        for (int i = 0; i < 6; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            GameObject groundObj = Instantiate(fallingGroundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            
            FallingGround currentFalling = groundObj.GetComponent<FallingGround>();
            if (currentFalling != null)
            {
                if (lastFallingBlock != null) lastFallingBlock.SetNextBlock(currentFalling);
                lastFallingBlock = currentFalling;
            }

            // Tavan
            float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset + ceilingHalfHeight, 0), Quaternion.identity);

            if (i == 0)
            {
                GenerateWalls(0, true);
                float iglooHalfHeight = GetHalfHeight(startIglooPrefab);
                Instantiate(startIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerXPos = xPos + GetHalfWidth(startIglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerXPos, topY + playerHalfHeight, 0), Quaternion.identity);
            }

            // Tavan Testereleri (1-5 arası)
            if (i >= 1 && i <= 5)
            {
                float sawHalfHeight = GetHalfHeight(sawPrefab);
                // Orijinal mantık: Tavan merkezinden biraz aşağıda (sawH/4f kadar)
                Instantiate(sawPrefab, new Vector3(xPos, ceilingYOffset - (sawHalfHeight / 2f), 0), Quaternion.identity);
            }
        }

        // 2. Hassas Pusu Bölgesi (Sıfır boşluklu yerleşim)
        float currentObstacleX = 6 * groundHalfWidth * 2;

        // 4 Diken
        for (int j = 0; j < 4; j++) 
        {
            float w = GetHalfWidth(groundSpikePrefab);
            float h = GetHalfHeight(groundSpikePrefab);
            Instantiate(groundSpikePrefab, new Vector3(currentObstacleX + w, topY + h, 0), Quaternion.identity);
            currentObstacleX += w * 2;
        }

        // 1. Kutu
        float boxHalfW = GetHalfWidth(boxPrefab);
        float boxHalfH = GetHalfHeight(boxPrefab);
        Instantiate(boxPrefab, new Vector3(currentObstacleX + boxHalfW, topY + boxHalfH, 0), Quaternion.identity);
        currentObstacleX += boxHalfW * 2;

        // 4 Diken daha
        for (int j = 0; j < 4; j++) 
        {
            float w = GetHalfWidth(groundSpikePrefab);
            float h = GetHalfHeight(groundSpikePrefab);
            Instantiate(groundSpikePrefab, new Vector3(currentObstacleX + w, topY + h, 0), Quaternion.identity);
            currentObstacleX += w * 2;
        }

        // 2. Kutu (Daha yüksek)
        Instantiate(boxPrefab, new Vector3(currentObstacleX + boxHalfW, topY + (boxHalfH * 2), 0), Quaternion.identity);
        currentObstacleX += boxHalfW * 2;

        // 3. Pusu Bölgesi Altı/Üstü (Zemin ve Tavan)
        int groundIndexAfterTrap = Mathf.CeilToInt(currentObstacleX / (groundHalfWidth * 2));
        for (int i = 6; i < groundIndexAfterTrap; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            float cHH = GetHalfHeight(ceilingPrefab);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset + cHH, 0), Quaternion.identity);
        }

        // 4. Uçurum (2 Blokluk Boşluk)
        int gapStartIndex = groundIndexAfterTrap;
        int gapEndIndex = gapStartIndex + 2;
        for (int i = gapStartIndex; i < gapEndIndex; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            // İlk boşlukta dipteki testere
            if (i == gapStartIndex)
            {
                float sHH = GetHalfHeight(abyssSawPrefab);
                Instantiate(abyssSawPrefab, new Vector3(xPos, topY + (sHH / 2f), 0), Quaternion.identity);
            }

            // Uçurumun tam ortasındaki özel testere
            if (i == gapStartIndex)
            {
                float middleX = (i + 1) * groundHalfWidth * 2;
                Instantiate(middleSawPrefab, new Vector3(middleX, topY + middleSawYHeight, 0), Quaternion.identity);
            }

            float cHH = GetHalfHeight(ceilingPrefab);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset + cHH, 0), Quaternion.identity);
        }

        // 5. Bitiş Alanı (2 Blok)
        for (int i = 0; i < 2; i++)
        {
            int blockIdx = gapEndIndex + i;
            float xPos = (blockIdx * groundHalfWidth * 2) + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            float cHH = GetHalfHeight(ceilingPrefab);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset + cHH, 0), Quaternion.identity);

            if (i == 1)
            {
                GenerateWalls((blockIdx + 1) * groundHalfWidth * 2, false);
                float fHH = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, topY + fHH, 0), Quaternion.identity);
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
