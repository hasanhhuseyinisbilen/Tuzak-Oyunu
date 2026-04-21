using UnityEngine;
using UnityEngine.UI;

public class WorldSelector : MonoBehaviour
{
    [Header("Sayfalarınızı (Buz, Orman) Buraya Ekleyin")]
    public GameObject[] worldPanels;

    [Header("Geçiş Okları (İsteğe Bağlı)")]
    public Button leftArrow;
    public Button rightArrow;

    private int currentIndex = 0;
    private const string WORLD_INDEX_KEY = "SelectedWorldIndex";

    void Start()
    {
        // Kayıtlı olan indexi yükle, yoksa 0'dan başla
        currentIndex = PlayerPrefs.GetInt(WORLD_INDEX_KEY, 0);
        
        Debug.Log($"WorldSelector: Başlatılıyor. Toplam Panel: {worldPanels.Length}, Başlangıç Index: {currentIndex}");
        UpdateWorldSelection();
    }

    public void GoNextWorld()
    {
        Debug.Log("<color=green>WorldSelector: SAĞ OK (İleri) tıklandı!</color>");
        if (currentIndex < worldPanels.Length - 1)
        {
            currentIndex++;
            PlayerPrefs.SetInt(WORLD_INDEX_KEY, currentIndex);
            PlayerPrefs.Save();
            UpdateWorldSelection();
        }
        else
        {
            Debug.LogWarning("WorldSelector: Son sayfadasın, ileri gidilemez!");
        }
    }

    public void GoPreviousWorld()
    {
        Debug.Log("<color=yellow>WorldSelector: SOL OK (Geri) tıklandı!</color>");
        if (currentIndex > 0)
        {
            currentIndex--;
            PlayerPrefs.SetInt(WORLD_INDEX_KEY, currentIndex);
            PlayerPrefs.Save();
            UpdateWorldSelection();
        }
        else
        {
            Debug.LogWarning("WorldSelector: İlk sayfadasın, geri gidilemez!");
        }
    }

    private void UpdateWorldSelection()
    {
        Debug.Log($"WorldSelector: Ekran Güncelleniyor -> Aktif Index: {currentIndex}");

        for (int i = 0; i < worldPanels.Length; i++)
        {
            if (worldPanels[i] != null)
            {
                bool shouldBeActive = (i == currentIndex);
                worldPanels[i].SetActive(shouldBeActive);
            }
            else
            {
                Debug.LogError($"WorldSelector HATA! {i}. panel boş!");
            }
        }

        if (leftArrow != null) leftArrow.interactable = (currentIndex > 0);
        if (rightArrow != null) rightArrow.interactable = (currentIndex < worldPanels.Length - 1);
    }
}
