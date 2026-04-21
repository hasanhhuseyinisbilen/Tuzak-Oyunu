using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public GameObject levelPrefab;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        Debug.Log("Butona tıklandı: " + (levelPrefab != null ? levelPrefab.name : "PREFAB YOK!"));
        if (levelPrefab != null)
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadPrefabDirectly(levelPrefab);
            }
            else
            {
                Debug.LogError("Sahne üzerinde LevelManager objesi bulunamadı! Lütfen sahnede LevelManager scriptinin takılı olduğu bir obje olduğundan emin olun.");
            }
        }
    }
}
