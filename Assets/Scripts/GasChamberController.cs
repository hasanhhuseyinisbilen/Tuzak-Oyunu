using UnityEngine;

public class GasChamberController : MonoBehaviour
{
    [Header("Gas Parents")]
    public Transform leftGasParent;
    public Transform rightGasParent;
    public Transform topGasParent;

    [Header("Settings")]
    public float timeLimit = 90f;
    private float elapsedTime = 0f;

    [Header("Targets")]
    public float targetY = 0.5f;
    public float targetMidX;

    private Vector3 initialLeftPos;
    private Vector3 initialRightPos;
    private Vector3 initialTopPos;

    void Start()
    {
        if (leftGasParent) initialLeftPos = leftGasParent.position;
        if (rightGasParent) initialRightPos = rightGasParent.position;
        if (topGasParent) initialTopPos = topGasParent.position;
    }

    void Update()
    {
        if (elapsedTime < timeLimit)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / timeLimit;

            if (leftGasParent)
                leftGasParent.position = new Vector3(Mathf.Lerp(initialLeftPos.x, targetMidX, t), initialLeftPos.y, 0);

            if (rightGasParent)
                rightGasParent.position = new Vector3(Mathf.Lerp(initialRightPos.x, targetMidX, t), initialRightPos.y, 0);

            if (topGasParent)
                topGasParent.position = new Vector3(initialTopPos.x, Mathf.Lerp(initialTopPos.y, targetY, t), 0);
        }
    }
}
