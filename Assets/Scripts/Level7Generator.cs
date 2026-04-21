using UnityEngine;

public class Level7Generator : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 10;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject snowmanPrefab;
    [SerializeField] private GameObject boxPrefab; 
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject flyingSpikePrefab;
    [SerializeField] private GameObject slidingSpikePrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || wallPrefab == null || playerPrefab == null || 
            startIglooPrefab == null || finishIglooPrefab == null || housePrefab == null || 
            snowmanPrefab == null || boxPrefab == null || spikePrefab == null || 
            flyingSpikePrefab == null || slidingSpikePrefab == null)
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
        float groundHalfWidth = GetHalfWidth(groundPrefab);
        float groundHalfHeight = GetHalfHeight(groundPrefab);
        float topY = groundHalfHeight;

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    GenerateWalls(0, true);
                    float sIHalf = GetHalfHeight(startIglooPrefab);
                    Instantiate(startIglooPrefab, new Vector3(xPos, topY + sIHalf, 0), Quaternion.Euler(0, 180, 0));
                    float pHalf = GetHalfHeight(playerPrefab);
                    float pX = xPos + GetHalfWidth(startIglooPrefab) + GetHalfWidth(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(pX, topY + pHalf, 0), Quaternion.identity);
                    break;

                case 2:
                    float bHalfW = GetHalfWidth(boxPrefab);
                    float bHalfH = GetHalfHeight(boxPrefab);
                    float currentObstacleX = xPos - groundHalfWidth;
                    int spikeCounter = 0;
                    Instantiate(boxPrefab, new Vector3(currentObstacleX + bHalfW, topY + bHalfH, 0), Quaternion.identity);
                    currentObstacleX += bHalfW * 2;
                    for (int p = 0; p < 3; p++)
                    {
                        for (int d = 0; d < 3; d++)
                        {
                            spikeCounter++;
                            GameObject selectedSpike = (spikeCounter == 2 || spikeCounter == 6 || spikeCounter == 7) ? flyingSpikePrefab : spikePrefab;
                            float sHalfW = GetHalfWidth(selectedSpike);
                            float sHalfH = GetHalfHeight(selectedSpike);
                            Instantiate(selectedSpike, new Vector3(currentObstacleX + sHalfW, topY + sHalfH, 0), Quaternion.identity);
                            currentObstacleX += sHalfW * 2;
                        }
                        Instantiate(boxPrefab, new Vector3(currentObstacleX + bHalfW, topY + bHalfH, 0), Quaternion.identity);
                        currentObstacleX += bHalfW * 2;
                    }
                    break;

                case 6:
                    float snHalf = GetHalfHeight(snowmanPrefab);
                    Instantiate(snowmanPrefab, new Vector3(xPos, topY + snHalf, 0), Quaternion.identity);
                    break;

                case 7:
                    float hHalf = GetHalfHeight(housePrefab);
                    Instantiate(housePrefab, new Vector3(xPos, topY + hHalf, 0), Quaternion.identity);
                    break;

                case 8:
                case 10:
                    float slHalf = GetHalfHeight(slidingSpikePrefab);
                    Instantiate(slidingSpikePrefab, new Vector3(xPos, topY + slHalf, 0), Quaternion.identity);
                    break;

                case int n when (n == groundCount - 1):
                    float fIHalf = GetHalfHeight(finishIglooPrefab);
                    Instantiate(finishIglooPrefab, new Vector3(xPos, topY + fIHalf, 0), Quaternion.identity);
                    GenerateWalls(groundCount * groundHalfWidth * 2, false);
                    break;
            }
        }
    }

    private void GenerateWalls(float xOrigin, bool isLeft)
    {
        float wHalfW = GetHalfWidth(wallPrefab);
        float wHalfH = GetHalfHeight(wallPrefab);
        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float px = isLeft ? xOrigin - (col * wHalfW * 2) - wHalfW : xOrigin + (col * wHalfW * 2) + wHalfW;
                float py = wallYOffset + (row * wHalfH * 2) + wHalfH;
                Instantiate(wallPrefab, new Vector3(px, py, 0), Quaternion.identity);
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
