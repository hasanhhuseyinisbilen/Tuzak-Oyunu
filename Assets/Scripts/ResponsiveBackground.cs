using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ResponsiveBackground : MonoBehaviour
{
    [Header("Saydam Kutu (Opsiyonel)")]
    public RectTransform saydamPanel; 
    [Range(0.1f, 1f)] public float panelGenislikOrani = 0.8f;
    [Range(0.1f, 1f)] public float panelYukseklikOrani = 0.8f;

    void Update()
    {
        // 1. Arka Planı Doldur (Kendisi)
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
        }

        // 2. Saydam Kutuyu (Panel) Ortalayıp Ölçeklendir
        if (saydamPanel != null)
        {
            // Paneli tam merkeze sabitle
            saydamPanel.anchorMin = new Vector2(0.5f, 0.5f);
            saydamPanel.anchorMax = new Vector2(0.5f, 0.5f);
            saydamPanel.pivot = new Vector2(0.5f, 0.5f);
            saydamPanel.anchoredPosition = Vector2.zero;

            // Ekranın belli bir yüzdesi kadar büyük olsun
            Rect parentRect = ((RectTransform)transform.parent).rect;
            saydamPanel.sizeDelta = new Vector2(parentRect.width * panelGenislikOrani, parentRect.height * panelYukseklikOrani);
        }
        
        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.type = Image.Type.Simple;
            img.preserveAspect = false;
        }
    }
}
