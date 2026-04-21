using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [Header("Hedef Sahne Adı")]
    [SerializeField] private string menuSceneName = "LevelSelector";

    /// <summary>
    /// Button click event'ine bu fonksiyonu bağla.
    /// </summary>
    public void GoToMenu()
    {
        Debug.Log("Menüye dönülüyor: " + menuSceneName);
        Time.timeScale = 1; // Zamanı tekrar başlat
        SceneManager.LoadScene(menuSceneName);
    }

    /// <summary>
    /// Eğer sahne index ile yüklenecekse bu kullanılabilir.
    /// </summary>
    public void GoToMenuByIndex(int index)
    {
        Debug.Log("Menüye index ile dönülüyor: " + index);
        Time.timeScale = 1; // Zamanı tekrar başlat
        SceneManager.LoadScene(index);
    }
}
