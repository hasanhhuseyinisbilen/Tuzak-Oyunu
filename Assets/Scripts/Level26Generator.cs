using UnityEngine;

public class Level26Generator : MonoBehaviour
{
    private static Level26Generator _instance;

    [Header("Prefablar")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject finishMushroomPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject elevatorPrefab;
    [SerializeField] private GameObject emptyBoxPrefab;
    [SerializeField] private GameObject boxPrefab;

    [Header("Case 4 Özel Objeleri")]
    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private GameObject specialColumnPrefab;
    [SerializeField] private GameObject vinePrefab;
    [Tooltip("Sarmaşığın Y eksenindeki konumunu ayarlamak için (Örn: -0.5)")]
    [SerializeField] private float vineYOffset = 0f;

    [SerializeField] private GameObject groundSpikePrefab;
    [SerializeField] private GameObject arrowPrefab;
    
    [Header("Level Ayarları")]
    [SerializeField] private int totalWidthInBlocks = 9;
    [SerializeField] private int wallRows = 5;
    [SerializeField] private int wallColumns = 3;
    [SerializeField] private float wallYOffset = 0f;

    private float groundHalfWidth;
    private float groundHalfHeight;

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
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        return (sr != null) ? sr.bounds.extents.x : 0.5f;
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 0.5f;
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        return (sr != null) ? sr.bounds.extents.y : 0.5f;
    }

    private void GenerateLevel()
    {
        if (groundPrefab == null) return;

        groundHalfWidth = GetHalfWidth(groundPrefab);
        groundHalfHeight = GetHalfHeight(groundPrefab);

        float startX = 0f;
        float currentX = startX;

        GenerateWalls(startX, true);

        // 7 ground + 1 elevator = 8 total positions in a row
        for (int i = 0; i < totalWidthInBlocks; i++)
        {
            float groundX = currentX + groundHalfWidth;
            float topY = groundHalfHeight;

            switch (i)
            {
                case 0:
                    Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    if (playerPrefab != null)
                        Instantiate(playerPrefab, new Vector3(groundX, topY + GetHalfHeight(playerPrefab), 0), Quaternion.identity);
                    if (mushroomPrefab != null)
                        Instantiate(mushroomPrefab, new Vector3(groundX, topY + GetHalfHeight(mushroomPrefab), 0), Quaternion.Euler(0, 180, 0));
                    break;

                case 1:
                case 5:
                case 6:
                    Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    break;

                case 4:
                    Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    
                    GameObject currentKolon = columnPrefab != null ? columnPrefab : boxPrefab;
                    if (currentKolon != null && vinePrefab != null)
                    {
                        float colHalfW = GetHalfWidth(currentKolon);
                        float colHalfH = GetHalfHeight(currentKolon);
                        float vineHalfW = GetHalfWidth(vinePrefab);
                        float vineHalfH = GetHalfHeight(vinePrefab);

                        float placeX = currentX;
                        int[] sequence = { 0, 1, 0, 1, 2, 1, 0 }; // 0: Kolon, 1: Sarmaşık, 2: Özel Kolon
                        
                        foreach (int item in sequence)
                        {
                            if (item == 0)
                            {
                                placeX += colHalfW;
                                Instantiate(currentKolon, new Vector3(placeX, topY + colHalfH, 0), Quaternion.identity);
                                placeX += colHalfW;
                            }
                            else if (item == 1)
                            {
                                placeX += vineHalfW;
                                Instantiate(vinePrefab, new Vector3(placeX, topY + vineHalfH + vineYOffset, 0), Quaternion.identity);
                                placeX += vineHalfW;
                            }
                            else if (item == 2)
                            {
                                GameObject currentSpecialKolon = specialColumnPrefab != null ? specialColumnPrefab : currentKolon;
                                float specialColHalfW = GetHalfWidth(currentSpecialKolon);
                                float specialColHalfH = GetHalfHeight(currentSpecialKolon);
                                
                                placeX += specialColHalfW;
                                Instantiate(currentSpecialKolon, new Vector3(placeX, topY + specialColHalfH, 0), Quaternion.identity);
                                placeX += specialColHalfW;
                            }
                        }
                    }
                    break;

                case 2:
                    if (emptyBoxPrefab != null)
                        Instantiate(emptyBoxPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    else
                        Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    break;

                case 3:
                    Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    if (boxPrefab != null)
                    {
                        Instantiate(boxPrefab, new Vector3(groundX, topY + GetHalfHeight(boxPrefab), 0), Quaternion.Euler(0, 0, 90));
                    }
                    break;

                case 7:
                    if (elevatorPrefab != null)
                    {
                        Instantiate(elevatorPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    }
                    break;

                case int n when (n == totalWidthInBlocks - 1):
                    Instantiate(groundPrefab, new Vector3(groundX, 0, 0), Quaternion.identity);
                    if (groundSpikePrefab != null)
                    {
                        float spikeHalfW = GetHalfWidth(groundSpikePrefab);
                        Instantiate(groundSpikePrefab, new Vector3(currentX + spikeHalfW, topY + GetHalfHeight(groundSpikePrefab), 0), Quaternion.identity);
                    }

                    // 5 ground bloğu yüksekliğinde yeni bir zemin (her bloğun yüksekliği groundHalfHeight * 2)
                    float elevatedY = (groundHalfHeight * 2) * 4;
                    Instantiate(groundPrefab, new Vector3(groundX, elevatedY, 0), Quaternion.identity);

                    if (finishMushroomPrefab != null)
                    {
                        float finishMushHalfH = GetHalfHeight(finishMushroomPrefab);
                        // Yeni zeminin üstüne bitiş mantarını koyuyoruz
                        Instantiate(finishMushroomPrefab, new Vector3(groundX, elevatedY + groundHalfHeight + finishMushHalfH, 0), Quaternion.identity);
                    }
                    break;
            }

            currentX += groundHalfWidth * 2f;
        }

        GenerateWalls(currentX, false);
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
                float xPos = isLeft 
                    ? xOrigin - (col * wallHalfWidth * 2) - wallHalfWidth 
                    : xOrigin + (col * wallHalfWidth * 2) + wallHalfWidth;
                
                float yPos = (row * wallHalfHeight * 2) + wallHalfHeight + wallYOffset;
                
                Instantiate(wallPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);

                if (isLeft && col == 0 && arrowPrefab != null)
                {
                    float arrowHalfW = GetHalfWidth(arrowPrefab);
                    // Duvarın tam sağ kenarından başlasın diye wallHalfWidth ekliyoruz, kendi genişliği kadar daha kaydırıyoruz.
                    Instantiate(arrowPrefab, new Vector3(xPos + wallHalfWidth + arrowHalfW, yPos, 0), Quaternion.Euler(0, 0, 270));
                }
            }
        }
    }
}
