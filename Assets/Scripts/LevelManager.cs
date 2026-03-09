using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Bu script sahneler arası veri taşır ve hangi seviyenin yükleneceğini yönetir.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Seviye Prefabları")]
    public GameObject[] levelPrefabs;

    [Header("Seçili Veri")]
    public int currentLevelIndex;
    public string gameSceneName = "GameScene"; // Levellerin içinde doğacağı ana sahne

    void Awake()
    {
        // Singleton: Sahne değiştiğinde bu obje silinmesin
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

    // Butonların OnClick kısmına bu fonksiyonu bağla!
    // Parametre olarak listenin sırasını (0, 1, 2...) gir.
    public void SelectAndLoadLevel(int index)
    {
        Debug.Log("LevelManager: Seçilen Seviye Indexi: " + index);
        
        if (index >= 0 && index < levelPrefabs.Length)
        {
            currentLevelIndex = index;
            string prefabName = levelPrefabs[index] != null ? levelPrefabs[index].name : "BOŞ PREFAB!";
            Debug.Log("LevelManager: Yükleniyor... Sahne: " + gameSceneName + " | Prefab: " + prefabName);
            
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Hata: " + index + " numaralı bir seviye prefab listesinde yok!");
        }
    }

    // Seçili prefab'ı döndürür
    public GameObject GetCurrentLevelPrefab()
    {
        if (currentLevelIndex >= 0 && currentLevelIndex < levelPrefabs.Length)
        {
            return levelPrefabs[currentLevelIndex];
        }
        return null;
    }
}
