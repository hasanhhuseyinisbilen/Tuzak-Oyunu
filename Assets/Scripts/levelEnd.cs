using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelEnd : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        int currentLevel = PlayerPrefs.GetInt("CurrentPlayingLevel", 1);
        int nextLevel = currentLevel + 1;

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (nextLevel > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();
            Debug.Log($"<color=green>TEBRİKLER!</color> Bölüm {currentLevel} Geçildi. Bölüm {nextLevel} kilidi açıldı!");
        }

        ReturnToMenu();
    }

    private void ReturnToMenu()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentPlayingLevel", 1);
        
        if (currentLevel >= 20 && currentLevel <= 40)
        {
            SceneManager.LoadScene("JungleLevel");
            return;
        }

        if (LevelManager.Instance != null && !string.IsNullOrEmpty(LevelManager.Instance.levelSelectorSceneName))
        {
            SceneManager.LoadScene(LevelManager.Instance.levelSelectorSceneName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
