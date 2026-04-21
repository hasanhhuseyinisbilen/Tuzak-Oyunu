using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
