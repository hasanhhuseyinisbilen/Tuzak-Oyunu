using UnityEngine;

public class Level29Generator : MonoBehaviour
{
    private static Level29Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject elevatorPrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 20;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;

    private float groundHalfHeight;
    private float wallHalfHeight;
    private float platformHalfHeight;

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
        platformHalfHeight = GetHalfHeight(platformPrefab);
        float groundWidth = GetHalfWidth(groundPrefab) * 2f;

        GenerateLeftWalls(0);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundWidth) + (groundWidth / 2f);

            if (i == totalWidthInBlocks - 1)
            {
                GenerateRightWalls(xPos + (groundWidth / 2f));
            }

            switch (i)
            {
                case 0:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    break;

                case 1:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    break;

                case 2:
                    if (elevatorPrefab != null)
                        Instantiate(elevatorPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    break;

                case 4:
                    float currentY = 0f;
                    if (wallPrefab != null)
                    {
                        for (int r = 0; r < 2; r++)
                        {
                            float y = currentY + wallHalfHeight;
                            Instantiate(wallPrefab, new Vector3(xPos, y, 0), Quaternion.identity);
                            currentY += wallHalfHeight * 2f;
                        }
                    }
                    if (platformPrefab != null)
                    {
                        Instantiate(platformPrefab, new Vector3(xPos, currentY + platformHalfHeight, 0), Quaternion.identity);
                    }
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (finishMushroomPrefab != null)
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    break;

                default:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    break;
            }
        }
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
                float yPos = (row * wallHH * 2) + wallHH + wallYOffset;
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
                float yPos = (row * wallHH * 2) + wallHH + wallYOffset;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }
}
