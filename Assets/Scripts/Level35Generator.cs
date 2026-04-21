using UnityEngine;

public class Level35Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 10;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private float movingWoodYOffset = 8f; // Yeni: Hareketli tahtaların başlama yüksekliği offseti

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 10f;

    [Header("Özel Objeler")]
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private GameObject movingWoodPrefab; // Yeni: Hareketli tahta parçası için prefab
    [SerializeField] private GameObject leftTrampolinePrefab; // Yeni: Sol trambolin prefabı
    [SerializeField] private GameObject hidingSpikePrefab; // Yeni: Gizli diken prefabı

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

            // Her adımda tavanı bas (Tüm level boyunca)
            if (ceilingPrefab != null)
                Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            switch (i)
            {
                case 0: // BAŞLANGIÇ: Havadaki platform
                    yPos = groundHeight;
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(xPos, yPos + groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    break;

                case 2: // HAREKETLİ TAHTA + TRAMBOLİN
                    SpawnGroundAndSpikes(xPos, groundWidth);
                    GameObject wood2 = Instantiate(movingWoodPrefab, new Vector3(xPos, groundHeight + movingWoodYOffset, 0), Quaternion.identity);
                    if (leftTrampolinePrefab != null)
                    {
                        float woodHH = GetHalfHeight(movingWoodPrefab);
                        float trampHH = GetHalfHeight(leftTrampolinePrefab);
                        GameObject tramp = Instantiate(leftTrampolinePrefab, new Vector3(xPos, wood2.transform.position.y + woodHH + trampHH, 0), Quaternion.Euler(0, 0, 90));
                        tramp.transform.SetParent(wood2.transform);
                    }
                    break;

                case 4:
                case 6:
                case 8: // SADECE HAREKETLİ TAHTALAR
                    SpawnGroundAndSpikes(xPos, groundWidth);
                    if (movingWoodPrefab != null)
                        Instantiate(movingWoodPrefab, new Vector3(xPos, groundHeight + movingWoodYOffset, 0), Quaternion.identity);
                    break;

                case 5: // SABİT TAHTA + GİZLİ DİKEN
                    SpawnGroundAndSpikes(xPos, groundWidth);
                    GameObject wood5 = Instantiate(woodPrefab, new Vector3(xPos, groundHeight, 0), Quaternion.identity);
                    if (hidingSpikePrefab != null)
                    {
                        float woodHH = GetHalfHeight(woodPrefab);
                        float spikeHH = GetHalfHeight(hidingSpikePrefab);
                        GameObject hSpike = Instantiate(hidingSpikePrefab, new Vector3(xPos, wood5.transform.position.y + woodHH + spikeHH, 0), Quaternion.identity);
                        hSpike.transform.SetParent(wood5.transform); // Beraber hareket etsinler
                    }
                    break;

                case int n when (n == groundCount - 1): // BİTİŞ BLOĞU
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    SpawnSpikes(xPos, groundWidth); // Son blokta da diken istenmişti
                    if (finishMushroomPrefab != null)
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    GenerateWalls(xPos + groundHalfWidth, false);
                    break;

                default: // STANDART BLOKLAR + SABİT TAHTA
                    SpawnGroundAndSpikes(xPos, groundWidth);
                    if (woodPrefab != null)
                        Instantiate(woodPrefab, new Vector3(xPos, groundHeight, 0), Quaternion.identity);
                    break;
            }
        }
    }

    // Kod tekrarını önlemek için yardımcı fonksiyonlar
    private void SpawnGroundAndSpikes(float xPos, float groundWidth)
    {
        Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
        SpawnSpikes(xPos, groundWidth);
    }

    private void SpawnSpikes(float xPos, float groundWidth)
    {
        if (groundSpikePrefab == null) return;
        float spikeStep = groundWidth / 4f;
        float spikeHH = GetHalfHeight(groundSpikePrefab);
        for (int s = 0; s < 4; s++)
        {
            float spikeX = (xPos - groundHalfWidth) + (s * spikeStep) + (spikeStep / 2f);
            Instantiate(groundSpikePrefab, new Vector3(spikeX, groundHalfHeight + spikeHH, 0), Quaternion.identity);
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
