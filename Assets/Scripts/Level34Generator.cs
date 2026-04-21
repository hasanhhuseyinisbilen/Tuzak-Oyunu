using UnityEngine;

public class Level34Generator : MonoBehaviour
{
    private static Level34Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private GameObject hidingSpikePrefab;
    [SerializeField] private GameObject polePrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject slidingSpikedGroundPrefab;
    [SerializeField] private GameObject triggerPrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 30;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;

    private float groundHalfHeight;
    private float wallHalfHeight;
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
        playerHalfHeight = GetHalfHeight(playerPrefab);
        
        float groundWidth = GetHalfWidth(groundPrefab) * 2f;
        float poleHH = GetHalfHeight(polePrefab);
        float verticalStep = groundHalfHeight * 0.75f;
        float highGroundY = (groundHalfHeight + (verticalStep * 10f) + poleHH + poleHH) - groundHalfHeight;

        GenerateLeftWalls(0);

        float endX = 0f;
        float lastGroundY = 0f;
        SlidingTrapTrigger pendingTrigger = null;
        Vector3 case8TargetPos = Vector3.zero;

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundWidth) + (groundWidth / 2f);
            endX = (i + 1) * groundWidth;

            float currentGroundY = (i >= 7) ? highGroundY : 0f;
            lastGroundY = currentGroundY;

            switch (i)
            {
                case 0:
                    Instantiate(groundPrefab, new Vector3(xPos, currentGroundY, 0), Quaternion.identity);

                    if (mushroomPrefab != null)
                    {
                        Instantiate(mushroomPrefab, new Vector3(xPos, currentGroundY + groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    }

                    if (playerPrefab != null)
                    {
                        Instantiate(playerPrefab, new Vector3(xPos, currentGroundY + groundHalfHeight + playerHalfHeight, 0), Quaternion.identity);
                    }
                    break;

                case 1:
                    Instantiate(groundPrefab, new Vector3(xPos, currentGroundY, 0), Quaternion.identity);

                    if (sawPrefab != null)
                    {
                        Instantiate(sawPrefab, new Vector3(xPos, currentGroundY + groundHalfHeight, 0), Quaternion.identity);
                    }
                    break;

                case 2:
                    Instantiate(groundPrefab, new Vector3(xPos, currentGroundY, 0), Quaternion.identity);

                    if (hidingSpikePrefab != null)
                    {
                        Instantiate(hidingSpikePrefab, new Vector3(xPos, currentGroundY + groundHalfHeight + GetHalfHeight(hidingSpikePrefab), 0), Quaternion.identity);
                    }
                    break;

                case 4:
                    if (trampolinePrefab != null)
                    {
                        float trampHalfH = GetHalfHeight(trampolinePrefab);
                        float trampX = xPos - (groundWidth / 2f);
                        Instantiate(trampolinePrefab, new Vector3(trampX, groundHalfHeight + trampHalfH, 0), Quaternion.identity);
                    }

                    if (polePrefab != null)
                    {
                        float poleXOffset = groundWidth / 4f;
                        for (int m = 1; m <= 9; m += 2)
                        {
                            float currentXOffset = ((m - 1) / 2) % 2 == 0 ? -poleXOffset : poleXOffset;
                            float y = groundHalfHeight + (verticalStep * (float)m) + poleHH;
                            Instantiate(polePrefab, new Vector3(xPos + currentXOffset, y, 0), Quaternion.identity);
                        }
                    }
                    break;

                case 5:
                    if (polePrefab != null)
                    {
                        float poleXOffset = groundWidth / 4f;
                        for (int m = 2; m <= 10; m += 2)
                        {
                            // m=2: (0)%2 == 0 -> Left
                            // m=4: (1)%2 == 1 -> Right
                            float currentXOffset = ((m / 2) - 1) % 2 == 0 ? -poleXOffset : poleXOffset;
                            float y = groundHalfHeight + (verticalStep * (float)m) + poleHH;
                            Instantiate(polePrefab, new Vector3(xPos + currentXOffset, y, 0), Quaternion.identity);
                        }
                    }
                    break;

                case 6:
                    break;

                case 7:
                    Instantiate(groundPrefab, new Vector3(xPos, currentGroundY, 0), Quaternion.identity);
                    if (triggerPrefab != null)
                    {
                        GameObject triggerObj = Instantiate(triggerPrefab, new Vector3(xPos, currentGroundY + groundHalfHeight + 1f, 0), Quaternion.identity);
                        pendingTrigger = triggerObj.GetComponent<SlidingTrapTrigger>();
                        if (pendingTrigger == null) pendingTrigger = triggerObj.AddComponent<SlidingTrapTrigger>();
                    }
                    break;

                case 8:
                    // Hedefi zeminin tam üstüne (bir blok boyu yukarı) koyuyoruz
                    case8TargetPos = new Vector3(xPos, currentGroundY + (groundHalfHeight * 2f), 0);
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(groundPrefab, new Vector3(xPos, currentGroundY, 0), Quaternion.identity);
                    if (finishMushroomPrefab != null)
                    {
                        Instantiate(finishMushroomPrefab, new Vector3(xPos, currentGroundY + groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    }

                    if (slidingSpikedGroundPrefab != null)
                    {
                        float spawnY = currentGroundY + (groundHalfHeight * 2f);
                        GameObject trapObj = Instantiate(slidingSpikedGroundPrefab, new Vector3(xPos, spawnY, 0), Quaternion.Euler(0, 0, 90));
                        
                        TargetedSlidingTrap trap = trapObj.GetComponent<TargetedSlidingTrap>();
                        if (trap == null) trap = trapObj.AddComponent<TargetedSlidingTrap>();
                        
                        trap.Setup(case8TargetPos, Quaternion.identity, true);

                        if (pendingTrigger != null)
                        {
                            pendingTrigger.Setup(trap);
                        }
                    }
                    break;

                default:
                    Instantiate(groundPrefab, new Vector3(xPos, currentGroundY, 0), Quaternion.identity);
                    break;
            }
        }
        GenerateRightWalls(endX, lastGroundY);
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

    private void GenerateRightWalls(float xOrigin, float yOffset)
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
                float yPos = yOffset + (row * wallHH * 2) + wallHH;
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }
        }
    }
}
