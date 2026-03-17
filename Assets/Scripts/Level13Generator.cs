using UnityEngine;

public class Level13Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int totalWidthInBlocks = 12;

    [Header("Duvar ve Tavan Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private int wallHeight = 11;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private float ceilingY = 5f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject trampoline2Prefab; 
    [SerializeField] private GameObject boxPrefab; 
    [SerializeField] private GameObject elevatorBoxPrefab; 
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || ceilingPrefab == null || 
            playerPrefab == null || trampolinePrefab == null || trampoline2Prefab == null || 
            boxPrefab == null || elevatorBoxPrefab == null || spikePrefab == null || 
            startIglooPrefab == null || finishIglooPrefab == null)
        {
            Debug.LogError("DİKKAT: Level13Generator içinde eksik prefab var!");
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
            
            bool shouldHaveGround = (i < 5 || i == totalWidthInBlocks - 1);
            if (shouldHaveGround)
            {
                Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            }

            if (i == 0)
            {
                GenerateWalls(0, true, wallHeight);

                float iglooHalfHeight = GetHalfHeight(startIglooPrefab);
                Instantiate(startIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerXPos = xPos + GetHalfWidth(startIglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerXPos, topY + playerHalfHeight, 0), Quaternion.identity);
            }
            
            if (i == 1)
            {
                float trampHalfH = GetHalfHeight(trampolinePrefab);
                Instantiate(trampolinePrefab, new Vector3(xPos, topY + trampHalfH, 0), Quaternion.identity);
            }

            if (i == 3)
            {
                float trampHalfH = GetHalfHeight(trampoline2Prefab);
                float trampHalfW = GetHalfWidth(trampoline2Prefab);
                Instantiate(trampoline2Prefab, new Vector3(xPos, topY + trampHalfH, 0), Quaternion.identity);

                float spikeHalfW = GetHalfWidth(spikePrefab);
                float spikeHalfH = GetHalfHeight(spikePrefab);
                float trampEndX = xPos + trampHalfW;

                for (int j = 0; j < 5; j++)
                {
                    float spikeX = trampEndX + (j * spikeHalfW * 2) + spikeHalfW;
                    Instantiate(spikePrefab, new Vector3(spikeX, topY + spikeHalfH, 0), Quaternion.identity);
                }
            }
            
            if (i == 6 || i == 8 || i == 10)
            {
                int step = (i == 6) ? 1 : (i == 8) ? 2 : 3;
                float boxHalfH = GetHalfHeight(boxPrefab);
                float boxY = topY + (step * groundHalfHeight * 2) + boxHalfH;
                
                GameObject prefabToUse = (i == 8) ? elevatorBoxPrefab : boxPrefab;
                GameObject boxObj = Instantiate(prefabToUse, new Vector3(xPos, boxY, 0), Quaternion.identity);

                if (i == 8)
                {
                    RisingBoxElevator elevator = boxObj.GetComponent<RisingBoxElevator>();
                    if (elevator == null) elevator = boxObj.AddComponent<RisingBoxElevator>();
                    
                    float finishX = ((totalWidthInBlocks - 1) * groundHalfWidth * 2) + groundHalfWidth;
                    float finishY = (3 * groundHalfHeight * 2) + groundHalfHeight + boxHalfH; 
                    elevator.SetTarget(new Vector3(finishX, finishY, 0));
                }
            }

            if (i == totalWidthInBlocks - 1)
            {
                float targetGroundY = 3 * groundHalfHeight * 2; 
                Instantiate(groundPrefab, new Vector3(xPos, targetGroundY, 0), Quaternion.identity);

                GenerateWalls(totalWidthInBlocks * groundHalfWidth * 2, false, wallHeight);

                float finishHalfHeight = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, targetGroundY + groundHalfHeight + finishHalfHeight, 0), Quaternion.identity);
            }
        }

        GenerateCeiling();
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

    private void GenerateCeiling()
    {
        float ceilingHalfWidth = GetHalfWidth(ceilingPrefab);
        float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
        
        float topY = ceilingY + ceilingHalfHeight;
        float levelEndX = totalWidthInBlocks * GetHalfWidth(groundPrefab) * 2; 
        
        float currentX = 0f;
        while (currentX < levelEndX)
        {
            Instantiate(ceilingPrefab, new Vector3(currentX + ceilingHalfWidth, topY, 0), Quaternion.identity);
            currentX += ceilingHalfWidth * 2;
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
