using UnityEngine;

public class Level24Generator : MonoBehaviour
{
    private static Level24Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private GameObject alternateSawPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject nativeAmericanPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject backgroundPrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 12;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;

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
        SpriteRenderer spriteRenderer = prefab.GetComponentInChildren<SpriteRenderer>();
        return (spriteRenderer != null) ? spriteRenderer.bounds.extents.x : 0.5f;
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        SpriteRenderer spriteRenderer = prefab.GetComponentInChildren<SpriteRenderer>();
        return (spriteRenderer != null) ? spriteRenderer.bounds.extents.y : 0.5f;
    }

    private void GenerateLevel()
    {
        groundHalfWidth = GetHalfWidth(groundPrefab);
        groundHalfHeight = GetHalfHeight(groundPrefab);

        float totalMapWidth = totalWidthInBlocks * groundHalfWidth * 2f;
        float backgroundHalfWidth = GetHalfWidth(backgroundPrefab);
        float backgroundHalfHeight = GetHalfHeight(backgroundPrefab);
        float backgroundFullWidth = backgroundHalfWidth * 2f;
        int backgroundCount = Mathf.CeilToInt(totalMapWidth / backgroundFullWidth);

        for (int i = 0; i < backgroundCount; i++)
        {
            float backgroundX = (i * backgroundFullWidth) + backgroundHalfWidth;
            Quaternion backgroundRotation = (i == 1 || i == 3) ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
            Instantiate(backgroundPrefab, new Vector3(backgroundX, backgroundHalfHeight, 0), backgroundRotation);
        }

        float currentXPosition = 0f;
        GenerateWalls(currentXPosition, true);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float groundX = currentXPosition + groundHalfWidth;
            float topYPosition = groundHalfHeight;

            Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    Instantiate(mushroomPrefab, new Vector3(groundX, topYPosition + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    Instantiate(playerPrefab, new Vector3(groundX, topYPosition + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    break;
                case 1:
                    float sawHalfHeight = GetHalfHeight(sawPrefab);
                    float sawHalfWidth = GetHalfWidth(sawPrefab);
                    Instantiate(sawPrefab, new Vector3(currentXPosition + sawHalfWidth, topYPosition + (sawHalfHeight * 2.5f), 0), Quaternion.identity);
                    Instantiate(sawPrefab, new Vector3(groundX - sawHalfWidth, topYPosition, 0), Quaternion.identity);
                    break;
                case 4:
                    if (alternateSawPrefab != null)
                    {
                        Instantiate(alternateSawPrefab, new Vector3(groundX, topYPosition, 0), Quaternion.identity);
                    }
                    if (platformPrefab != null)
                    {
                        float sawHalfHeightForPlatform = GetHalfHeight(sawPrefab);
                        Instantiate(platformPrefab, new Vector3(groundX, topYPosition + (sawHalfHeightForPlatform * 1.5f), 0), Quaternion.identity);
                    }
                    break;
                case 7:
                    if (nativeAmericanPrefab != null)
                    {
                        float nativeHalfHeight = GetHalfHeight(nativeAmericanPrefab);
                        Instantiate(nativeAmericanPrefab, new Vector3(groundX, topYPosition + nativeHalfHeight, 0), Quaternion.identity);
                    }
                    break;
                case 10:
                    if (spikePrefab != null)
                    {
                        float spikeHalfHeight = GetHalfHeight(spikePrefab);
                        Instantiate(spikePrefab, new Vector3(groundX, topYPosition + spikeHalfHeight, 0), Quaternion.identity);
                    }
                    break;
                case int n when (n == totalWidthInBlocks - 1):
                    if (finishMushroomPrefab != null)
                    {
                        Instantiate(finishMushroomPrefab, new Vector3(groundX, topYPosition + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    }
                    break;
            }
            currentXPosition += groundHalfWidth * 2f;
        }
        GenerateWalls(currentXPosition, false);
    }

    private void GenerateWalls(float xOrigin, bool isLeft)
    {
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);
        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xPosition = isLeft ? xOrigin - (col * wallHalfWidth * 2) - wallHalfWidth : xOrigin + (col * wallHalfWidth * 2) + wallHalfWidth;
                float yPosition = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                Instantiate(wallPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            }
        }
    }
}
