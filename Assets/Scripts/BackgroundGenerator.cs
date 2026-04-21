using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

[System.Serializable]
public struct BackgroundPieceData
{
    public Transform transform;
    public float3 startPos;
    public float parallaxFactor;
}

public class BackgroundGenerator : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private GameObject bgPrefab;
    [SerializeField] private int yataySayisi = 10; 
    [SerializeField] private int dikeySayisi = 1;  
    [SerializeField] private float parallaxMultiplier = 0.5f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;

    private List<BackgroundPieceData> _pieces = new List<BackgroundPieceData>();
    private Transform _camTransform;
    private float3 _camStartPos;

    void Start()
    {
        if (Camera.main != null)
        {
            _camTransform = Camera.main.transform;
            _camStartPos = (float3)_camTransform.position;
        }

        if (bgPrefab != null) GenerateBackground();
    }

    private void GenerateBackground()
    {
        float bgHalfWidth = GetHalfWidth(bgPrefab);
        float bgHalfHeight = GetHalfHeight(bgPrefab);

        if (bgHalfWidth <= 0.1f) bgHalfWidth = 5f;
        if (bgHalfHeight <= 0.1f) bgHalfHeight = 5f;

        _pieces.Clear();

        for (int col = 0; col < yataySayisi; col++)
        {
            for (int row = 0; row < dikeySayisi; row++)
            {
                float xPos = xOffset + (col * bgHalfWidth * 2) + bgHalfWidth;
                float yPos = yOffset + (row * bgHalfHeight * 2) + bgHalfHeight;
                
                GameObject bgPart = Instantiate(bgPrefab, new Vector3(xPos, yPos, 10f), Quaternion.identity);
                bgPart.transform.parent = this.transform;
                bgPart.name = $"BG_Sutun{col}_Satir{row}";

                _pieces.Add(new BackgroundPieceData
                {
                    transform = bgPart.transform,
                    startPos = new float3(xPos, yPos, 10f),
                    parallaxFactor = parallaxMultiplier
                });
            }
        }
    }

    private void LateUpdate()
    {
        if (_camTransform == null || _pieces.Count == 0) return;

        float3 camCurrentPos = (float3)_camTransform.position;
        float3 camDelta = camCurrentPos - _camStartPos;

        for (int i = 0; i < _pieces.Count; i++)
        {
            BackgroundPieceData data = _pieces[i];
            float newX = data.startPos.x + (camDelta.x * data.parallaxFactor);
            data.transform.position = new Vector3(newX, data.startPos.y, data.startPos.z);
        }
    }

    private float GetHalfWidth(GameObject prefab)
    {
        if (prefab == null) return 5f;
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null && r.bounds.size.x > 0) ? r.bounds.size.x / 2f : 5f;
    }

    private float GetHalfHeight(GameObject prefab)
    {
        if (prefab == null) return 5f;
        Renderer r = prefab.GetComponentInChildren<Renderer>();
        return (r != null && r.bounds.size.y > 0) ? r.bounds.size.y / 2f : 5f;
    }
}
