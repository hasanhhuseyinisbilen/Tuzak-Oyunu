using UnityEngine;

public class Level25Generator : MonoBehaviour
{
    private static Level25Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject platform2Prefab;
    [SerializeField] private GameObject bridgePrefab;
    [SerializeField] private GameObject swingingChainPrefab;
    [SerializeField] private GameObject background1Prefab;
    [SerializeField] private GameObject background2Prefab;
    [SerializeField] private GameObject background3Prefab;

    [Header("Ayarlar")]
    [SerializeField] private float platformHeightOffset = 3f;
    [SerializeField] private float bridgeHeightOffset = 0f;
    [SerializeField] private float chainHeightOffset = 12f;
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
        ValidatePrefabs();
    }

    private void ValidatePrefabs()
    {
        if (groundPrefab == null || wallPrefab == null || mushroomPrefab == null || 
            playerPrefab == null || background1Prefab == null || background2Prefab == null || background3Prefab == null)
        {
            enabled = false;
        }
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
        if (groundPrefab == null) return;

        groundHalfWidth = GetHalfWidth(groundPrefab);
        groundHalfHeight = GetHalfHeight(groundPrefab);

        float totalLevelWidth = totalWidthInBlocks * groundHalfWidth * 2f;
        float backgroundHalfWidth = GetHalfWidth(background1Prefab);
        float backgroundFullWidth = backgroundHalfWidth * 2f;
        float backgroundHalfHeight = GetHalfHeight(background1Prefab);
        int backgroundCount = Mathf.CeilToInt(totalLevelWidth / backgroundFullWidth);

        for (int i = 0; i < backgroundCount; i++)
        {
            float backgroundX = (i * backgroundFullWidth) + backgroundHalfWidth;
            GameObject selectedBackground = background1Prefab;
            
            int sequenceIndex = i % 3;
            if (sequenceIndex == 1) selectedBackground = background2Prefab;
            else if (sequenceIndex == 2) selectedBackground = background3Prefab;

            Instantiate(selectedBackground, new Vector3(backgroundX, backgroundHalfHeight, 0), Quaternion.identity);
        }

        float currentXPosition = 0f;
        GenerateWalls(currentXPosition, true);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float groundXPosition = currentXPosition + groundHalfWidth;
            float topYPosition = groundHalfHeight;

            Instantiate(groundPrefab, new Vector3(groundXPosition, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    break;

                case 1:
                    if (platformPrefab != null)
                        Instantiate(platformPrefab, new Vector3(groundXPosition, topYPosition + platformHeightOffset, 0), Quaternion.identity);
                    break;

                case 6:
                    if (bridgePrefab != null)
                        Instantiate(bridgePrefab, new Vector3(groundXPosition, topYPosition + bridgeHeightOffset, 0), Quaternion.identity);
                    break;
                
                case 10:
                case 11:
                    if (swingingChainPrefab != null)
                        Instantiate(swingingChainPrefab, new Vector3(groundXPosition, topYPosition + chainHeightOffset, 0), Quaternion.identity);
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    if (finishMushroomPrefab != null)
                        Instantiate(finishMushroomPrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    break;
            }
            
            currentXPosition += groundHalfWidth * 2f;
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
                float xPosition = isLeft ? xOrigin - (col * wallHalfWidth * 2) - wallHalfWidth : xOrigin + (col * wallHalfWidth * 2) + wallHalfWidth;
                float yPosition = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                Instantiate(wallPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            }
        }
    }
}
