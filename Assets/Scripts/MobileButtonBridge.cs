using UnityEngine;

public class MobileButtonBridge : MonoBehaviour
{
    public void SetMove(float value)
    {
        if (PlayerMovement2D.Instance != null)
            PlayerMovement2D.Instance.SetMobileMoveInput(value);
    }

    public void Jump()
    {
        if (PlayerMovement2D.Instance != null)
            PlayerMovement2D.Instance.MobileJumpDown();
    }
}
