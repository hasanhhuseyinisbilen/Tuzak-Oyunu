using UnityEngine;

public class LevelDebugTool : MonoBehaviour
{
    private void OnGUI()
    {
        // Sadece Unity Editörde veya Debug modunda göster
        if (!Debug.isDebugBuild && !Application.isEditor) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 25;
        style.normal.textColor = Color.yellow;

        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int current = PlayerPrefs.GetInt("CurrentPlayingLevel", -1);
        
        string displayText = $"DEBUG - En Yüksek Açık Level: {unlocked}\n" +
                           $"Şu An Oynanan: {(current == -1 ? "Bilinmiyor" : current.ToString())}";

        GUI.Label(new Rect(20, 20, 400, 100), displayText, style);
    }
}
