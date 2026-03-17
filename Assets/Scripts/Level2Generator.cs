using UnityEngine;

public class Level2Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject trapGroundPrefab;
    [SerializeField] private int groundCount = 15;

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Oyuncu Ayarları")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Özel Objeler")]
    [SerializeField] private GameObject iglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject midCeilingSpikePrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || trapGroundPrefab == null || wallPrefab == null || 
            ceilingPrefab == null || iglooPrefab == null || playerPrefab == null || 
            finishIglooPrefab == null || groundSpikePrefab == null || midCeilingSpikePrefab == null)
        {
            Debug.LogError("DİKKAT: Level2Generator içinde eksik prefab var! Tüm prefabları atadığınızdan emin olun.");
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
            
            if (i == groundCount - 2)
            {
                Instantiate(trapGroundPrefab, groundPos, Quaternion.identity);
            }
            else
            {
                Instantiate(groundPrefab, groundPos, Quaternion.identity);
            }

            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            if (i == 0)
            {
                float objHalfHeight = GetHalfHeight(iglooPrefab);
                Instantiate(iglooPrefab, new Vector3(xPos, topY + objHalfHeight, 0), Quaternion.Euler(0, 180, 0));
                
                float pobjHalfHeight = GetHalfHeight(playerPrefab);
                float playerXPos = xPos + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerXPos, topY + pobjHalfHeight, 0), Quaternion.identity);
            }

            if (i >= 2 && i < groundCount - 3)
            {
                float spikeHalfHeight = GetHalfHeight(groundSpikePrefab);
                Instantiate(groundSpikePrefab, new Vector3(xPos, topY + spikeHalfHeight, 0), Quaternion.identity);
            }

            if (i >= 2 && i < groundCount - 3)
            {
                float borderX = (i + 1) * groundHalfWidth * 2;
                float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
                float spikeHalfHeight = GetHalfHeight(midCeilingSpikePrefab);
                float spawnY = ceilingYOffset - ceilingHalfHeight - spikeHalfHeight;
                Instantiate(midCeilingSpikePrefab, new Vector3(borderX, spawnY, 0), Quaternion.identity);
            }

            if (i == groundCount - 1)
            {
                float objHalfHeight = GetHalfHeight(finishIglooPrefab);
                Instantiate(finishIglooPrefab, new Vector3(xPos, topY + objHalfHeight, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateWalls()
    {
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);
        float totalGroundWidth = groundCount * GetHalfWidth(groundPrefab) * 2;

        // Sol Duvar
        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xPos = -(col * wallHalfWidth * 2) - wallHalfWidth;
                float yPos = wallYOffset + (row * wallHalfHeight * 2) + wallHalfHeight;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }

        // Sağ Duvar
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
