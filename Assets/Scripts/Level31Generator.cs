using UnityEngine;

public class Level31Generator : MonoBehaviour
{
    private static Level31Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject fallingCeilingPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject indianPrefab;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject slidingSpikePrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 30;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float ceilingHeight = 6f;

    private float groundHalfHeight;
    private float wallHalfHeight;
    private float ceilingHalfHeight;
    private float playerHalfHeight;
    private float platformHalfHeight;
    private float trampolineHalfHeight;
    private float spikeHalfWidth;
    private float spikeHalfHeight;

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
        platformHalfHeight = GetHalfHeight(platformPrefab);
        trampolineHalfHeight = GetHalfHeight(trampolinePrefab);
        spikeHalfWidth = GetHalfWidth(spikePrefab);
        spikeHalfHeight = GetHalfHeight(spikePrefab);
        
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

                    if (playerPrefab != null)
                    {
                        Instantiate(playerPrefab, new Vector3(xPos, groundHalfHeight + playerHalfHeight, 0), Quaternion.identity);
                    }
                    if (mushroomPrefab != null)
                    {
                        Instantiate(mushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    }
                    break;
                case 1:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (trampolinePrefab != null)
                    {
                        float tX1 = xPos - (groundWidth / 2f) + GetHalfWidth(trampolinePrefab);
                        Instantiate(trampolinePrefab, new Vector3(tX1, groundHalfHeight + trampolineHalfHeight, 0), Quaternion.identity);

                        float tX2 = xPos + (groundWidth / 2f) - GetHalfWidth(trampolinePrefab);
                        float targetY = groundHalfHeight + platformHalfHeight + 1.5f + trampolineHalfHeight;
                        Instantiate(trampolinePrefab, new Vector3(tX2, targetY, 0), Quaternion.identity);
                    }
                    break;
                case 2:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (fallingCeilingPrefab != null)
                        Instantiate(fallingCeilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (platformPrefab != null)
                    {
                        float platformY = groundHalfHeight + platformHalfHeight + 1.5f;
                        Instantiate(platformPrefab, new Vector3(xPos, platformY, 0), Quaternion.Euler(0, 0, 90));

                        float groundW = GetHalfWidth(groundPrefab) * 2f;
                        float gY = platformY + platformHalfHeight + groundHalfHeight;
                        for (int j = 0; j < 5; j++)
                        {
                            float gX = xPos + (j * groundW);
                            Instantiate(groundPrefab, new Vector3(gX, gY, 0), Quaternion.identity);
                        }

                        if (spikePrefab != null)
                        {
                            float sW = spikeHalfWidth * 2f;
                            float totalTrapWidth = groundW * 5f;
                            float startX = xPos - (groundWidth / 2f);
                            float sY = gY + groundHalfHeight + spikeHalfHeight;
                            int numSpikeSlots = Mathf.FloorToInt(totalTrapWidth / sW);
                            for (int k = 0; k < numSpikeSlots; k++)
                            {
                                if (k % 4 != 0)
                                {
                                    float sX = startX + (k * sW) + spikeHalfWidth;
                                    Instantiate(spikePrefab, new Vector3(sX, sY, 0), Quaternion.identity);
                                }
                            }
                        }
                    }
                    break;
                case 3:
                case 13:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (i == 3 && fallingCeilingPrefab != null)
                        Instantiate(fallingCeilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    else if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (platformPrefab != null)
                    {
                        float platformY = groundHalfHeight + platformHalfHeight + 1.5f;
                        Instantiate(platformPrefab, new Vector3(xPos, platformY, 0), Quaternion.Euler(0, 0, 90));
                    }
                    break;
                case 12:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (slidingSpikePrefab != null)
                    {
                        float slH = GetHalfHeight(slidingSpikePrefab);
                        Instantiate(slidingSpikePrefab, new Vector3(xPos, groundHalfHeight + slH, 0), Quaternion.identity);
                    }
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (platformPrefab != null)
                    {
                        float groundW = GetHalfWidth(groundPrefab) * 2f;
                        float gY = groundHalfHeight + platformHalfHeight + 1.5f + platformHalfHeight + groundHalfHeight;
                        
                        for (int j = 0; j < 5; j++)
                        {
                            float gX = xPos + (j * groundW);
                            Instantiate(groundPrefab, new Vector3(gX, gY, 0), Quaternion.identity);
                        }

                        float platformX = xPos + (4 * groundW);
                        float platformY = gY + groundHalfHeight + platformHalfHeight;
                        Instantiate(platformPrefab, new Vector3(platformX, platformY, 0), Quaternion.Euler(0, 0, 90));

                        if (spikePrefab != null)
                        {
                            float sW = spikeHalfWidth * 2f;
                            float totalTrapWidth = groundW * 5f;
                            float startX = xPos - (groundWidth / 2f);
                            float sY = gY + groundHalfHeight + spikeHalfHeight;
                            int numSpikeSlots = Mathf.FloorToInt(totalTrapWidth / sW);
                            for (int k = 0; k < numSpikeSlots; k++)
                            {
                                if (k % 4 != 0)
                                {
                                    float sX = startX + (k * sW) + spikeHalfWidth;
                                    Instantiate(spikePrefab, new Vector3(sX, sY, 0), Quaternion.identity);
                                }
                            }
                        }
                    }
                    break;
                case 4:
                case 5:
                case 6:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (fallingCeilingPrefab != null)
                        Instantiate(fallingCeilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    break;
                case 7:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    
                    if (monsterPrefab != null)
                    {
                        float monsterH = GetHalfHeight(monsterPrefab);
                        Instantiate(monsterPrefab, new Vector3(xPos, groundHalfHeight + monsterH, 0), Quaternion.Euler(0, 180, 0));
                    }
                    break;
                case 8:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    break;
                case 9:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (indianPrefab != null)
                    {
                        float indianH = GetHalfHeight(indianPrefab);
                        Instantiate(indianPrefab, new Vector3(xPos, groundHalfHeight + indianH, 0), Quaternion.identity);
                    }
                    break;
                case 11:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (trampolinePrefab != null)
                    {
                        Instantiate(trampolinePrefab, new Vector3(xPos, groundHalfHeight + trampolineHalfHeight, 0), Quaternion.identity);

                        float tX2 = xPos + (groundWidth / 2f) - GetHalfWidth(trampolinePrefab);
                        float targetY = groundHalfHeight + platformHalfHeight + 1.5f + trampolineHalfHeight;
                        Instantiate(trampolinePrefab, new Vector3(tX2, targetY, 0), Quaternion.identity);
                    }
                    break;
                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (finishMushroomPrefab != null)
                    {
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    }
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
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
