using UnityEngine;

public class Level12Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject roomGroundPrefab;
    [SerializeField] private int totalWidthInBlocks = 5;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject elevatorPrefab;
    [SerializeField] private GameObject ledgeSpikePrefab;
    [SerializeField] private GameObject ledgeSawPrefab;
    [SerializeField] private GameObject ceilingSpikePrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || roomGroundPrefab == null || wallPrefab == null || 
            ceilingPrefab == null || playerPrefab == null || elevatorPrefab == null || 
            ledgeSpikePrefab == null || ledgeSawPrefab == null || ceilingSpikePrefab == null || 
            startIglooPrefab == null || finishIglooPrefab == null)
        {
            Debug.LogError("DİKKAT: Level12Generator içinde eksik prefab var!");
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

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;

            if (i == 0)
            {
                Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                GenerateWalls(0, true, 16);

                float iglooHalfHeight = GetHalfHeight(startIglooPrefab);
                Instantiate(startIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                float playerHalfHeight = GetHalfHeight(playerPrefab);
                Instantiate(playerPrefab, new Vector3(xPos, topY + playerHalfHeight, 0), Quaternion.identity);
            }
            else if (i == 2)
            {
                Instantiate(elevatorPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            }

            if (i == totalWidthInBlocks - 1)
            {
                GenerateSpecialRightWall(totalWidthInBlocks * groundHalfWidth * 2);
            }
        }
    }

    private void GenerateSpecialRightWall(float xOrigin)
    {
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);
        float roomWidth = wallColumns * wallHalfWidth * 2;
        float wallStartX = xOrigin + roomWidth;

        // Ana Sağ Kule (16 sıra)
        GenerateWalls(wallStartX, false, 16);

        int currentRow = 0;
        // 1. Taban (3 sıra tam duvar)
        for (int r = 0; r < 3; r++) PlaceWallRow(xOrigin, currentRow++, wallHalfWidth, wallHalfHeight);
        
        // 2. 1. Odacık Zemini (Diken ile)
        PlaceGroundRow(xOrigin, currentRow, wallHalfWidth, wallHalfHeight, 2, ledgeSpikePrefab);
        // 1. Odacık Testeresi (Hesaplama hatası düzeltildi: Zemin yüzeyinden 1 birim yukarı)
        float roomGHH = GetHalfHeight(roomGroundPrefab);
        float sawHH = GetHalfHeight(ledgeSawPrefab);
        float sawY1 = wallYOffset + (currentRow * wallHalfHeight * 2) + (roomGHH * 2) + sawHH + 1.0f;
        Instantiate(ledgeSawPrefab, new Vector3(wallStartX, sawY1, 0), Quaternion.identity);
        currentRow++;
        
        // 3. 1. Odacık Boşluğu
        currentRow++; 

        // 4. Ara Kat Tavanı ve Duvarları
        PlaceCeilingRow(xOrigin, currentRow, wallHalfWidth, wallHalfHeight, new int[] {7, 8, 9, 10}, ceilingSpikePrefab);
        for (int r = 0; r < 3; r++) PlaceWallRow(xOrigin, currentRow++, wallHalfWidth, wallHalfHeight);

        // 5. 2. Odacık Zemini
        PlaceGroundRow(xOrigin, currentRow, wallHalfWidth, wallHalfHeight);
        // 2. Odacık Testeresi (Hesaplama hatası düzeltildi: Zemin yüzeyinden 1 birim yukarı)
        float sawY2 = wallYOffset + (currentRow * wallHalfHeight * 2) + (roomGHH * 2) + sawHH + 1.0f;
        Instantiate(ledgeSawPrefab, new Vector3(wallStartX, sawY2, 0), Quaternion.identity);
        currentRow++;
        
        // 6. 2. Odacık Boşluğu
        currentRow++;

        // 7. En Üst Tavan ve Tepe Duvarları
        PlaceCeilingRow(xOrigin, currentRow, wallHalfWidth, wallHalfHeight, null, null);
        for (int r = 0; r < 2; r++) PlaceWallRow(xOrigin, currentRow++, wallHalfWidth, wallHalfHeight);

        // Bitiş İglosu (2. Odacıktan sonraki kule yapısının en tepesinde)
        float nicheTopY = wallYOffset + (currentRow * wallHalfHeight * 2);
        float iglooHalfH = GetHalfHeight(finishIglooPrefab);
        Instantiate(finishIglooPrefab, new Vector3(xOrigin + wallHalfWidth, nicheTopY + iglooHalfH, 0), Quaternion.identity);
    }

    private void PlaceWallRow(float xOrigin, int rowIndex, float wHW, float wHH)
    {
        for (int col = 0; col < wallColumns; col++)
        {
            float xPos = xOrigin + (col * wHW * 2) + wHW;
            float yPos = wallYOffset + (rowIndex * wHH * 2) + wHH;
            Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
        }
    }

    private void PlaceGroundRow(float xOrigin, int rowIndex, float wHW, float wHH, int spikeIdx = -1, GameObject spikePrefab = null, int sawIdx = -1, GameObject sawPrefab = null)
    {
        GameObject prefabToUse = roomGroundPrefab;
        float gHW = GetHalfWidth(prefabToUse);
        float gHH = GetHalfHeight(prefabToUse);
        float totalWallWidth = wallColumns * wHW * 2;
        int count = Mathf.CeilToInt(totalWallWidth / (gHW * 2));

        int finalSpikeIdx = (spikeIdx == -2) ? count - 1 : spikeIdx;
        int finalSawIdx = (sawIdx == -2) ? count - 1 : sawIdx;

        for (int i = 0; i < count; i++)
        {
            float xPos = xOrigin + (i * gHW * 2) + gHW;
            float yPos = wallYOffset + (rowIndex * wHH * 2) + gHH;
            Instantiate(prefabToUse, new Vector3(xPos, yPos, 0), Quaternion.identity);

            if (i == finalSpikeIdx && spikePrefab != null)
            {
                float sHH = GetHalfHeight(spikePrefab);
                Instantiate(spikePrefab, new Vector3(xPos, yPos + gHH + sHH, 0), Quaternion.identity);
            }

            if (i == finalSawIdx && sawPrefab != null)
            {
                float sHH = GetHalfHeight(sawPrefab);
                Instantiate(sawPrefab, new Vector3(xPos, yPos + gHH + sHH, 0), Quaternion.identity);
            }
        }
    }

    private void PlaceCeilingRow(float xOrigin, int rowIndex, float wHW, float wHH, int[] spikeIndices = null, GameObject spikePrefab = null)
    {
        float cHW = GetHalfWidth(ceilingPrefab);
        float cHH = GetHalfHeight(ceilingPrefab);
        float totalWallWidth = wallColumns * wHW * 2;
        int count = Mathf.CeilToInt(totalWallWidth / (cHW * 2));

        for (int i = 0; i < count; i++)
        {
            float xPos = xOrigin + (i * cHW * 2) + cHW;
            float yPos = wallYOffset + (rowIndex * wHH * 2) - cHH;
            Instantiate(ceilingPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
        }

        if (spikeIndices != null && spikePrefab != null)
        {
            float sHW = GetHalfWidth(spikePrefab);
            float sHH = GetHalfHeight(spikePrefab);
            float yPosBottom = wallYOffset + (rowIndex * wHH * 2) - (cHH * 2);

            foreach (int i in spikeIndices)
            {
                float xPos = xOrigin + (i * sHW * 2) + sHW;
                Instantiate(spikePrefab, new Vector3(xPos, yPosBottom - sHH, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateWalls(float xOrigin, bool isLeft, int rows)
    {
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < rows; row++)
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
