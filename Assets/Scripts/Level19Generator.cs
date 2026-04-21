using UnityEngine;

public class Level19Generator : MonoBehaviour
{
    [Header("Foundation Settings")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int totalWidthInBlocks = 20;

    [Header("Wall Settings")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Special Objects")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject highGroundPrefab;

    [Header("Trampoline Pattern Settings")]
    [SerializeField] private GameObject cleanTrampPrefab;
    [SerializeField] private GameObject explodingTrampPrefab;
    [SerializeField] private GameObject spikedTrampPrefab;
    [SerializeField] private GameObject emptyTrampPrefab;
    [SerializeField] private float trampStepHeight = 1.2f;
    [SerializeField] private float trampBaseYOffset = 0f;
    [Header("VFX")]
    [SerializeField] private GameObject trampEffectPrefab;
    [SerializeField] private GameObject particleSystemPrefab;
    [SerializeField] private float particleYOffset = 2.0f;
    [SerializeField] private GameObject bgPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || 
            playerPrefab == null || startIglooPrefab == null || 
            finishIglooPrefab == null || cleanTrampPrefab == null ||
            explodingTrampPrefab == null || spikedTrampPrefab == null ||
            emptyTrampPrefab == null || highGroundPrefab == null)
        {
            Debug.LogError("Level19Generator: Prefabs are missing!");
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

       
        if (bgPrefab != null)
        {
            float bgW = GetHalfWidth(bgPrefab) * 2;
            float bgH = GetHalfHeight(bgPrefab) * 2;
            for (int k = 0; k < 2; k++)
            {
                float bgX = (k * bgW) + (bgW / 2f);
                float bgY = groundHalfHeight + (bgH / 2f);
                
                Quaternion rotation = (k == 1) ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                
                GameObject bg = Instantiate(bgPrefab, new Vector3(bgX, bgY, 10f), rotation);
                bg.name = $"BG_{k}";
                bg.transform.parent = this.transform;

            }
        }

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    GenerateStartArea(xPos, groundHalfHeight);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    GenerateTrampoline(i - 1, xPos, groundHalfHeight);
                    break;
                case 11:
                    GenerateHighGround(10, xPos, groundHalfHeight);
                    break;
                case int n when n == totalWidthInBlocks - 1:
                    GenerateFinishArea(xPos, groundHalfWidth, groundHalfHeight);
                    break;
            }
        }
    }

    private void GenerateTrampoline(int stepIndex, float xPos, float groundHalfHeight)
    {
        GameObject prefabToUse = cleanTrampPrefab;

        switch (stepIndex)
        {
            case 2: 
                prefabToUse = explodingTrampPrefab;
                break;
            case 5: 
                prefabToUse = spikedTrampPrefab;
                break;
            case 7: 
                prefabToUse = emptyTrampPrefab;
                break;
        }

        if (prefabToUse != null)
        {
            float trampHalfHeight = GetHalfHeight(prefabToUse);
            float trampY = groundHalfHeight + (stepIndex * trampStepHeight) + trampHalfHeight + trampBaseYOffset;
            Instantiate(prefabToUse, new Vector3(xPos, trampY, 0), Quaternion.identity);
        }
    }

    private void GenerateHighGround(int stepIndex, float xPos, float groundHalfHeight)
    {
        if (highGroundPrefab != null)
        {
            float highGroundHalfHeight = GetHalfHeight(highGroundPrefab);
            float highGroundY = groundHalfHeight + (stepIndex * trampStepHeight) + highGroundHalfHeight + trampBaseYOffset;
            Instantiate(highGroundPrefab, new Vector3(xPos, highGroundY, 0), Quaternion.identity);

            if (particleSystemPrefab != null)
            {
                float particleY = highGroundY + highGroundHalfHeight + particleYOffset;
                Instantiate(particleSystemPrefab, new Vector3(xPos, particleY, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateStartArea(float xPos, float groundHalfHeight)
    {
        GenerateWalls(0, true);
        float startIglooY = groundHalfHeight + GetHalfHeight(startIglooPrefab);
        Instantiate(startIglooPrefab, new Vector3(xPos, startIglooY, 0), Quaternion.Euler(0, 180, 0));
        float playerY = groundHalfHeight + GetHalfHeight(playerPrefab);
        Instantiate(playerPrefab, new Vector3(xPos, playerY, 0), Quaternion.identity);
    }

    private void GenerateFinishArea(float xPos, float groundHalfWidth, float groundHalfHeight)
    {
        GenerateWalls(totalWidthInBlocks * groundHalfWidth * 2, false);
        float finishY = groundHalfHeight + GetHalfHeight(finishIglooPrefab);
        Instantiate(finishIglooPrefab, new Vector3(xPos, finishY, 0), Quaternion.identity);
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
