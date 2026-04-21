using UnityEngine;

public class Level21Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;

    [Header("Tuzak Ayarları")]
    [SerializeField] private GameObject columnPrefab1;
    [SerializeField] private GameObject columnPrefab2;
    [SerializeField] private GameObject columnPrefab3;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject flyingSpikePrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private float boxSpacingX = 1f;
    [SerializeField] private GameObject horizantalSpikePrefab;
    [SerializeField] private GameObject hiddingSpikePrefab;
    [SerializeField] private GameObject sevenSpikesPrefab;
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject horizontalSawPrefab;
    [SerializeField] private GameObject treePrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 20;

    private bool canGenerate = true;
    private float groundHalfWidth;
    private float groundTopY;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || playerPrefab == null || 
            startIglooPrefab == null || finishMushroomPrefab == null || columnPrefab1 == null || 
            columnPrefab2 == null || columnPrefab3 == null || groundSpikePrefab == null || 
            flyingSpikePrefab == null || boxPrefab == null || horizantalSpikePrefab == null || 
            hiddingSpikePrefab == null || sevenSpikesPrefab == null || housePrefab == null || 
            horizontalSawPrefab == null || treePrefab == null)
        {
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
        groundHalfWidth = GetHalfWidth(groundPrefab);
        groundTopY = GetHalfHeight(groundPrefab);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float groundX = (i * groundHalfWidth * 2) + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);

            if (i == 0) GenerateWalls(0, true);
            if (i == totalWidthInBlocks - 1) GenerateWalls((i + 1) * groundHalfWidth * 2, false);
        }

        int totalSpikesPlaced = 0;
        int totalColumnsPlaced = 0;
        float currentXPosition = groundHalfWidth * 2; 

        float boxHalfHeight = GetHalfHeight(boxPrefab);
        float boxHalfWidth = GetHalfWidth(boxPrefab);
        float column3HalfHeight = GetHalfHeight(columnPrefab3);
        float boxSpawnerY = groundTopY + (column3HalfHeight * 2) + boxHalfHeight;
        float horizontalSpikeHalfHeight = GetHalfHeight(horizantalSpikePrefab);
        float sevenSpikesHalfWidth = GetHalfWidth(sevenSpikesPrefab);
        float sevenSpikesHalfHeight = GetHalfHeight(sevenSpikesPrefab);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;

            switch (i)
            {
                case 0:
                    float iglooHalfWidth = GetHalfWidth(startIglooPrefab);
                    Instantiate(startIglooPrefab, new Vector3(xPos, groundTopY + GetHalfHeight(startIglooPrefab), 0), Quaternion.Euler(0, 180, 0));
                    
                    float playerHalfWidth = GetHalfWidth(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(xPos + iglooHalfWidth + playerHalfWidth, groundTopY + GetHalfHeight(playerPrefab), 0), Quaternion.identity);

                   

                    currentXPosition = xPos + iglooHalfWidth + playerHalfWidth * 2f + (groundHalfWidth * 2);
                    break;

                case 1:
                    SpawnTwoTrees(xPos);
                    break;

                case 2:
                case 5:
                case 9:
                    totalColumnsPlaced++;
                    GameObject selectedColumn = columnPrefab1;
                    
                    if (totalColumnsPlaced == 2) selectedColumn = columnPrefab2;
                    else if (totalColumnsPlaced == 3) selectedColumn = columnPrefab3;

                    if (i == 9)
                    {
                        Instantiate(housePrefab, new Vector3(xPos, groundTopY + GetHalfHeight(housePrefab), 0), Quaternion.identity);
                    }

                    float colHalfWidth = GetHalfWidth(selectedColumn);
                    float colHalfHeight = GetHalfHeight(selectedColumn);
                    
                    currentXPosition += colHalfWidth;
                    Instantiate(selectedColumn, new Vector3(currentXPosition, groundTopY + colHalfHeight, 0), Quaternion.identity);
                    currentXPosition += colHalfWidth;

                    if (totalColumnsPlaced == 3)
                    {
                        float tempSpikeX = currentXPosition;
                        for (int s = 0; s < 7; s++)
                        {
                            tempSpikeX += sevenSpikesHalfWidth;
                            Instantiate(sevenSpikesPrefab, new Vector3(tempSpikeX, groundTopY + sevenSpikesHalfHeight, 0), Quaternion.identity);
                            tempSpikeX += sevenSpikesHalfWidth;
                        }
                    }
                    break;

                case 10:
                    currentXPosition += boxSpacingX;
                    currentXPosition += boxHalfWidth;
                    Instantiate(boxPrefab, new Vector3(currentXPosition, boxSpawnerY, 0), Quaternion.identity);
                    currentXPosition += boxHalfWidth;
                    SpawnTwoTrees(xPos);
                    break;

                case 11:
                case 13:
                    currentXPosition += boxHalfWidth;
                    Instantiate(boxPrefab, new Vector3(currentXPosition, boxSpawnerY, 0), Quaternion.identity);
                    Instantiate(horizontalSawPrefab, new Vector3(xPos, groundTopY + GetHalfHeight(horizontalSawPrefab), 0), Quaternion.identity);
                    currentXPosition += boxHalfWidth;
                    SpawnTwoTrees(xPos);
                    break;

                case 12:
                    currentXPosition += boxHalfWidth;
                    Instantiate(boxPrefab, new Vector3(currentXPosition, boxSpawnerY, 0), Quaternion.identity);
                    currentXPosition += boxHalfWidth;
                    SpawnTwoTrees(xPos);
                    break;

                case 14:
                    currentXPosition += boxHalfWidth;
                    Instantiate(boxPrefab, new Vector3(currentXPosition, boxSpawnerY, 0), Quaternion.identity);
                    Instantiate(horizantalSpikePrefab, new Vector3(currentXPosition, boxSpawnerY + boxHalfHeight + horizontalSpikeHalfHeight, 0), Quaternion.identity);
                    currentXPosition += boxHalfWidth;
                    SpawnTwoTrees(xPos);
                    break;

                case 3:
                case 4:
                case 6:
                case 7:
                case 8:
                    totalSpikesPlaced++;
                    GameObject spikeType = groundSpikePrefab;
                    if (totalSpikesPlaced == 4) spikeType = flyingSpikePrefab;

                    float spikeHalfWidth = GetHalfWidth(spikeType);
                    currentXPosition += spikeHalfWidth;
                    Instantiate(spikeType, new Vector3(currentXPosition, groundTopY + GetHalfHeight(spikeType), 0), Quaternion.identity);

                    if (i == 7)
                    {
                        Instantiate(hiddingSpikePrefab, new Vector3(xPos, groundTopY + GetHalfHeight(hiddingSpikePrefab), 0), Quaternion.identity);
                        SpawnTwoTrees(xPos);
                    }
                    if (i == 8)
                    {
                        SpawnTwoTrees(xPos);
                    }

                    currentXPosition += spikeHalfWidth;
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(finishMushroomPrefab, new Vector3(xPos, groundTopY + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    SpawnTwoTrees(xPos);
                    break;
            }
        }
    }

    private void SpawnTwoTrees(float xPos)
    {
        float treeHeight = GetHalfHeight(treePrefab);
        Instantiate(treePrefab, new Vector3(xPos - groundHalfWidth * 0.5f, groundTopY + treeHeight, 0), Quaternion.identity);
        Instantiate(treePrefab, new Vector3(xPos + groundHalfWidth * 0.5f, groundTopY + treeHeight, 0), Quaternion.identity);
    }

    private void SpawnTrees(float xPos)
    {
        float estimatedTrunkWidth = GetHalfWidth(treePrefab) * 2f * 0.4f;
        float groundWidth = groundHalfWidth * 2f;
        int maxTrees = Mathf.FloorToInt(groundWidth / estimatedTrunkWidth);
        for (int k = 0; k < maxTrees; k++)
        {
            float treeX = (xPos - groundHalfWidth) + (k * estimatedTrunkWidth) + (estimatedTrunkWidth / 2f);
            Instantiate(treePrefab, new Vector3(treeX, groundTopY + GetHalfHeight(treePrefab), 0), Quaternion.identity);
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
                float yPos = wallYOffset + (row * wallHalfHeight * 2) + wallHalfHeight;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer renderer = prefab.GetComponentInChildren<Renderer>();
        return (renderer != null) ? renderer.bounds.size.y / 2f : 0.5f;
    }

    private float GetHalfWidth(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer renderer = prefab.GetComponentInChildren<Renderer>();
        return (renderer != null) ? renderer.bounds.size.x / 2f : 0.5f;
    }
}
