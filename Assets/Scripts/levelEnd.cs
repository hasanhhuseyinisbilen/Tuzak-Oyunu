using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelEnd : MonoBehaviour
{
    private bool triggered = false;
    // private SnowyLevelManager manager;

    private void Start()
    {
        // manager = FindObjectOfType<SnowyLevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);
        if (nextSceneIndex > reachedLevel)
        {
            PlayerPrefs.SetInt("ReachedLevel", nextSceneIndex);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene(0);
    }
}
