using UnityEngine;

public class Level33Generator : MonoBehaviour
{
    private static Level33Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private GameObject dalPrefab;
    [SerializeField] private GameObject bigWoodPrefab;
    [SerializeField] private GameObject stickPrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject nativePrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 30;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float ceilingHeight = 6f;

    private float groundHalfHeight;
    private float wallHalfHeight;
    private float ceilingHalfHeight;
    private float playerHalfHeight;

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
        ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
        playerHalfHeight = GetHalfHeight(playerPrefab);
        
        float groundWidth = GetHalfWidth(groundPrefab) * 2f;
        float ceilingWidth = GetHalfWidth(ceilingPrefab) * 2f;

        GenerateLeftWalls(0);

        float endX = 0f;
        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundWidth) + (groundWidth / 2f);
            float ceilingXPos = (i * ceilingWidth) + (GetHalfWidth(ceilingPrefab));
            endX = (i + 1) * groundWidth;

            switch (i)
            {
                case 0:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (playerPrefab != null)
                    {
                        Instantiate(playerPrefab, new Vector3(xPos, groundHalfHeight + playerHalfHeight, 0), Quaternion.identity);
                    }
                    if (mushroomPrefab != null)
                    {
                        Instantiate(mushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    }
                    break;
                case 2:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    float sawH = GetHalfHeight(sawPrefab);
                    if (sawPrefab != null)
                    {
                        Instantiate(sawPrefab, new Vector3(xPos, groundHalfHeight, 0), Quaternion.identity);
                    }

                    if (woodPrefab != null)
                    {
                        float woodH = GetHalfHeight(woodPrefab);
                        float sawTotalH = sawH * 2f;
                        float woodY = groundHalfHeight + (sawTotalH * 1f) + woodH;
                        Instantiate(woodPrefab, new Vector3(xPos, woodY, 0), Quaternion.identity);

                        if (dalPrefab != null)
                        {
                            float woodW = GetHalfWidth(woodPrefab);
                            float dalW = GetHalfWidth(dalPrefab);
                            float dalH = GetHalfHeight(dalPrefab);
                            float dalY = woodY - woodH + dalW; // Shifted up so half doesn't stay below
                            float dalX = xPos - woodW - dalH;
                            Instantiate(dalPrefab, new Vector3(dalX, dalY, 0), Quaternion.Euler(0, 0, 90));
                        }
                    }
                    break;
                case 4:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (bigWoodPrefab != null)
                    {
                        Instantiate(bigWoodPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(bigWoodPrefab), 0), Quaternion.identity);

                        if (trampolinePrefab != null)
                        {
                            float bigWoodW = GetHalfWidth(bigWoodPrefab);
                            float trampW = GetHalfWidth(trampolinePrefab);
                            float trampH = GetHalfHeight(trampolinePrefab);
                            float trampX = xPos - bigWoodW - trampW;
                            float trampY = groundHalfHeight + trampH;
                            Instantiate(trampolinePrefab, new Vector3(trampX, trampY, 0), Quaternion.identity);
                        }
                    }
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                    if (i == 6 || i == 9 && nativePrefab != null)
                    {
                        Instantiate(nativePrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(nativePrefab), 0), Quaternion.identity);
                    }

                    if (stickPrefab != null)
                    {
                        // Odunun tepe hizasına denk gelmesi için yüksekliği tam katıyoruz
                        float bigWoodTop = groundHalfHeight + (GetHalfHeight(bigWoodPrefab) * 2f);
                        float stickY = bigWoodTop; 
                        Instantiate(stickPrefab, new Vector3(xPos, stickY, 0), Quaternion.identity);
                    }
                    break;
                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    if (finishMushroomPrefab != null)
                    {
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    }
                    break;

                default:
                    Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
                    if (ceilingPrefab != null)
                        Instantiate(ceilingPrefab, new Vector3(ceilingXPos, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
                    break;
            }
        }
        GenerateRightWalls(endX);
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
                float yPos = (row * wallHH * 2) + wallHH;
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
                float yPos = (row * wallHH * 2) + wallHH;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }
}
