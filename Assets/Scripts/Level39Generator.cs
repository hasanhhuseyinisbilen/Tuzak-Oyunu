using UnityEngine;
using System.Collections;

public class Level39Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 20;

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
    [SerializeField] private GameObject slidingSpikePrefab;
    [SerializeField] private GameObject ceilingSpikePrefab;
    [SerializeField] private GameObject yerliPrefab;
    [SerializeField] private GameObject arrowPrefab;

    [Header("Platform Ayarları")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private float platformYOffset = 3f;

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

        // Sol Duvarlar
        GenerateWalls(0, true);

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundWidth) + groundHalfWidth;
            float yPos = 0f;

            // Tavan yerleşimi (her blok için)
            if (ceilingPrefab != null)
            {
                GameObject currentCeiling = Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.Euler(0, 0, 180));
                
                // Case 9: Tavan dikeni ekle
                if (i == 9 && ceilingSpikePrefab != null)
                {
                    float ceilHH = GetHalfHeight(ceilingPrefab);
                    float spikeHH = GetHalfHeight(ceilingSpikePrefab);
                    Vector3 spikePos = new Vector3(xPos, ceilingYOffset - ceilHH - spikeHH, 0);
                    Instantiate(ceilingSpikePrefab, spikePos, Quaternion.identity);
                }
            }

            // Zemin ve Özellikler Switch-Case Yapısı
            switch (i)
            {
                case 0: // BAŞLANGIÇ
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (playerPrefab != null)
                    {
                        float playerHH = GetHalfHeight(playerPrefab);
                        Instantiate(playerPrefab, new Vector3(xPos, yPos + groundHalfHeight + playerHH, 0), Quaternion.identity);
                    }
                    if (mushroomPrefab != null)
                    {
                        float mushHH = GetHalfHeight(mushroomPrefab);
                        // "sağa baksın" -> Y ekseninde 180 derece dönüş (Euler 0, 180, 0)
                        Instantiate(mushroomPrefab, new Vector3(xPos, yPos + groundHalfHeight + mushHH, 0), Quaternion.Euler(0, 180, 0));
                    }
                    break;

                case 2:
                case 4:
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (slidingSpikePrefab != null)
                    {
                        float spikeHH = GetHalfHeight(slidingSpikePrefab);
                        Instantiate(slidingSpikePrefab, new Vector3(xPos, yPos + groundHalfHeight + spikeHH, 0), Quaternion.identity);
                    }
                    break;

                case 5:
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (platformPrefab != null)
                    {
                        float platHH = GetHalfHeight(platformPrefab);
                        Instantiate(platformPrefab, new Vector3(xPos, yPos + groundHalfHeight + platHH, 0), Quaternion.identity);
                    }
                    break;

                case 11:
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (yerliPrefab != null)
                    {
                        float yerliHH = GetHalfHeight(yerliPrefab);
                        Instantiate(yerliPrefab, new Vector3(xPos, yPos + groundHalfHeight + yerliHH, 0), Quaternion.identity);
                    }
                    break;

                case 13:
                case 14:
                case 15:
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (arrowPrefab != null)
                    {
                        float playerHH = GetHalfHeight(playerPrefab);
                        // Oyuncu yüksekliğinin 2 katı (playerHH * 2 = tam boy, * 2 = iki katı)
                        float arrowY = yPos + groundHalfHeight + (playerHH * 1.5f * 1.5f);
                        Instantiate(arrowPrefab, new Vector3(xPos, arrowY, 0), Quaternion.Euler(0, 0, 90));
                    }
                    break;

                case int n when (n == groundCount - 1): // BİTİŞ (En Son Blok)
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (finishMushroomPrefab != null)
                    {
                        float finMushHH = GetHalfHeight(finishMushroomPrefab);
                        float targetY = yPos + groundHalfHeight + finMushHH;
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, targetY, 0), Quaternion.identity);
                    }
                    // Sağ Duvarlar
                    GenerateWalls(xPos + groundHalfWidth, false);
                    break;

                default: // DİĞER DÜZ ZEMİNLER
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
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
