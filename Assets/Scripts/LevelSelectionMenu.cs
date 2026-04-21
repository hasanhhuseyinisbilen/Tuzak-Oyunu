using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour
{
    [Header("Bölüm Butonlarını Sırayla Buraya Ekleyin (Level 1, Level 2...)")]
    [SerializeField] private Button[] levelButtons;

    [Header("Bu sayfadaki ilk bölümün numarası (Örn: Snowy için 1, Jungle için 21)")]
    [SerializeField] private int startingLevelNumber = 1;

    private void Start()
    {
        // Oyuncunun ulaştığı en yüksek bölümü alıyoruz. Varsayılan başlangıç 1'dir.
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            // Bu butonun temsil ettiği gerçek bölüm sayısı:
            int thisButtonLevel = startingLevelNumber + i;

            // Kilitleme mantığı
            if (thisButtonLevel > unlockedLevel)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
                
                // --- SENIOR ÇÖZÜM: BUTONA OTOMATİK TAKİP KODU EKLEME ---
                // Butona tıklandığında hangi levelin oynandığını hafızaya alır.
                int capturedLevel = thisButtonLevel;
                levelButtons[i].onClick.AddListener(() => {
                    PlayerPrefs.SetInt("CurrentPlayingLevel", capturedLevel);
                    PlayerPrefs.Save();
                });
            }
        }
    }
}
