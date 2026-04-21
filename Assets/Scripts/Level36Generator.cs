using UnityEngine;

public class Level36Generator : MonoBehaviour
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
    [SerializeField] private GameObject hidingSpikePrefab; 
    [SerializeField] private GameObject groundSpikePrefab; 
    [SerializeField] private GameObject ceilingSpikePrefab; // Yeni: Tavan dikeni prefabı
    [SerializeField] private GameObject trampolinePrefab; // Yeni: Trambolin prefabı
    [SerializeField] private GameObject ivyPrefab; // Yeni: Sarmaşık prefabı

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

            // 1. Yükseklik Belirle (V-ŞEKİLLİ - Kullanıcının son değerleri)
            switch (i)
            {
                case 0: yPos = groundHeight * 1.0f; break;
                case 1: yPos = groundHeight * 0.6f; break;
                case 2: yPos = groundHeight * 0.2f; break;
                case 3: yPos = 0f; break; // En alt nokta
                case 4: yPos = groundHeight * 0.2f; break;
                case 5: yPos = groundHeight * 0.6f; break;
                case 6: yPos = groundHeight * 1.0f; break;
                case 7: yPos = 0f; break; // Case 6'dan sonrası 0f
                case 8: yPos = 0f; break;
                case 9: yPos = 0f; break;
            }

            // 2. Tavanı oluştur
            GameObject currentCeiling = null;
            if (ceilingPrefab != null)
                currentCeiling = Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            // 3. Zemini oluştur
            GameObject currentGround = Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);

            // 4. Ekstra Özellikler
            switch (i)
            {
                case 0: // Başlangıç + Sağa Bakan Diken
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    
                    if (hidingSpikePrefab != null)
                    {
                        float spikeHH = GetHalfHeight(hidingSpikePrefab);
                        float spikeHW = GetHalfWidth(hidingSpikePrefab);
                        Vector3 spikePos = new Vector3(xPos + groundHalfWidth + spikeHH, yPos + groundHalfHeight - spikeHW, 0);
                        GameObject spike = Instantiate(hidingSpikePrefab, spikePos, Quaternion.Euler(0, 0, -90));
                        spike.transform.SetParent(currentGround.transform);
                    }
                    break;

                case 3: // 4. Blok (0f) - Yer Dikenleri (Sığdığı kadar)
                    if (groundSpikePrefab != null)
                    {
                        float sWidth = GetHalfWidth(groundSpikePrefab) * 2f;
                        int sCount = Mathf.FloorToInt(groundWidth / sWidth);
                        float sHH = GetHalfHeight(groundSpikePrefab);
                        for (int j = 0; j < sCount; j++)
                        {
                            float sx = (xPos - groundHalfWidth) + (j * sWidth) + (sWidth / 2f);
                            Instantiate(groundSpikePrefab, new Vector3(sx, yPos + groundHalfHeight + sHH, 0), Quaternion.identity);
                        }
                    }
                    break;

                case 5: // 6. Blok - Sola Bakan Diken
                    if (hidingSpikePrefab != null)
                    {
                        float spikeHH = GetHalfHeight(hidingSpikePrefab);
                        float spikeHW = GetHalfWidth(hidingSpikePrefab);
                        Vector3 spikePos = new Vector3(xPos - groundHalfWidth - spikeHH, yPos + groundHalfHeight - spikeHW, 0);
                        GameObject spike = Instantiate(hidingSpikePrefab, spikePos, Quaternion.Euler(0, 0, 90));
                        spike.transform.SetParent(currentGround.transform);
                    }
                    break;

                case 6: // 7. BLOK: TAVAN DİKENİ
                    if (ceilingSpikePrefab != null && currentCeiling != null)
                    {
                        float ceilHH = GetHalfHeight(ceilingPrefab);
                        float spikeHH = GetHalfHeight(ceilingSpikePrefab);
                        
                        // Tavana bağla ve tam altına sıfırla
                        GameObject spike = Instantiate(ceilingSpikePrefab, Vector3.zero, Quaternion.identity);
                        spike.transform.SetParent(currentCeiling.transform);
                        
                        // Local pozisyon ile tam merkezin altına (kendi boyu + tavanın boyu kadar aşağı) indir
                        spike.transform.localPosition = new Vector3(0, -ceilHH - spikeHH, 0);
                    }
                    break;

                case 7: // 8. BLOK: Trambolin
                    if (trampolinePrefab != null)
                    {
                        float trampHH = GetHalfHeight(trampolinePrefab);
                        Vector3 trampPos = new Vector3(xPos, yPos + groundHalfHeight + trampHH, 0);
                        Instantiate(trampolinePrefab, trampPos, Quaternion.identity);
                    }
                    break;

                case int n when (n == groundCount - 1): // Bitiş
                    if (finishMushroomPrefab != null)
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    
                    if (ivyPrefab != null)
                    {
                        float ivyHH = GetHalfHeight(ivyPrefab);
                        // Bitişin orada zemine sıfır sarmaşık
                        Instantiate(ivyPrefab, new Vector3(xPos, yPos + groundHalfHeight + ivyHH, 0), Quaternion.identity);
                    }
                    
                    GenerateWalls(xPos + groundHalfWidth, false);
                    break;
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
