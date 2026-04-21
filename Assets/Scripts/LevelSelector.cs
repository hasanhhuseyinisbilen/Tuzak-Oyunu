using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static int SelectedLevelIndex { get; private set; }

    public void OpenLevel(int levelIndex)
    {
        // CAN KONTROLÜ: Eğer can bitti ise leveli açtırma
        if (LivesManager.Instance != null && LivesManager.Instance.currentLives <= 0)
        {
            Debug.LogWarning("Can bitti! Bölüm seçimine izin verilmiyor.");
            // Sahnede LivesUI varsa paneli açar
            // LivesManager içindeki statik eventi tetikle
            return; // Fonksiyondan çık, sahne yükleme!
        }

        SelectedLevelIndex = levelIndex;
        SceneManager.LoadScene(levelIndex);
    }

    public void OpenLevel(string levelName)
    {
        // CAN KONTROLÜ
        if (levelName.Contains("Level") && LivesManager.Instance != null && LivesManager.Instance.currentLives <= 0)
        {
            Debug.LogWarning("Can bitti! " + levelName + " açılamıyor.");
            return;
        }

        SceneManager.LoadScene(levelName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0); 
    }
}