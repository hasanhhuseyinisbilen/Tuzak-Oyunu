using UnityEngine;

public class Level17Generator : MonoBehaviour
{
    [Header("Foundation Settings")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject trapCeilingPrefab;
    [SerializeField] private GameObject ceilingSpikePrefab;
    [SerializeField] private int totalWidthInBlocks = 16;
    [SerializeField] private float ceilingYOffset = 6f;
    [SerializeField] private float ground7YOffset = -1f;

    [Header("Wall Settings")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Special Objects")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject platformPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || trapCeilingPrefab == null || 
            ceilingSpikePrefab == null || wallPrefab == null || playerPrefab == null || 
            finishIglooPrefab == null || platformPrefab == null)
        {
            Debug.LogError("Level17Generator: Prefabs are missing!");
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
        float platHalfHeight = GetHalfHeight(platformPrefab);
        float platHalfWidth = GetHalfWidth(platformPrefab);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            float groundY = (i == 6) ? ground7YOffset : 0f;
            Instantiate(groundPrefab, new Vector3(xPos, groundY, 0), Quaternion.identity);

            if (i == 0)
            {
                GenerateWalls(0, true);
                
                float platX = platHalfWidth;
                float platY = groundHalfHeight + platHalfHeight;
                Instantiate(platformPrefab, new Vector3(platX, platY, 0), Quaternion.identity);
                
                float playerY = platY + platHalfHeight + GetHalfHeight(playerPrefab);
                Instantiate(playerPrefab, new Vector3(platX, playerY, 0), Quaternion.identity);

                float finishIglooHalfWidth = GetHalfWidth(finishIglooPrefab);
                float finishIglooX = platX + platHalfWidth + finishIglooHalfWidth;
                float finishIglooY = groundHalfHeight + GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(finishIglooX, finishIglooY, 0), Quaternion.Euler(0, 180, 0));
            }
            else if (i == totalWidthInBlocks - 1)
            {
                GenerateWalls((i + 1) * groundHalfWidth * 2, false);
            }

            float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
            GameObject ceilingToSpawn = (i == 9) ? trapCeilingPrefab : ceilingPrefab;
            Instantiate(ceilingToSpawn, new Vector3(xPos, ceilingYOffset + ceilingHalfHeight, 0), Quaternion.identity);

            if (i == 11)
            {
                float spikeHalfHeight = GetHalfHeight(ceilingSpikePrefab);
                float spikeY = ceilingYOffset - spikeHalfHeight;
                Instantiate(ceilingSpikePrefab, new Vector3(xPos, spikeY, 0), Quaternion.identity);
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
