using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private GameObject bgPrefab;
    [SerializeField] private int columns = 10;
    [SerializeField] private int rows = 1;
    [SerializeField] private float parallaxMultiplier = 0.5f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;

    void Start()
    {
        if (bgPrefab != null) GenerateBackground();
    }

    private void GenerateBackground()
    {
        Vector3 bgSize = bgPrefab.GetComponent<Renderer>().bounds.size;

        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                float xPos = xOffset + (col * bgSize.x) + (bgSize.x / 2f);
                float yPos = yOffset + (row * bgSize.y) + (bgSize.y / 2f);
                
                GameObject bgPart = Instantiate(bgPrefab, new Vector3(xPos, yPos, 10f), Quaternion.identity);
                bgPart.transform.parent = this.transform;
                bgPart.name = $"BG_{col}_{row}";

                // Parallax efektini ekle
                ParallaxEffect parallax = bgPart.AddComponent<ParallaxEffect>();
                parallax.parallaxFactor = parallaxMultiplier;
            }
        }
    }
}
