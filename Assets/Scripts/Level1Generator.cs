using UnityEngine;

public class Level1Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 10;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;

    [Header("Özel Objeler")]
    [SerializeField] private GameObject iglooPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject finishPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject treePrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || ceilingPrefab == null || 
            iglooPrefab == null || playerPrefab == null || finishPrefab == null || 
            spikePrefab == null || groundSpikePrefab == null || treePrefab == null)
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

        float topY = GetHalfHeight(groundPrefab);
        float halfWidth = GetHalfWidth(groundPrefab);

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * halfWidth * 2) + halfWidth;
            Vector3 groundPos = new Vector3(xPos, 0, 0);
            Instantiate(groundPrefab, groundPos, Quaternion.identity);

            GameObject ceiling = Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);
            
            switch (i)
            {
                case 0:
                    float iglooHalfHeight = GetHalfHeight(iglooPrefab);
                    Instantiate(iglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));
                    
                    float playerHalfHeight = GetHalfHeight(playerPrefab);
                    float playerXPos = xPos + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(playerXPos, topY + playerHalfHeight, 0), Quaternion.identity);
                    break;

                case 1:
                    float ceilingHalfHeight = GetHalfHeight(ceiling);
                    float spikeHalfHeight = GetHalfHeight(spikePrefab);
                    Vector3 spikePos = new Vector3(xPos, ceilingYOffset - (ceilingHalfHeight + spikeHalfHeight), 0);
                    Instantiate(spikePrefab, spikePos, Quaternion.identity);
                    break;

                case 4:
                    float treeHalfHeight = GetHalfHeight(treePrefab);
                    Instantiate(treePrefab, new Vector3(xPos, topY + treeHalfHeight, 0), Quaternion.identity);
                    break;

                case 5:
                    float gSpikeHalfHeight = GetHalfHeight(groundSpikePrefab);
                    Instantiate(groundSpikePrefab, new Vector3(xPos, topY + gSpikeHalfHeight, 0), Quaternion.identity);
                    break;

                default:
                    if (i == groundCount - 1)
                    {
                        float finishHalfHeight = GetHalfHeight(finishPrefab);
                        Instantiate(finishPrefab, new Vector3(xPos, topY + finishHalfHeight, 0), Quaternion.identity);
                    }
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
                float xPos = -(col * wallHalfWidth * 2) - wallHalfWidth;
                float yPos = wallYOffset + (row * wallHalfHeight * 2) + wallHalfHeight;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xPos = totalGroundWidth + (col * wallHalfWidth * 2) + wallHalfWidth;
                float yPos = wallYOffset + (row * wallHalfHeight * 2) + wallHalfHeight;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
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
