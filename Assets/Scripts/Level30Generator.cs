using UnityEngine;
using System.Collections.Generic;

public class Level30Generator : MonoBehaviour
{
    private static Level30Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private GameObject specialSawPrefab;
    [SerializeField] private GameObject stopButtonPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject normalSpikePrefab;
    [SerializeField] private GameObject flySpikePrefab;
    [SerializeField] private GameObject specialSpikePrefab;
    [SerializeField] private GameObject ceilingSpikePrefab;
    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject playerPrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 20;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private float ceilingHeight = 6f;
    [SerializeField] private float platformSpawnHeight = 3f;

    private float groundHalfHeight;
    private float wallHalfHeight;
    private float ceilingHalfHeight;
    private float platformHalfHeight;
    private float playerHalfHeight;
    private float sawHalfWidth;
    private float specialSawHalfWidth;
    private float spikeHalfWidth;
    private float columnHalfWidth;

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
        Renderer renderer = prefab.GetComponentInChildren<Renderer>();
        return (renderer != null) ? renderer.bounds.size.x / 2f : 0.5f;
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        Renderer renderer = prefab.GetComponentInChildren<Renderer>();
        return (renderer != null) ? renderer.bounds.size.y / 2f : 0.5f;
    }

    private void GenerateLevel()
    {
        if (groundPrefab == null) return;
        
        groundHalfHeight = GetHalfHeight(groundPrefab);
        wallHalfHeight = GetHalfHeight(wallPrefab);
        ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
        platformHalfHeight = GetHalfHeight(platformPrefab);
        playerHalfHeight = GetHalfHeight(playerPrefab);
        sawHalfWidth = GetHalfWidth(sawPrefab);
        specialSawHalfWidth = GetHalfWidth(specialSawPrefab);
        spikeHalfWidth = GetHalfWidth(spikePrefab);
        columnHalfWidth = GetHalfWidth(columnPrefab);
        
        float groundWidth = GetHalfWidth(groundPrefab) * 2f;
        float ceilingWidth = GetHalfWidth(ceilingPrefab) * 2f;

        GenerateLeftWalls(0);

        float totalLevelEndX = 0f;
        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float currentXPosition = (i * groundWidth) + (groundWidth / 2f);
            float currentCeilingXPosition = (i * ceilingWidth) + (GetHalfWidth(ceilingPrefab));
            totalLevelEndX = (i + 1) * groundWidth;

            if (ceilingPrefab != null)
            {
                Instantiate(ceilingPrefab, new Vector3(currentCeilingXPosition, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));
            }

            switch (i)
            {
                case 0:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    if (playerPrefab != null)
                    {
                        Instantiate(playerPrefab, new Vector3(currentXPosition, groundHalfHeight + playerHalfHeight, 0), Quaternion.identity);
                    }
                    if (mushroomPrefab != null)
                    {
                        Instantiate(mushroomPrefab, new Vector3(currentXPosition, groundHalfHeight + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    }
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    if (finishMushroomPrefab != null)
                    {
                        Instantiate(finishMushroomPrefab, new Vector3(currentXPosition, groundHalfHeight + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    }
                    break;

                case 1:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    if (platformPrefab != null)
                    {
                        Instantiate(platformPrefab, new Vector3(currentXPosition, groundHalfHeight + platformHalfHeight, 0), Quaternion.identity);
                    }
                    if (stopButtonPrefab != null)
                    {
                        Instantiate(stopButtonPrefab, new Vector3(currentXPosition + 1.5f, groundHalfHeight + GetHalfHeight(stopButtonPrefab), 0), Quaternion.identity);
                    }
                    break;

                case 2:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    float middleY = ceilingHeight / 2f;
                    for (int j = 0; j < 5; j++)
                    {
                        GameObject selectedSawPrefab = (j == 4) ? specialSawPrefab : sawPrefab;
                        if (selectedSawPrefab != null)
                        {
                            float currentHalfWidth = GetHalfWidth(selectedSawPrefab);
                            float startEdgePosition = currentXPosition - (groundWidth / 2f);
                            float sawXPosition = startEdgePosition + currentHalfWidth + (j * currentHalfWidth * 4f);
                            Instantiate(selectedSawPrefab, new Vector3(sawXPosition, middleY, 0), Quaternion.identity);
                        }
                    }
                    break;

                case 4:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    if (platformPrefab != null)
                    {
                        Instantiate(platformPrefab, new Vector3(currentXPosition, groundHalfHeight + platformHalfHeight, 0), Quaternion.identity);
                    }
                    break;

                case 5:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    if (spikePrefab != null)
                    {
                        float spikeWidth = spikeHalfWidth * 2f;
                        float startEdgePosition = currentXPosition - (groundWidth / 2f);
                        for (int j = 0; j < 7; j++)
                        {
                            float spikeXPosition = startEdgePosition + spikeHalfWidth + (j * spikeWidth);
                            Instantiate(spikePrefab, new Vector3(spikeXPosition, groundHalfHeight + GetHalfHeight(spikePrefab), 0), Quaternion.identity);
                        }
                    }
                    break;

                case 7:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    if (platformPrefab != null)
                    {
                        Instantiate(platformPrefab, new Vector3(currentXPosition, groundHalfHeight + platformHalfHeight, 0), Quaternion.identity);
                    }
                    if (ceilingSpikePrefab != null)
                    {
                        float ceilingBaseX = currentCeilingXPosition - (ceilingWidth / 2f);
                        float spikeXPosition = ceilingBaseX + GetHalfWidth(ceilingSpikePrefab);
                        float spikeYPosition = ceilingHeight - ceilingHalfHeight - GetHalfHeight(ceilingSpikePrefab);
                        Instantiate(ceilingSpikePrefab, new Vector3(spikeXPosition, spikeYPosition, 0), Quaternion.identity);
                    }
                    break;

                case 8:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    float currentPlacementX = currentXPosition - (groundWidth / 2f);
                    if (columnPrefab != null)
                    {
                        Instantiate(columnPrefab, new Vector3(currentPlacementX + columnHalfWidth, groundHalfHeight + GetHalfHeight(columnPrefab), 0), Quaternion.identity);
                        currentPlacementX += columnHalfWidth * 2f;
                    }
                    if (normalSpikePrefab != null)
                    {
                        float normalSpikeHalfWidth = GetHalfWidth(normalSpikePrefab);
                        for (int j = 0; j < 2; j++)
                        {
                            Instantiate(normalSpikePrefab, new Vector3(currentPlacementX + normalSpikeHalfWidth, groundHalfHeight + GetHalfHeight(normalSpikePrefab), 0), Quaternion.identity);
                            currentPlacementX += normalSpikeHalfWidth * 2f;
                        }
                    }
                    if (columnPrefab != null)
                    {
                        Instantiate(columnPrefab, new Vector3(currentPlacementX + columnHalfWidth, groundHalfHeight + GetHalfHeight(columnPrefab), 0), Quaternion.identity);
                        currentPlacementX += columnHalfWidth * 2f;
                    }
                    if (flySpikePrefab != null)
                    {
                        float flySpikeHalfWidth = GetHalfWidth(flySpikePrefab);
                        Instantiate(flySpikePrefab, new Vector3(currentPlacementX + flySpikeHalfWidth, groundHalfHeight + GetHalfHeight(flySpikePrefab), 0), Quaternion.identity);
                        currentPlacementX += flySpikeHalfWidth * 2f;
                    }
                    if (normalSpikePrefab != null)
                    {
                        float normalSpikeHalfWidth = GetHalfWidth(normalSpikePrefab);
                        Instantiate(normalSpikePrefab, new Vector3(currentPlacementX + normalSpikeHalfWidth, groundHalfHeight + GetHalfHeight(normalSpikePrefab), 0), Quaternion.identity);
                        currentPlacementX += normalSpikeHalfWidth * 2f;
                    }
                    break;

                case 9:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    if (arrowPrefab != null)
                    {
                        Instantiate(arrowPrefab, new Vector3(currentXPosition, (groundHalfHeight + playerHalfHeight) * 1.5f, 0), Quaternion.Euler(0, 0, 90));
                    }
                    break;

                default:
                    Instantiate(groundPrefab, new Vector3(currentXPosition, 0, 0), Quaternion.identity);
                    break;
            }
        }
        GenerateRightWalls(totalLevelEndX);
    }

    private void GenerateLeftWalls(float xOrigin)
    {
        if (wallPrefab == null) return;
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xOffset = (col * 2 + 1) * wallHalfWidth;
                float xPosition = xOrigin - xOffset; 
                float yPosition = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                Instantiate(wallPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateRightWalls(float xOrigin)
    {
        if (wallPrefab == null) return;
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float xOffset = (col * 2 + 1) * wallHalfWidth;
                float xPosition = xOrigin + xOffset; 
                float yPosition = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                Instantiate(wallPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            }
        }
    }
}
