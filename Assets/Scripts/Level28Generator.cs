using UnityEngine;
using System.Collections.Generic;

public class Level28Generator : MonoBehaviour
{
    private static Level28Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject greenSmokePrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private GameObject spikeGroupPrefab;
    [SerializeField] private GameObject horizontalSawPrefab;
    [SerializeField] private GameObject specialPlatformPrefab;

    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 20;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;
    [SerializeField] private float ceilingHeight = 10f;
    [SerializeField] private float platformYOffset = 2f;

    [Header("Genişleme Ayarları")]
    [SerializeField] private float expansionTime = 60f;

    private float groundHalfWidth;
    private float groundHalfHeight;
    private float startXPosition;
    private float totalLevelWidth;
    private float gasExpansionTimer;
    private List<GameObject> activeGasList = new List<GameObject>();

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

    private void Update()
    {
        if (activeGasList.Count == 0) return;

        gasExpansionTimer += Time.deltaTime;
        float expansionPercentage = Mathf.Clamp01(gasExpansionTimer / expansionTime);
        
        float gasStartY = ceilingHeight - GetHalfHeight(ceilingPrefab);
        float gasEndY = groundHalfHeight * 2f;
        float currentGasY = Mathf.Lerp(gasStartY, gasEndY, expansionPercentage);

        foreach (GameObject gas in activeGasList)
        {
            if (gas != null)
            {
                gas.transform.position = new Vector3(gas.transform.position.x, currentGasY, 0);
            }
        }
    }

    private void SetupGasParticle(GameObject gas)
    {
        if (gas == null) return;
        
        ParticleSystem particleSystem = gas.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            var mainModule = particleSystem.main;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
            mainModule.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            mainModule.prewarm = true;
        }
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

        groundHalfWidth = GetHalfWidth(groundPrefab);
        groundHalfHeight = GetHalfHeight(groundPrefab);

        startXPosition = 0f;
        float currentXPosition = startXPosition;

        GenerateWalls(startXPosition, true);

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float groundXPosition = currentXPosition + groundHalfWidth;
            float topYPosition = groundHalfHeight;
            Instantiate(groundPrefab, new Vector3(groundXPosition, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(mushroomPrefab), 0), Quaternion.identity);
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    break;

                case 1:
                    if (platformPrefab != null)
                        Instantiate(platformPrefab, new Vector3(groundXPosition, topYPosition + platformYOffset, 0), Quaternion.identity);
                    break;

                case 4:
                    if (spikePrefab != null)
                        Instantiate(spikePrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(spikePrefab), 0), Quaternion.identity);
                    break;

                case 6:
                    if (sawPrefab != null)
                    {
                        float sawWidth = GetHalfWidth(sawPrefab) * 2f;
                        float sawHeight = GetHalfHeight(sawPrefab) * 2f;
                        float sawGap = 25f;

                        Instantiate(sawPrefab, new Vector3(groundXPosition, topYPosition, 0), Quaternion.identity);
                        
                        float secondSawX = groundXPosition + sawWidth + sawGap;
                        Instantiate(sawPrefab, new Vector3(secondSawX, topYPosition + (sawHeight * 1.5f), 0), Quaternion.identity);
                        
                        float thirdSawX = secondSawX + sawWidth + sawGap;
                        Instantiate(sawPrefab, new Vector3(thirdSawX, topYPosition, 0), Quaternion.identity);
                        
                        float fourthSawX = thirdSawX + sawWidth + sawGap;
                        Instantiate(sawPrefab, new Vector3(fourthSawX, topYPosition + (sawHeight * 1.5f), 0), Quaternion.identity);
                    }
                    break;

                case 9:
                    if (spikeGroupPrefab != null)
                    {
                        float spikeGroupWidth = GetHalfWidth(spikeGroupPrefab) * 2f;
                        float spikeGroupHeight = GetHalfHeight(spikeGroupPrefab);
                        
                        for (int j = 0; j < 3; j++) Instantiate(spikeGroupPrefab, new Vector3(groundXPosition + (j * spikeGroupWidth), topYPosition + spikeGroupHeight, 0), Quaternion.identity);
                        
                        float secondSetOffset = (spikeGroupWidth * 5f);
                        for (int j = 0; j < 3; j++) Instantiate(spikeGroupPrefab, new Vector3(groundXPosition + secondSetOffset + (j * spikeGroupWidth), topYPosition + spikeGroupHeight, 0), Quaternion.identity);
                        
                        float thirdSetOffset = (spikeGroupWidth * 10f);
                        for (int j = 0; j < 3; j++) Instantiate(spikeGroupPrefab, new Vector3(groundXPosition + thirdSetOffset + (j * spikeGroupWidth), topYPosition + spikeGroupHeight, 0), Quaternion.identity);
                    }
                    break;

                case 14:
                case 15:
                    if (horizontalSawPrefab != null)
                        Instantiate(horizontalSawPrefab, new Vector3(groundXPosition, topYPosition + 1.5f, 0), Quaternion.identity);
                    break;

                case 16:
                case 17:
                case 18:
                    if (specialPlatformPrefab != null)
                        Instantiate(specialPlatformPrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(specialPlatformPrefab), 0), Quaternion.identity);
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    if (finishMushroomPrefab != null)
                        Instantiate(finishMushroomPrefab, new Vector3(groundXPosition, topYPosition + GetHalfHeight(finishMushroomPrefab), 0), Quaternion.identity);
                    break;
            }
            currentXPosition += groundHalfWidth * 2f;
        }

        totalLevelWidth = totalWidthInBlocks * groundHalfWidth * 2f;

        if (ceilingPrefab != null)
        {
            float ceilingHalfWidth = GetHalfWidth(ceilingPrefab);
            int ceilingCount = Mathf.CeilToInt(totalLevelWidth / (ceilingHalfWidth * 2f));
            float gasBaseStartY = ceilingHeight - GetHalfHeight(ceilingPrefab);

            for (int i = 0; i < ceilingCount; i++)
            {
                float ceilingXPosition = startXPosition + (i * ceilingHalfWidth * 2f) + ceilingHalfWidth;
                Instantiate(ceilingPrefab, new Vector3(ceilingXPosition, ceilingHeight, 0), Quaternion.Euler(0, 0, 180));

                if (greenSmokePrefab != null)
                {
                    GameObject gas = Instantiate(greenSmokePrefab, new Vector3(ceilingXPosition, gasBaseStartY, 0), Quaternion.identity);
                    SetupGasParticle(gas);
                    activeGasList.Add(gas);
                }
            }
        }

        GenerateWalls(currentXPosition, false);
    }

    private void GenerateWalls(float xOrigin, bool isLeft)
    {
        if (wallPrefab == null) return;
        float wallHalfWidth = GetHalfWidth(wallPrefab);
        float wallHalfHeight = GetHalfHeight(wallPrefab);

        for (int col = 0; col < wallColumns; col++)
        {
            for (int row = 0; row < wallRows; row++)
            {
                float direction = isLeft ? -1 : 1;
                float xOffset = (col * 2 + 1) * wallHalfWidth;
                float xPosition = xOrigin + (direction * xOffset);
                float yPosition = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                Instantiate(wallPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            }
        }
    }
}
