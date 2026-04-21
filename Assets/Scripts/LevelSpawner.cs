using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    void Start()
    {
        if (LevelManager.Instance != null)
        {
            GameObject levelPrefab = LevelManager.Instance.GetCurrentLevelPrefab();
            Debug.Log("LevelSpawner: GetCurrentLevelPrefab sonucu -> " + (levelPrefab != null ? levelPrefab.name : "NULL"));
            
            if (levelPrefab != null)
            {
                Debug.Log("LevelSpawner: Prefab oluşturuluyor (Instantiate)...");
                Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Oluşturulacak seviye prefabı bulunamadı!");
            }
        }
    }
}
