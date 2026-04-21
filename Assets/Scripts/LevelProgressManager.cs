using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressManager : MonoBehaviour
{
    [Header("Bölümü Bitirince Aktifleşecek Level Numarası")]
    [Tooltip("Eğer oyuncu Level 1'i bitirdiyse, bu değeri 2 yapın. Böylece 2. bölüm kilidi açılır.")]
    [SerializeField] private int levelToUnlock;

    // Bu fonksiyonu bölümü bitirince çalıştıracak şekilde tetikleyin (örneğin bitiş igloosuna çarpınca)
    public void LevelPassed()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Sadece eğer geçilen bölüm, mevcut açık olan bölümden daha ileriyse kaydet.
        if (levelToUnlock > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", levelToUnlock);
            PlayerPrefs.Save();
        }
    }

    // İsteğe bağlı: Tüm kayıtları sıfırlamak için bir metod (Ayarlar menüsüne eklenebilir)
    public void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();
    }
}
