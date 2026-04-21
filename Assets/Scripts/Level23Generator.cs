using UnityEngine;

public class Level23Generator : MonoBehaviour
{
    private static Level23Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject mushroomPrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 12;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float ceilingHeight = 6f;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private float fallSpeed = 0.5f;

    private float groundHalfWidth;
    private float groundHalfHeight;
    private Transform ceilingParent;

    private void Awake()
    {
        // Prevent multiple generators from running and causing lag
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        totalWidthInBlocks = 12;
        GenerateLevel();
    }

    private void Update()
    {
        if (ceilingParent != null)
        {
            // Simple movement in Update
            ceilingParent.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        }
    }

    private float GetHalfWidth(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        return (sr != null) ? sr.bounds.extents.x : 0.5f;
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        return (sr != null) ? sr.bounds.extents.y : 0.5f;
    }

    private void GenerateLevel()
    {
        groundHalfWidth = GetHalfWidth(groundPrefab);
        groundHalfHeight = GetHalfHeight(groundPrefab);

        float startX = 0f;
        float currentX = startX;

        GenerateWalls(startX, true);

        GameObject cpObj = new GameObject("CeilingParent");
        ceilingParent = cpObj.transform;
        ceilingParent.SetParent(transform);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float groundX = currentX + groundHalfWidth;
            float topY = groundHalfHeight;

            Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
            GameObject ceiling = Instantiate(ceilingPrefab, new Vector3(groundX, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
            
            foreach (var rb2d in ceiling.GetComponentsInChildren<Rigidbody2D>())
            {
                rb2d.simulated = false;
            }
            foreach (var tb in ceiling.GetComponentsInChildren<TavanBlogu>())
            {
                tb.enabled = false;
            }

            ceiling.transform.SetParent(ceilingParent);

            switch (i)
            {
                case 0:
                    float igHW = GetHalfWidth(startIglooPrefab);
                    Instantiate(startIglooPrefab, new Vector3(groundX, topY + GetHalfHeight(startIglooPrefab), 0), Quaternion.Euler(0, 180, 0));
                    float tHW = GetHalfWidth(trampolinePrefab);
                    Instantiate(trampolinePrefab, new Vector3(groundX + igHW + tHW + 0.5f, topY + GetHalfHeight(trampolinePrefab), 0), Quaternion.identity);
                    Instantiate(playerPrefab, new Vector3(groundX, topY + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    SpawnSpikesOnBlock(groundX, topY, groundX - groundHalfWidth + (igHW * 2f) + (tHW * 2f) + 1.5f, groundX + groundHalfWidth);
                    break;
                case 1:
                    Instantiate(platformPrefab, new Vector3(groundX, ceilingHeight / 2f, 0), Quaternion.identity);
                    SpawnSpikesOnBlock(groundX, topY);
                    break;
                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(finishMushroomPrefab, new Vector3(groundX, topY + GetHalfHeight(finishMushroomPrefab) - 0.1f, 0), Quaternion.identity);
                    break;
                default:
                    SpawnSpikesOnBlock(groundX, topY);
                    break;
            }
            currentX += groundHalfWidth * 2f;
        }
        GenerateWalls(currentX, false);
    }

    private void SpawnSpikesOnBlock(float groundX, float topY, float minX = -10000f, float maxX = 10000f)
    {
        float sHW = GetHalfWidth(groundSpikePrefab);
        float sFullW = sHW * 2f;
        int spikeCount = Mathf.FloorToInt((groundHalfWidth * 2) / sFullW);
        if (spikeCount > 5) spikeCount = 5;

        float startSpikeX = groundX - groundHalfWidth + sHW;
        for (int s = 0; s < spikeCount; s++)
        {
            float spikeX = startSpikeX + (s * sFullW);
            if (spikeX >= minX && spikeX <= maxX)
            {
                Instantiate(groundSpikePrefab, new Vector3(spikeX, topY + GetHalfHeight(groundSpikePrefab), 0), Quaternion.identity);
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
                float xPos = isLeft ? xOrigin - (col * wallHalfWidth * 2) - wallHalfWidth : xOrigin + (col * wallHalfWidth * 2) + wallHalfWidth;
                float yPos = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }
}
