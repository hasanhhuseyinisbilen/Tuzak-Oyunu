using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static int SelectedLevelIndex { get; private set; }

    public void OpenLevel(int levelIndex)
    {
        SelectedLevelIndex = levelIndex;
       
        SceneManager.LoadScene(levelIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0); 
    }
}
