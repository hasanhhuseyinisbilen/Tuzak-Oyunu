using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // Eğer hedef sahne bir bölüm ise ve can bitti ise engelle
        if (sceneName.Contains("Level") && LivesManager.Instance != null && LivesManager.Instance.currentLives <= 0)
        {
            Debug.LogWarning("Can bitti! Sahne yüklenemiyor: " + sceneName);
            return;
        }

        Debug.Log("SceneLoader: " + sceneName + " sahnesi yükleniyor...");
        Time.timeScale = 1; // Donmayı çöz
        SceneManager.LoadScene(sceneName);
    }
}
