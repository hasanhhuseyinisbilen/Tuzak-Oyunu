using UnityEngine;
using System.Collections;


public class Level38Generator : MonoBehaviour
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
    [SerializeField] private GameObject spikedCeilingPrefab;
    [SerializeField] private float ceilingYOffset = 10f;

    [Header("Özel Objeler")]
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject movingPlatformPrefab;
    [SerializeField] private GameObject sideTrampolinePrefab;

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

            // Tavan: İlk 4 normal (hemen), sonraki 4 dikenli (3 sn sonra)
            if (i >= 4 && i <= 7)
            {
                if (spikedCeilingPrefab != null)
                    StartCoroutine(SpawnDelayed(spikedCeilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.Euler(0, 0, 180), 10f));
            }
            else
            {
                if (ceilingPrefab != null)
                    Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.Euler(0, 0, 180));
            }

            // Zemin ve Özellikler
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
                        // "sağa baksın" -> Euler(0, 180, 0)
                        Instantiate(mushroomPrefab, new Vector3(xPos, yPos + groundHalfHeight + mushHH, 0), Quaternion.Euler(0, 180, 0));
                    }
                    break;

                case 1:
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    break;

                case 2:
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (groundSpikePrefab != null)
                    {
                        float spikeWidth = GetHalfWidth(groundSpikePrefab) * 2f;
                        float startX = xPos - (spikeWidth * 10f / 2f) + (spikeWidth / 2f);

                        for (int s = 0; s < 10; s++)
                        {
                            if ((s >= 0 && s <= 3) || (s >= 6 && s <= 9))
                            {
                                float sx = startX + (s * spikeWidth);
                                float sy = yPos + groundHalfHeight + GetHalfHeight(groundSpikePrefab);
                                Instantiate(groundSpikePrefab, new Vector3(sx, sy, 0), Quaternion.identity);
                            }
                        }
                    }
                    break;
                case 9:
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (platformPrefab != null)
                    {
                        float platformHH = GetHalfHeight(platformPrefab);
                        Instantiate(platformPrefab, new Vector3(xPos, yPos + groundHalfHeight + platformYOffset + platformHH, 0), Quaternion.identity);
                    }
                    break;

                case 12: // Trambolin ve Hareket Eden Platform
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (trampolinePrefab != null)
                    {
                        float trampHH = GetHalfHeight(trampolinePrefab);
                        Instantiate(trampolinePrefab, new Vector3(xPos, yPos + groundHalfHeight + trampHH, 0), Quaternion.identity);
                    }
                    if (movingPlatformPrefab != null)
                    {
                        float platHH = GetHalfHeight(movingPlatformPrefab);
                        float platWidth = GetHalfWidth(movingPlatformPrefab) * 2f;
                        Instantiate(movingPlatformPrefab, new Vector3(xPos + platWidth, yPos + groundHalfHeight + 4f + platHH, 0), Quaternion.identity);
                    }
                    break;

                case 15: // Yan Trambolin
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    if (sideTrampolinePrefab != null)
                    {
                        float sideTrampW = GetHalfWidth(sideTrampolinePrefab);
                        // Yan tarafa, 90 derece döndürülmüş şekilde koy
                        Instantiate(sideTrampolinePrefab, new Vector3(xPos + groundHalfWidth + sideTrampW, yPos + groundHalfHeight + 2f, 0), Quaternion.Euler(0, 0, 90));
                    }
                    break;

                case int n when (n == groundCount - 1): // BİTİŞ (En Son Blok)
                    Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    
                    if (finishMushroomPrefab != null)
                    {
                        float finMushHH = GetHalfHeight(finishMushroomPrefab);
                        // Mantar her zaman zeminin üstünde olsun
                        float targetY = yPos + groundHalfHeight + finMushHH;
                        
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, targetY, 0), Quaternion.identity);
                    }
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

    private IEnumerator SpawnDelayed(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(prefab, position, rotation);
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
