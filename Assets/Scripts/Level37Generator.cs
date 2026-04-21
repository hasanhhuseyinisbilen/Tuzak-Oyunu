using UnityEngine;

public class Level37Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 10;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 10f;

    [Header("Özel Objeler")]
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject boxPrefab; // Yeni: Kutu prefabı
    [SerializeField] private GameObject fallingBoxPrefab; // Yeni: Düşen kutu prefabı (Tuzak için)

    private float groundHalfHeight;
    private float groundHalfWidth;

    void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        if (groundPrefab == null) return;

        groundHalfHeight = GetHalfHeight(groundPrefab);
        groundHalfWidth = GetHalfWidth(groundPrefab);
        float groundWidth = groundHalfWidth * 2f;
        float groundHeight = groundHalfHeight * 2f;

        // Sol Duvarlar
        GenerateWalls(0, true);

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundWidth) + groundHalfWidth;
            float yPos = 0f;

            // 1. Yüksekliği belirle
            switch (i)
            {
                case 0: yPos = groundHeight * 1f; break;
                case 1: yPos = groundHeight * 0.5f; break;
                case 2: yPos = 0f; break; // Boşluk basılacak
                default: yPos = 0f; break;
            }

            // 2. Tavanı her zaman bas
            if (ceilingPrefab != null)
                Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            // 3. Zemin ve Özellikler
            switch (i)
            {
                case 0: // BAŞLANGIÇ (Yüksek)
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    break;

                case 1: // BASAMAK
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    break;

                case 2: // BOŞLUK (Zemin basılmıyor)
                    break;

                case int n when (n == groundCount - 1): // BİTİŞ (Düz)
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (finishMushroomPrefab != null)
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    GenerateWalls(xPos + groundHalfWidth, false);
                    break;

                default: // DİĞER DÜZ ZEMİNLER
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    break;
            }

            // 4. Kutuları oluştur (4. bloktan başla, sondan bir öncekine kadar git)
            if (i >= 3 && i <= groundCount - 2)
            {
                // 2. kutu (index 4) farklı prefab olsun
                GameObject prefabToSpawn = (i == 4) ? fallingBoxPrefab : boxPrefab;

                if (prefabToSpawn != null)
                {
                    float firstPlatformY = groundHeight * 1f; 
                    float boxY = firstPlatformY + groundHalfHeight + GetHalfHeight(prefabToSpawn);
                    Instantiate(prefabToSpawn, new Vector3(xPos, boxY, 0), Quaternion.identity);
                }
            }
        }
    }

    private void GenerateWalls(float xOrigin, bool isLeft)
    {
        if (wallPrefab == null) return;
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
