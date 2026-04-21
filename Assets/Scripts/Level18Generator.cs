using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Level18Generator : MonoBehaviour
{
    [Header("Foundation Settings")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private int totalWidthInBlocks = 20;
    [SerializeField] private float ceilingYOffset = 6f; // Tavan Yüksekliği Ayarı

    [Header("Wall Settings")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private int wallRows = 8; // Duvar Yüksekliği Ayarı (Sıra Sayısı)
    [SerializeField] private float wallYOffset = 0f;

    [Header("Special Objects")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject startIglooPrefab;
    [SerializeField] private GameObject finishIglooPrefab;
    [Header("Spike Trap Settings")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private int spikeCount = 10;
    [SerializeField] private float spikeYOffset = 0f; // Ekstra Y kaydırma ayarı

    [Header("Ceiling Spike Settings")]
    [SerializeField] private GameObject ceilingSpikePrefab;
    [SerializeField] private GameObject ceilingSpecialSpikePrefab;
    [SerializeField] private int ceilingSpikeCount = 9;
    [SerializeField] private float ceilingSpikeYOffset = 0f;

    [Header("Extra Ground Spike Settings")]
    [SerializeField] private GameObject extraGroundSpikePrefab;
    [SerializeField] private int extraGroundSpikeIndex = 15;
    [SerializeField] private float extraGroundSpikeYOffset = 0f;

    [Header("Saw Trap Settings")]
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private int sawIndex = 20;
    [SerializeField] private float sawYOffset = 0f;

    [Header("Trampoline Settings")]
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private int trampolineOffsetFromEnd = 2; // Igloo'dan kaç blok önce
    [SerializeField] private float trampolineYOffset = 0f;

    [Header("End Ceiling Spike Settings")]
    [SerializeField] private GameObject endCeilingSpikePrefab;
    [SerializeField] private int endCeilingSpikeCount = 3;
    [SerializeField] private float endCeilingSpikeYOffset = 0f;

    private bool canGenerate = true;

    void Awake()
    {
        if (groundPrefab == null || ceilingPrefab == null || wallPrefab == null || 
            playerPrefab == null || startIglooPrefab == null || 
            finishIglooPrefab == null || spikePrefab == null || 
            ceilingSpikePrefab == null || ceilingSpecialSpikePrefab == null ||
            extraGroundSpikePrefab == null || sawPrefab == null || trampolinePrefab == null ||
            endCeilingSpikePrefab == null)
        {
            Debug.LogError("Level18Generator: Prefabs are missing!");
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

        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float xPos = (i * groundHalfWidth * 2) + groundHalfWidth;
            Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity);

            switch (i)
            {
                case 0:
                    GenerateStartArea(xPos, groundHalfHeight);
                    break;
                case 2:
                    GenerateSpikeTrap(xPos, groundHalfHeight);
                    break;
                case 6:
                    GenerateCeilingTrap(xPos);
                    break;
                case int n when n == totalWidthInBlocks - 2:
                    GenerateEndCeilingTrap(xPos, groundHalfWidth);
                    break;
                case int n when n == totalWidthInBlocks - 1:
                    GenerateFinishArea(xPos, groundHalfWidth, groundHalfHeight);
                    break;
                case int n when n == extraGroundSpikeIndex - 1:
                    GenerateExtraSpike(xPos, groundHalfHeight);
                    break;
                case int n when n == sawIndex - 1:
                    GenerateSawTrap(xPos, groundHalfHeight);
                    break;
                case int n when n == totalWidthInBlocks - trampolineOffsetFromEnd:
                    GenerateTrampolineTrap(xPos, groundHalfHeight);
                    break;
            }

            float ceilingHalfHeight = GetHalfHeight(ceilingPrefab);
            Instantiate(ceilingPrefab, new Vector3(xPos, ceilingYOffset + ceilingHalfHeight, 0), Quaternion.identity);
        }
    }

    private void GenerateStartArea(float xPos, float groundHalfHeight)
    {
        GenerateWalls(0, true);
        float startIglooY = groundHalfHeight + GetHalfHeight(startIglooPrefab);
        Instantiate(startIglooPrefab, new Vector3(xPos, startIglooY, 0), Quaternion.Euler(0, 180, 0));
        float playerY = groundHalfHeight + GetHalfHeight(playerPrefab);
        Instantiate(playerPrefab, new Vector3(xPos, playerY, 0), Quaternion.identity);
    }

    private void GenerateFinishArea(float xPos, float groundHalfWidth, float groundHalfHeight)
    {
        GenerateWalls(totalWidthInBlocks * groundHalfWidth * 2, false);
        float finishY = groundHalfHeight + GetHalfHeight(finishIglooPrefab);
        Instantiate(finishIglooPrefab, new Vector3(xPos, finishY, 0), Quaternion.identity);
    }

    private void GenerateSpikeTrap(float xPos, float groundHalfHeight)
    {
        float spikeWidth = GetHalfWidth(spikePrefab) * 2f;
        float spikeHalfHeight = GetHalfHeight(spikePrefab);
        float trapY = groundHalfHeight + spikeHalfHeight + spikeYOffset;

        for (int s = 0; s < spikeCount; s++)
        {
            float spikeX = xPos + (s * spikeWidth);
            Instantiate(spikePrefab, new Vector3(spikeX, trapY, 0), Quaternion.identity);
        }
    }

    private void GenerateCeilingTrap(float xPos)
    {
        float cSpikeWidth = GetHalfWidth(ceilingSpikePrefab) * 2f;
        float ceilingSurfaceY = ceilingYOffset;

        for (int s = 0; s < ceilingSpikeCount; s++)
        {
            float cSpikeX = xPos + (s * cSpikeWidth);
            GameObject prefabToUse = (s == ceilingSpikeCount - 1) ? ceilingSpecialSpikePrefab : ceilingSpikePrefab;
            
            if (prefabToUse != null)
            {
                float cSpikeHalfHeight = GetHalfHeight(prefabToUse);
                float cSpikeY = ceilingSurfaceY - cSpikeHalfHeight + ceilingSpikeYOffset;
                Instantiate(prefabToUse, new Vector3(cSpikeX, cSpikeY, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateExtraSpike(float xPos, float groundHalfHeight)
    {
        float extraSpikeHalfHeight = GetHalfHeight(extraGroundSpikePrefab);
        float extraSpikeY = groundHalfHeight + extraSpikeHalfHeight + extraGroundSpikeYOffset;
        Instantiate(extraGroundSpikePrefab, new Vector3(xPos, extraSpikeY, 0), Quaternion.identity);
    }

    private void GenerateSawTrap(float xPos, float groundHalfHeight)
    {
        float sawHalfHeight = GetHalfHeight(sawPrefab);
        float sawY = groundHalfHeight + (sawHalfHeight * 2f) + sawYOffset;
        Instantiate(sawPrefab, new Vector3(xPos, sawY, 0), Quaternion.identity);
    }

    private void GenerateTrampolineTrap(float xPos, float groundHalfHeight)
    {
        float trampHalfHeight = GetHalfHeight(trampolinePrefab);
        float trampY = groundHalfHeight + trampHalfHeight + trampolineYOffset;
        Instantiate(trampolinePrefab, new Vector3(xPos, trampY, 0), Quaternion.identity);
    }

    private void GenerateEndCeilingTrap(float xPos, float groundHalfWidth)
    {
        float eCSpikeWidth = GetHalfWidth(endCeilingSpikePrefab) * 2f;
        float ceilingSurfaceY = ceilingYOffset;
        float startX = xPos - groundHalfWidth;

        for (int s = 0; s < endCeilingSpikeCount; s++)
        {
            float eCSpikeX = startX + (s * eCSpikeWidth) + (eCSpikeWidth / 2f);
            float eCSpikeHalfHeight = GetHalfHeight(endCeilingSpikePrefab);
            float eCSpikeY = ceilingSurfaceY - eCSpikeHalfHeight + endCeilingSpikeYOffset;
            Instantiate(endCeilingSpikePrefab, new Vector3(eCSpikeX, eCSpikeY, 0), Quaternion.identity);
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
