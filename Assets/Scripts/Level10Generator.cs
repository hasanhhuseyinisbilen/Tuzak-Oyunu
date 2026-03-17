using UnityEngine;

public class Level10Generator : MonoBehaviour
{
    [Header("Zemin Ayarları")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject trapGroundPrefab;
    [SerializeField] private int groundCount = 12;
    [SerializeField] private float trapGroundY = 2f; // Tuzak zeminlerin yüksekliği

    [Header("Tavan Ayarları")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;

    [Header("Duvar Ayarları")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;

    [Header("Obje Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject trapTrampolinePrefab; // Yeni: Tuzaklı Trambolin
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || trapGroundPrefab == null || ceilingPrefab == null || 
            wallPrefab == null || playerPrefab == null || trampolinePrefab == null || 
            trapTrampolinePrefab == null || startIglooPrefab == null || finishIglooPrefab == null)
        {
            Debug.LogError("DİKKAT: Level10Generator içinde eksik prefab var!");
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

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            // Örüntü Mantığı:
            // 0: Normal
            // 2, 4, 6, 8: Tuzaklı (Custom Y)
            // Diğerleri (1, 3, 5, 7, 9): Trambolinli (Normal)
            // 10, 11: Normal Zemin (Kullanıcı İsteği)
            
            if (i > 0 && i % 2 == 0 && i <= 8)
            {
                // Tuzaklı Zemin
                Instantiate(trapGroundPrefab, new Vector3(xPos, trapGroundY, 0), Quaternion.identity);
            }
            else
            {
                // Normal Zemin
                Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

                // Tek sayılarda trambolin koy (1, 3, 5, 7 normal | 9 trap)
                if (i % 2 != 0 && i <= 9)
                {
                    GameObject trampToSpawn = (i == 9) ? trapTrampolinePrefab : trampolinePrefab;
                    float trampHalfHeight = GetHalfHeight(trampToSpawn);
                    Instantiate(trampToSpawn, new Vector3(xPos, topY + trampHalfHeight, 0), Quaternion.identity);
                }
            }

            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            if (i == 0)
            {
                GenerateWalls(0, true);

                float iglooHalfHeight = GetHalfHeight(startIglooPrefab);
                Instantiate(startIglooPrefab, new Vector3(xPos, topY + iglooHalfHeight, 0), Quaternion.Euler(0, 180, 0));

                float playerHalfHeight = GetHalfHeight(playerPrefab);
                float playerXPos = xPos + GetHalfWidth(startIglooPrefab) + GetHalfWidth(playerPrefab);
                Instantiate(playerPrefab, new Vector3(playerXPos, topY + playerHalfHeight, 0), Quaternion.identity);
            }

            if (i == groundCount - 1)
            {
                float iglooHalfHeight = GetHalfHeight(finishIglooPrefab);
                float currentTopY = (i > 0 && i % 2 == 0 && i <= 8) ? trapGroundY + GetHalfHeight(trapGroundPrefab) : topY;
                Instantiate(finishIglooPrefab, new Vector3(xPos, currentTopY + iglooHalfHeight, 0), Quaternion.identity);
                GenerateWalls(groundCount * groundHalfWidth * 2, false);
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
