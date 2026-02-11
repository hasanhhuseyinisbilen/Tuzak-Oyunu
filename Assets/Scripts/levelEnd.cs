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

        SnowyLevelManager manager = FindObjectOfType<SnowyLevelManager>();
        if (manager != null)
        {
            int currentIdx = LevelSelector.SelectedLevelIndex; 
            int reached = PlayerPrefs.GetInt("ReachedLevel", 0); 
            if (currentIdx == reached)
            {
                PlayerPrefs.SetInt("ReachedLevel", reached + 1);
                PlayerPrefs.Save();
            }
              SceneManager.LoadScene(1); 
        }
    }
}
