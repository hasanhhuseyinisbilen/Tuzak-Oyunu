using UnityEngine;

public class Level11Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int totalWidthInBlocks = 5;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject elevatorPrefab;
    [SerializeField] private GameObject sawRightPrefab;
    [SerializeField] private GameObject sawLeftPrefab;
    [SerializeField] private GameObject topSpikePrefab;
    [SerializeField] private GameObject finishSpikePrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || playerPrefab == null || 
            elevatorPrefab == null || sawRightPrefab == null || sawLeftPrefab == null || 
            topSpikePrefab == null || finishSpikePrefab == null || startIglooPrefab == null || finishIglooPrefab == null)
        {
            Debug.LogError("DİKKAT: Level11Generator içinde eksik prefab var!");
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

        float totalGroundWidth = totalWidthInBlocks * groundHalfWidth * 2;

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;

            switch (i)
            {
                case 0:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    GenerateWalls(0, true);

                    float iglooHalfHeight = GetHalfHeight(startIglooPrefab);
                    Instantiate(startIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                    float playerHalfHeight = GetHalfHeight(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(xPos, topY + playerHalfHeight, 0), Quaternion.identity);
                    break;

                case 2:
                        
                    Instantiate(elevatorPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

                    float wallHalfHeight = GetHalfHeight(wallPrefab);
                    float sawHeight3 = wallYOffset + (2 * wallHalfHeight * 2) + wallHalfHeight;
                    float sawHeight4 = wallYOffset + (3 * wallHalfHeight * 2) + wallHalfHeight;

                    Instantiate(sawRightPrefab, new Vector3(0, sawHeight3, 0), Quaternion.identity);
                    Instantiate(sawLeftPrefab, new Vector3(totalGroundWidth, sawHeight4, 0), Quaternion.identity);

                    float wallTopY = wallYOffset + (wallRows * wallHalfHeight * 2);
                    float spikeHalfH = GetHalfHeight(topSpikePrefab);
                    Instantiate(topSpikePrefab, new Vector3(xPos, wallTopY + spikeHalfH, 0), Quaternion.identity);
                    break;

                case 4:
                    float rightWallX = totalGroundWidth;
                    GenerateWalls(rightWallX, false);

                    float wallHalfWidth = GetHalfWidth(wallPrefab);
                    float wHalfHeight = GetHalfHeight(wallPrefab);
                    float wTopY = wallYOffset + (wallRows * wHalfHeight * 2);

                    float finalX_spike = rightWallX + wallHalfWidth;
                    float sHalfH = GetHalfHeight(finishSpikePrefab);
                    Instantiate(finishSpikePrefab, new Vector3(finalX_spike, wTopY + sHalfH, 0), Quaternion.identity);

                    float finalX_igloo = rightWallX + (3 * wallHalfWidth);
                    float iglooHalfH = GetHalfHeight(finishIglooPrefab);
                    Instantiate(finishIglooPrefab, new Vector3(finalX_igloo, wTopY + iglooHalfH, 0), Quaternion.identity);
                    break;

                default:
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
