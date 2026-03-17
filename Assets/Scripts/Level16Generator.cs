using UnityEngine;

public class Level16Generator : MonoBehaviour
{
    [Header("Foundation Settings")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private int totalWidthInBlocks = 20;
    [SerializeField] private float ceilingYOffset = 6f;

    [Header("Wall Settings")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Special Objects")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject spikePrefab;

    [Header("Special Structure Settings")]
    [SerializeField] private GameObject structurePrefab;
    [SerializeField] private float structureYPos = 0f; // Kullanıcının ayarlayacağı yükseklik

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            structurePrefab == null || playerPrefab == null || finishIglooPrefab == null || spikePrefab == null)
        {
            Debug.LogError("DİKKAT: Level16Generator içinde prefablar eksik!");
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
        int middleIndex = totalWidthInBlocks / 2;
        
        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            // Ground
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            
            // Özel Yapı (Tam Ortada Başla)
            if (i == middleIndex)
            {
                float structHalfWidth = GetHalfWidth(structurePrefab);
                float structHalfHeight = GetHalfHeight(structurePrefab);
                // Zemin üstü + kullanıcının istediği ekstra yükseklik
                float finalY = groundHalfHeight + structHalfHeight + structureYPos;
                Instantiate(structurePrefab, new Vector3(xPos, finalY, 0), Quaternion.identity);

                // Yapının En Soluna Oyuncuyu Koy
                float structureTopY = finalY + structHalfHeight;
                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerHalfWidth = GetHalfWidth(playerPrefab);
                
                // Yapının sol kenarı: xPos - structHalfWidth
                // Oyuncunun merkezi: sol kenar + playerHalfWidth
                float playerXPos = xPos - structHalfWidth + playerHalfWidth;
                Instantiate(playerPrefab, new Vector3(playerXPos, structureTopY + playerHalfHeight, 0), Quaternion.identity);
            }

            // Ceiling
            float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset + ceilingHalfHeight, 0), Quaternion.identity);

            // Başlangıç Duvarı
            if (i == 0)
            {
                GenerateWalls(0, true);
            }

            // Bitiş Duvarı ve Bitiş İglosu
            if (i == totalWidthInBlocks - 1)
            {
                GenerateWalls((i + 1) * groundHalfWidth * 2, false);

                // Bitiş İglosu
                float finishIglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, groundHalfHeight + finishIglooHalfHeight, 0), Quaternion.identity);
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

                // Sol duvar 4. blok (row == 3) için 4 adet diken ekle
                if (isLeft && row == 3 && col == 0)
                {
                    float spikeHalfHeight = GetHalfHeight(spikePrefab); // Dikenin boyu (duvardan çıkan kısmı)
                    float spikeHalfWidth = GetHalfWidth(spikePrefab);   // Dikenin taban genişliği
                    
                    // Diken tabanı duvarın tam sağ yüzeyine değsin
                    float spikeX = xOrigin + spikeHalfHeight;

                    // Duvar bloğunun dikeyde tam ortasını ve sınırlarını bul (yPos merkezdir)
                    float wallBottomY = yPos - wallHalfHeight;
                    
                    // 4 tane dikeni dikeyde (Y ekseninde) duvar bloğuna yay
                    // Duvar bloğunu 4 parçaya bölüp her birine bir diken koyalım
                    float segmentHeight = (wallHalfHeight * 2f) / 4f;

                    for (int s = 0; s < 4; s++)
                    {
                        // Her segmentin ortasına koy
                        float spikeY = wallBottomY + (segmentHeight * s) + (segmentHeight / 2f);
                        Instantiate(spikePrefab, new Vector3(spikeX, spikeY, 0), Quaternion.Euler(0, 0, 270));
                    }
                }
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
