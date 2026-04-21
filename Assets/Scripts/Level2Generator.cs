using UnityEngine;

public class Level2Generator : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject trapGroundPrefab;
    [SerializeField] private int groundCount = 15;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject midCeilingSpikePrefab;
    [SerializeField] private GameObject treePrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || trapGroundPrefab == null || wallPrefab == null || 
            ceilingPrefab == null || iglooPrefab == null || playerPrefab == null || 
            finishIglooPrefab == null || groundSpikePrefab == null || midCeilingSpikePrefab == null ||
            treePrefab == null)
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
        GenerateWalls();

        float groundHalfHeight = GetHalfHeight(groundPrefab);
        float groundHalfWidth = GetHalfWidth(groundPrefab);
        float topY = groundHalfHeight;

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            Vector3 groundPos = new Vector3(xPos, 0, 0);

            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            if (i == groundCount - 2)
                Instantiate(trapGroundPrefab, groundPos, Quaternion.identity);
            else
                Instantiate(groundPrefab, groundPos, Quaternion.identity);

            switch (i)
            {
                case 0:
                    float objHalfHeight = GetHalfHeight(iglooPrefab);
                    Instantiate(iglooPrefab, new Vector3(xPos, topY + objHalfHeight, 0), Quaternion.Euler(0, 180, 0));
                    float pobjHalfHeight = GetHalfHeight(playerPrefab);
                    float playerXPos = xPos + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(playerXPos, topY + pobjHalfHeight, 0), Quaternion.identity);
                    break;

                case 1:
                    float treeHeight = GetHalfHeight(treePrefab);
                    float treeOffset = groundHalfWidth / 2f;
                    Instantiate(treePrefab, new Vector3(xPos - treeOffset, topY + treeHeight, 0), Quaternion.identity);
                    Instantiate(treePrefab, new Vector3(xPos + treeOffset, topY + treeHeight, 0), Quaternion.identity);
                    break;

                case int n when (n >= 2 && n < groundCount - 3):
                    float spikeHalfHeight = GetHalfHeight(groundSpikePrefab);
                    Instantiate(groundSpikePrefab, new Vector3(xPos, topY + spikeHalfHeight, 0), Quaternion.identity);
                    
                    float borderX = (i + 1) * groundHalfWidth * 2;
                    float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
                    float mSpikeHalfHeight = GetHalfHeight(midCeilingSpikePrefab);
                    float spawnY = ceilingYOffset - ceilingHalfHeight - mSpikeHalfHeight;
                    Instantiate(midCeilingSpikePrefab, new Vector3(borderX, spawnY, 0), Quaternion.identity);
                    break;

                case int n when (n == groundCount - 1):
                    float finishHalfHeight = GetHalfHeight(finishIglooPrefab);
                    Instantiate(finishIglooPrefab, new Vector3(xPos, topY + finishHalfHeight, 0), Quaternion.identity);
                    break;
            }
        }
    }

    private void GenerateWalls()
    {
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);
        float totalGroundWidth = groundCount * GetHalfWidth(groundPrefab) * 2;

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xPosL = -(col * wallHalfWidth * 2) - wallHalfWidth;
                float yPosL = wallYOffset + (row * wallHalfHeight * 2) + wallHalfHeight;
                Instantiate(wallPrefab, new Vector3(xPosL, yPosL, 0), Quaternion.identity);

                float xPosR = totalGroundWidth + (col * wallHalfWidth * 2) + wallHalfWidth;
                float yPosR = wallYOffset + (row * wallHalfHeight * 2) + wallHalfHeight;
                Instantiate(wallPrefab, new Vector3(xPosR, yPosR, 0), Quaternion.identity);
            }
        }
    }

    private float GetHalfHeight(GameObject prefab)
    {
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null) ? r.bounds.size.y / 2f : 0.5f;
    }

    private float GetHalfWidth(GameObject prefab)
    {
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null) ? r.bounds.size.x / 2f : 0.5f;
    }
}
