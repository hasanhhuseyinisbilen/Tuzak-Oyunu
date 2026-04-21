using UnityEngine;

public class Level22Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingHeight = 10f;
    [SerializeField] private float groundSinkAmount = 3f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject housePrefab;

    [Header("Tuzak Ayarları")]
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject ceilingSpikePrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject leftTrampolinePrefab;
    [SerializeField] private GameObject swingingChainPrefab;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject treePrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 20;

    private bool canGenerate = true;
    private float groundHalfWidth;
    private float groundTopY;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || ceilingPrefab == null ||
            playerPrefab == null || startIglooPrefab == null || finishMushroomPrefab == null ||
            groundSpikePrefab == null || ceilingSpikePrefab == null || platformPrefab == null ||
            leftTrampolinePrefab == null || swingingChainPrefab == null || knifePrefab == null || housePrefab == null || treePrefab == null)
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
        float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
        float groundSpikeHalfHeight = GetHalfHeight(groundSpikePrefab);
        float groundSpikeHalfWidth = GetHalfWidth(groundSpikePrefab);
        float ceilingSpikeHalfHeight = GetHalfHeight(ceilingSpikePrefab);
        float ceilingSpikeHalfWidth = GetHalfWidth(ceilingSpikePrefab);
        float platformHalfHeight = GetHalfHeight(platformPrefab);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float groundX = (i * groundHalfWidth * 2) + groundHalfWidth;
            float groundY = (i == 1) ? -groundSinkAmount : 0f;

            Instantiate(groundPrefab, new Vector3(groundX, groundY, 0), Quaternion.identity);
            Instantiate(ceilingPrefab, new Vector3(groundX, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

            if (i == 1)
            {
                float sunkTopY = groundY + groundTopY;
                float segmentStartX = groundX - groundHalfWidth;
                float currentSpikeX = segmentStartX;
                for (int s = 0; s < 4; s++)
                {
                    currentSpikeX += groundSpikeHalfWidth;
                    Instantiate(groundSpikePrefab, new Vector3(currentSpikeX, sunkTopY + groundSpikeHalfHeight, 0), Quaternion.identity);
                    currentSpikeX += groundSpikeHalfWidth;
                }

                float ceilingSpikeY = ceilingHeight - ceilingHalfHeight - ceilingSpikeHalfHeight;
                currentSpikeX = segmentStartX;
                while (currentSpikeX + ceilingSpikeHalfWidth <= groundX + groundHalfWidth)
                {
                    currentSpikeX += ceilingSpikeHalfWidth;
                    Instantiate(ceilingSpikePrefab, new Vector3(currentSpikeX, ceilingSpikeY, 0), Quaternion.identity);
                    currentSpikeX += ceilingSpikeHalfWidth;
                }
            }

            if (i == 0) GenerateWalls(0, true);
            if (i == totalWidthInBlocks - 1) GenerateWalls((i + 1) * groundHalfWidth * 2, false);
        }

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;

            if (i == totalWidthInBlocks - 1 || i == totalWidthInBlocks - 2)
            {
                Instantiate(knifePrefab, new Vector3(xPos, groundTopY + GetHalfHeight(knifePrefab), 0), Quaternion.identity);
            }

            switch (i)
            {
                case 0:
                    Instantiate(startIglooPrefab, new Vector3(xPos, groundTopY + GetHalfHeight(startIglooPrefab), 0), Quaternion.Euler(0, 180, 0));
                    Instantiate(playerPrefab, new Vector3(xPos, groundTopY + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    break;

                case 2:
                    Instantiate(housePrefab, new Vector3(xPos, groundTopY + GetHalfHeight(housePrefab), 0), Quaternion.identity);
                    break;

                case 3:
                    Instantiate(platformPrefab, new Vector3(xPos, groundTopY + platformHalfHeight, 0), Quaternion.identity);
                    break;

                case 7:
                    Instantiate(leftTrampolinePrefab, new Vector3(xPos, groundTopY + GetHalfWidth(leftTrampolinePrefab), 0), Quaternion.Euler(0, 0, 90));
                    break;

                case 9:
                    Instantiate(swingingChainPrefab, new Vector3(xPos, ceilingHeight - ceilingHalfHeight, 0), Quaternion.identity);
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(finishMushroomPrefab, new Vector3(xPos, groundTopY + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    break;

                default:
                    if (i != 1 && i != 2) 
                    {
                        SpawnTwoTrees(xPos);
                    }
                    break;
            }
        }
    }

    private void SpawnTwoTrees(float xPos)
    {
        float treeHalfHeight = GetHalfHeight(treePrefab);
        Instantiate(treePrefab, new Vector3(xPos - groundHalfWidth * 0.5f, groundTopY + treeHalfHeight, 0), Quaternion.identity);
        Instantiate(treePrefab, new Vector3(xPos + groundHalfWidth * 0.5f, groundTopY + treeHalfHeight, 0), Quaternion.identity);
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
