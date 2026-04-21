using UnityEngine;

public class ProgressResetHelper : MonoBehaviour
{
    [ContextMenu("RESET ALL PROGRESS")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Veya sadece belirli keyleri: PlayerPrefs.DeleteKey("UnlockedLevel");
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("CurrentPlayingLevel", 1);
        PlayerPrefs.Save();
        Debug.Log("<color=red>TÜM İLERLEME SIFIRLANDI!</color> Level 1'den başlayabilirsiniz.");
    }
}
