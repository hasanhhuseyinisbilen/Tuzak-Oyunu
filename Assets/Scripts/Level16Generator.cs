using UnityEngine;

public class Level16Generator : MonoBehaviour
{
    [Header("Foundation Settings")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private int totalWidthInBlocks = 20;
    [SerializeField] private float ceilingYOffset = 6f;

    [Header("Background Settings")]
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private float backgroundYOffset = 0f;

    [Header("Wall Settings")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Special Objects")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject spikyBoxPrefab;
    [SerializeField] private GameObject ground15SpikePrefab;

    [Header("Storm Settings")]
    [SerializeField] private GameObject snowyPrefab;
    [SerializeField] private GameObject stormTriggerPrefab;
    [SerializeField] private GameObject spikeBehindTriggerPrefab;
    [SerializeField] private int stormTriggerIndex = 5; 

    [Header("Special Structure Settings")]
    [SerializeField] private GameObject structurePrefab;
    [SerializeField] private float structureYPos = 0f;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            structurePrefab == null || playerPrefab == null || finishIglooPrefab == null || 
            spikePrefab == null || stormTriggerPrefab == null || snowyPrefab == null || 
            spikeBehindTriggerPrefab == null || spikyBoxPrefab == null || ground15SpikePrefab == null ||
            backgroundPrefab == null)
        {
            Debug.LogError("DİKKAT: Level16Generator içinde prefablar eksik!");
            canGenerate = false;
            enabled = false;
        }
    }

    void Start()
    {
        if (canGenerate) 
        {
            if (snowyPrefab != null) Instantiate(snowyPrefab);
            GenerateLevel();

            if (spikyBoxPrefab != null)
            {
                float groundHalfHeight = GetHalfHeight(groundPrefab);
                float boxHalfWidth = GetHalfWidth(spikyBoxPrefab);
                float boxHalfHeight = GetHalfHeight(spikyBoxPrefab);
                Instantiate(spikyBoxPrefab, new Vector3(-boxHalfWidth - 1f, groundHalfHeight + boxHalfHeight, 0), Quaternion.Euler(180f, 0, 180f));
            }
        }
    }

    private void GenerateLevel()
    {
        float groundHalfWidth = GetHalfWidth(groundPrefab);
        float groundHalfHeight = GetHalfHeight(groundPrefab);
        float topY = groundHalfHeight;
        int middleIndex = totalWidthInBlocks / 2;

        if (backgroundPrefab != null)
        {
            float firstGroundX = groundHalfWidth;
            GameObject bg = Instantiate(backgroundPrefab, new Vector3(firstGroundX, topY + backgroundYOffset, 0), Quaternion.identity);
            
            ParallaxEffect parallax = bg.AddComponent<ParallaxEffect>();
            parallax.parallaxFactorX = 1f;
            parallax.infiniteScrolling = true;
        }

        
        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    GenerateWalls(0, true);
                    break;

                case 5:
                    if (stormTriggerPrefab != null)
                    {
                        float triggerY = groundHalfHeight + 2f;
                        Instantiate(stormTriggerPrefab, new Vector3(xPos, triggerY, 0), Quaternion.identity);
                    }

                    if (spikePrefab != null)
                    {
                        float spikeX = xPos - (groundHalfWidth * 2f);
                        float spikeHalfHeight = GetHalfHeight(spikePrefab);
                        float spikeY = groundHalfHeight + spikeHalfHeight;
                        Instantiate(spikePrefab, new Vector3(spikeX, spikeY, 0), Quaternion.identity);
                    }
                    break;

                case 14:
                    if (ground15SpikePrefab != null)
                    {
                        float spikeHeight = GetHalfHeight(ground15SpikePrefab);
                        Instantiate(ground15SpikePrefab, new Vector3(xPos, groundHalfHeight + spikeHeight, 0), Quaternion.identity);
                    }
                    break;

                default:
                    if (i == middleIndex)
                    {
                        float structHalfWidth = GetHalfWidth(structurePrefab);
                        float structHalfHeight = GetHalfHeight(structurePrefab);
                        float finalY = groundHalfHeight + structHalfHeight + structureYPos;
                        Instantiate(structurePrefab, new Vector3(xPos, finalY, 0), Quaternion.identity);

                        float structureTopY = finalY + structHalfHeight;
                        float playerHalfHeight = GetHalfHeight(playerPrefab);
                        float playerHalfWidth = GetHalfWidth(playerPrefab);
                        float playerXPos = xPos - structHalfWidth + playerHalfWidth;
                        Instantiate(playerPrefab, new Vector3(playerXPos, structureTopY + playerHalfHeight, 0), Quaternion.identity);
                    }

                    if (i == totalWidthInBlocks - 1)
                    {
                        GenerateWalls((i + 1) * groundHalfWidth * 2, false);
                        float finishIglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                        Instantiate(finishIglooPrefab, new Vector3(xPos, groundHalfHeight + finishIglooHalfHeight, 0), Quaternion.identity);
                    }
                    break;
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

                switch (row)
                {
                    case 3:
                        if (isLeft && col == 0)
                        {
                            float spikeHalfHeight = GetHalfHeight(spikePrefab); 
                            float spikeX = xOrigin + spikeHalfHeight;
                            float wallBottomY = yPos - wallHalfHeight;
                            float segmentHeight = (wallHalfHeight * 2f) / 4f;

                            for (int s = 0; s < 4; s++)
                            {
                                float spikeY = wallBottomY + (segmentHeight * s) + (segmentHeight / 2f);
                                Instantiate(spikePrefab, new Vector3(spikeX, spikeY, 0), Quaternion.Euler(0, 0, 270));
                            }
                        }
                        break;
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
