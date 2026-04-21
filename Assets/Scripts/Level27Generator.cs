using UnityEngine;

public class Level27Generator : MonoBehaviour
{
    private static Level27Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject ceilingSpikePrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 20;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private float ceilingHeight = 10f;

    private float groundHalfWidth;
    private float groundHalfHeight;

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
        Renderer renderer = prefab.GetComponentInChildren<Renderer>();
        return (renderer != null) ? renderer.bounds.size.x / 2f : 0.5f;
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer renderer = prefab.GetComponentInChildren<Renderer>();
        return (renderer != null) ? renderer.bounds.size.y / 2f : 0.5f;
    }

    private void GenerateLevel()
    {
        if (groundPrefab == null) return;

        groundHalfWidth = GetHalfWidth(groundPrefab);
        groundHalfHeight = GetHalfHeight(groundPrefab);

        float startXPosition = 0f;
        float currentXPosition = startXPosition;

        GenerateWalls(startXPosition, true);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float currentGroundX = currentXPosition + groundHalfWidth;
            float topYPosition = groundHalfHeight;

            Instantiate(groundPrefab, new Vector3(currentGroundX, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(currentGroundX, topYPosition + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(currentGroundX, topYPosition + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    break;

                case 5:
                    if (platformPrefab != null)
                        Instantiate(platformPrefab, new Vector3(currentGroundX, topYPosition + GetHalfHeight(platformPrefab), 0), Quaternion.identity);
                    break;

                case 12:
                case 15:
                    if (arrowPrefab != null)
                    {
                        float arrowYPosition = topYPosition + (groundHalfWidth * 1.3f);
                        Instantiate(arrowPrefab, new Vector3(currentGroundX, arrowYPosition, 0), Quaternion.Euler(0, 0, 90));
                    }
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    if (finishMushroomPrefab != null)
                        Instantiate(finishMushroomPrefab, new Vector3(currentGroundX, topYPosition + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    break;
            }

            currentXPosition += groundHalfWidth * 2f;
        }

        if (ceilingPrefab != null)
        {
            float ceilingHalfWidth = GetHalfWidth(ceilingPrefab);
            float ceilingFullWidth = ceilingHalfWidth * 2f;
            float totalLevelWidth = totalWidthInBlocks * groundHalfWidth * 2f;
            
            int ceilingCount = Mathf.CeilToInt(totalLevelWidth / ceilingFullWidth);

            for (int i = 0; i < ceilingCount; i++)
            {
                float ceilingXPosition = startXPosition + (i * ceilingFullWidth) + ceilingHalfWidth;
                Instantiate(ceilingPrefab, new Vector3(ceilingXPosition, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                switch (i)
                {
                    case 6:
                        if (ceilingSpikePrefab != null)
                        {
                            float spikeHalfWidth = GetHalfWidth(ceilingSpikePrefab);
                            float spikeFullWidth = spikeHalfWidth * 2f;
                            float spikeYPosition = ceilingHeight - GetHalfHeight(ceilingPrefab) - GetHalfHeight(ceilingSpikePrefab);
                            
                            for (int j = 0; j < 10; j++)
                            {
                                float spikeXPosition = ceilingXPosition + (j * spikeFullWidth);
                                Instantiate(ceilingSpikePrefab, new Vector3(spikeXPosition, spikeYPosition, 0), Quaternion.identity);
                            }
                        }
                        break;
                }
            }
        }

        GenerateWalls(currentXPosition, false);
    }

    private void GenerateWalls(float xOrigin, bool isLeft)
    {
        if (wallPrefab == null) return;

        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float direction = isLeft ? -1 : 1;
                float xOffset = (col * 2 + 1) * wallHalfWidth;
                float xPosition = xOrigin + (direction * xOffset);
                
                float yPosition = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                
                Instantiate(wallPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            }
        }
    }
}
