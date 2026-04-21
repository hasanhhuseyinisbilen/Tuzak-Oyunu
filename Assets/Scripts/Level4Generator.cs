using UnityEngine;

public class Level4Generator : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int groundCount = 15;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float ceilingYOffset = 5f;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 6;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject iglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject specialSpikePrefab;
    [SerializeField] private GameObject ground7SpikePrefab;
    [SerializeField] private GameObject pineTreePrefab;
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject swingingChainPrefab;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null || iglooPrefab == null || finishIglooPrefab == null || 
            boxPrefab == null || spikePrefab == null || specialSpikePrefab == null || 
            ground7SpikePrefab == null || pineTreePrefab == null)
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
        GenerateWalls(0, true);

        float groundHalfWidth = GetHalfWidth(groundPrefab);
        float groundHalfHeight = GetHalfHeight(groundPrefab);
        float topY = groundHalfHeight;

        for (int i = 0; i < groundCount; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset, 0), Quaternion.identity);

            if (pineTreePrefab != null)
            {
                float treeHalfHeight = GetHalfHeight(pineTreePrefab);
                float leftTreeX = xPos - (groundHalfWidth * 0.5f);
                float rightTreeX = xPos + (groundHalfWidth * 0.5f);
                Instantiate(pineTreePrefab, new Vector3(leftTreeX, topY + treeHalfHeight, 0), Quaternion.identity);
                Instantiate(pineTreePrefab, new Vector3(rightTreeX, topY + treeHalfHeight, 0), Quaternion.identity);
            }

            switch (i)
            {
                case 0:
                    float igHalf = GetHalfHeight(iglooPrefab);
                    Instantiate(iglooPrefab, new Vector3(xPos, topY + igHalf, 0), Quaternion.Euler(0, 180, 0));
                    float pHalf = GetHalfHeight(playerPrefab);
                    float pX = xPos + GetHalfWidth(iglooPrefab) + GetHalfWidth(playerPrefab);
                    Instantiate(playerPrefab, new Vector3(pX, topY + pHalf, 0), Quaternion.identity);
                    break;

                case 1:
                    if (housePrefab != null)
                    {
                        float houseHalfHeight = GetHalfHeight(housePrefab);
                        Instantiate(housePrefab, new Vector3(xPos, topY + houseHalfHeight, 0), Quaternion.identity);
                    }
                    break;

                case 3:
                    float currentObstacleX = i * groundHalfWidth * 2;
                    for (int pattern = 0; pattern < 3; pattern++)
                    {
                        float bHalfW = GetHalfWidth(boxPrefab);
                        float bHalfH = GetHalfHeight(boxPrefab);
                        Instantiate(boxPrefab, new Vector3(currentObstacleX + bHalfW, topY + bHalfH, 0), Quaternion.identity);
                        currentObstacleX += bHalfW * 2;
                        for (int s = 0; s < 2; s++)
                        {
                            GameObject currentSpike = (pattern == 2) ? specialSpikePrefab : spikePrefab;
                            float sHalfW = GetHalfWidth(currentSpike);
                            float sHalfH = GetHalfHeight(currentSpike);
                            Instantiate(currentSpike, new Vector3(currentObstacleX + sHalfW, topY + sHalfH, 0), Quaternion.identity);
                            currentObstacleX += sHalfW * 2;
                        }
                    }
                    float fBHalfW = GetHalfWidth(boxPrefab);
                    float fBHalfH = GetHalfHeight(boxPrefab);
                    Instantiate(boxPrefab, new Vector3(currentObstacleX + fBHalfW, topY + fBHalfH, 0), Quaternion.identity);
                    break;

                case 10:
                    float g7SHalf = GetHalfHeight(ground7SpikePrefab);
                    Instantiate(ground7SpikePrefab, new Vector3(xPos, topY + g7SHalf, 0), Quaternion.identity);
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
