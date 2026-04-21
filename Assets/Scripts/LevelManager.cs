using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>

/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Seviye Prefabları")]
    public GameObject[] levelPrefabs;

    [Header("Seçili Veri")]
    public int currentLevelIndex;
    public GameObject currentLevelPrefab; 
    public string gameSceneName = "GameScene";
    public string levelSelectorSceneName = "LevelSelector";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectAndLoadLevel(int index)
    {
        if (index >= 0 && index < levelPrefabs.Length)
        {
            currentLevelIndex = index;
            
            // Bölüm başlarken canları ve zamanı sıfırla
            if (LivesManager.Instance != null) LivesManager.Instance.ResetLivesToMax();
            Time.timeScale = 1; 
            
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public void LoadPrefabDirectly(GameObject prefab)
    {
        if (prefab != null)
        {
            Debug.Log("LevelManager: Direkt prefab yükleniyor -> " + prefab.name);
            currentLevelPrefab = prefab; 

            // Bölüm başlarken canları ve zamanı sıfırla
            if (LivesManager.Instance != null) LivesManager.Instance.ResetLivesToMax();
            Time.timeScale = 1; 
            
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public GameObject GetCurrentLevelPrefab()
    {
        if (currentLevelPrefab != null) return currentLevelPrefab;

        if (currentLevelIndex >= 0 && currentLevelIndex < levelPrefabs.Length)
        {
            return levelPrefabs[currentLevelIndex];
        }
        return null;
    }
}
