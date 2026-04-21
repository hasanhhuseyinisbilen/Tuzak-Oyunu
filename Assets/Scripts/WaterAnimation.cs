using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    [Header("Dalga Ayarları")]
    public float amplitude = 0.2f;
    public float frequency = 2f;
    public bool autoPhase = true;
    public float manualPhase = 0f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        if (autoPhase) manualPhase = transform.position.x * 0.8f;
    }

    void Update()
    {

        float newY = startPos.y + Mathf.Sin(Time.time * frequency + manualPhase) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
