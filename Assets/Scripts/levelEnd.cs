using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelEnd : MonoBehaviour
{
    private bool triggered = false;
    private SnowyLevelManager manager;

    private void Start()
    {
        manager = FindObjectOfType<SnowyLevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (manager != null)
        {
            int currentIdx = LevelSelector.SelectedLevelIndex; 
            int reached = PlayerPrefs.GetInt("ReachedLevel", 0); 
            if (currentIdx == reached)
            {
                PlayerPrefs.SetInt("ReachedLevel", reached + 1);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
    }
}
