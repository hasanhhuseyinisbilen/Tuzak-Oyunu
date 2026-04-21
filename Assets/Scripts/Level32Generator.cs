using UnityEngine;

public class Level32Generator : MonoBehaviour
{
    private static Level32Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private GameObject woodPrefab2;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject specialPrefab;
    [SerializeField] private GameObject toxicWaterPrefab;
    [SerializeField] private GameObject cubukPrefab;
    [SerializeField] private GameObject slideDikenPrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 30;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float ceilingHeight = 6f;

    private float groundHalfHeight;
    private float wallHalfHeight;
    private float ceilingHalfHeight;
    private float playerHalfHeight;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        GenerateLevel();
    }

    private float GetHalfWidth(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null) ? r.bounds.size.x / 2f : 0.5f;
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null) ? r.bounds.size.y / 2f : 0.5f;
    }

    private void GenerateLevel()
    {
        if (groundPrefab == null) return;
        
        groundHalfHeight = GetHalfHeight(groundPrefab);
        wallHalfHeight = GetHalfHeight(wallPrefab);
        ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
        playerHalfHeight = GetHalfHeight(playerPrefab);
        
        float groundWidth = GetHalfWidth(groundPrefab) * 2f;
        float ceilingWidth = GetHalfWidth(ceilingPrefab) * 2f;

        GenerateLeftWalls(0);

        float endX = 0f;
        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundWidth) + (groundWidth / 2f);
            float ceilingXPos = (i * ceilingWidth) + (GetHalfWidth(ceilingPrefab));
            endX = (i + 1) * groundWidth;

            switch (i)
            {
                case 0:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    
                    if (mushroomPrefab != null)
                    {
                        Instantiate(mushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    }

                    if (playerPrefab != null)
                    {
                        Instantiate(playerPrefab, new Vector3(xPos, groundHalfHeight + playerHalfHeight, 0), Quaternion.identity);
                    }
                    break;
                case 1:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (woodPrefab != null)
                    {
                        float woodH = GetHalfHeight(woodPrefab);
                        Instantiate(woodPrefab, new Vector3(xPos, groundHalfHeight + woodH, 0), Quaternion.identity);
                    }

                    if (spikePrefab != null)
                    {
                        float spikeH = GetHalfHeight(spikePrefab);
                        float spikeX = xPos + (groundWidth / 2f);
                        Instantiate(spikePrefab, new Vector3(spikeX, groundHalfHeight + spikeH, 0), Quaternion.identity);
                    }
                    break;
                case 2:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (woodPrefab2 != null)
                    {
                        float woodH2 = GetHalfHeight(woodPrefab2);
                        Instantiate(woodPrefab2, new Vector3(xPos, groundHalfHeight + woodH2, 0), Quaternion.identity);
                    }
                    break;
                case 4:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (specialPrefab != null)
                    {
                        float specH = GetHalfHeight(specialPrefab);
                        float specY = groundHalfHeight + (specH * 2f);
                        Instantiate(specialPrefab, new Vector3(xPos, specY, 0), Quaternion.identity);
                    }
                    break;
                case 7:
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (toxicWaterPrefab != null)
                    {
                        float waterHalfW = GetHalfWidth(toxicWaterPrefab);
                        float waterHalfH = GetHalfHeight(toxicWaterPrefab);
                        float waterFullW = waterHalfW * 2f;
                        float waterFullH = waterHalfH * 2f;
                        float gapWidth = groundWidth * 5f; 
                        int waterCount = Mathf.CeilToInt(gapWidth / waterFullW) + 1;
                        float startX = (7 * groundWidth);

                        for (int k = 0; k < waterCount; k++)
                        {
                            float wx = startX + (k * waterFullW) + waterHalfW;
                            GameObject water = Instantiate(toxicWaterPrefab, new Vector3(wx, 0, 0), Quaternion.identity);
                            if (water.GetComponent<WaterAnimation>() == null)
                            {
                                water.AddComponent<WaterAnimation>();
                            }
                        }

                        if (cubukPrefab != null)
                        {
                            float woodH = GetHalfHeight(cubukPrefab);
                            float poleY = waterFullH * 1f;

                            for (int p = 1; p <= 4; p++)
                            {
                                float poleX = startX + (p * groundWidth);
                                Instantiate(cubukPrefab, new Vector3(poleX, poleY + woodH, 0), Quaternion.identity);

                                if (p == 3 && slideDikenPrefab != null)
                                {
                                    float spikeH = GetHalfHeight(slideDikenPrefab);
                                    Instantiate(slideDikenPrefab, new Vector3(poleX, poleY + (woodH * 2f) + spikeH, 0), Quaternion.identity);
                                }
                            }
                        }
                    }
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    break;
                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    if (finishMushroomPrefab != null)
                    {
                        float mushroomHeight = GetHalfHeight(finishMushroomPrefab);
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, groundHalfHeight + mushroomHeight, 0), Quaternion.identity);
                    }
                    break;

                default:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    break;
            }
        }
        GenerateRightWalls(endX);
    }

    private void GenerateLeftWalls(float xOrigin)
    {
        if (wallPrefab == null) return;
        float wallHW = GetHalfWidth(wallPrefab);
        float wallHH = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xOffset = (col * 2 + 1) * wallHW;
                float xPos = xOrigin - xOffset; 
                float yPos = (row * wallHH * 2) + wallHH;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateRightWalls(float xOrigin)
    {
        if (wallPrefab == null) return;
        float wallHW = GetHalfWidth(wallPrefab);
        float wallHH = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xOffset = (col * 2 + 1) * wallHW;
                float xPos = xOrigin + xOffset; 
                float yPos = (row * wallHH * 2) + wallHH;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }
}
