using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class ResponseBackgraund : MonoBehaviour
{
    [SerializeField] private float padding = 0.01f;
    [Range(0, 1)] [SerializeField] private float widthPercent = 1f;
    [Range(0, 1)] [SerializeField] private float heightPercent = 1f;

    private Camera cam;
    private SpriteRenderer sr;

    private void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        
        ScaleBackground();
    }

    private void LateUpdate()
    {
        if (!cam) cam = Camera.main;
        if (!sr) sr = GetComponent<SpriteRenderer>();

        if (!cam || !cam.orthographic || !sr || !sr.sprite) return;

        ScaleBackground();
    }

    public void ScaleBackground()
    {
        float worldHeight = cam.orthographicSize * 2f;
        float worldWidth = worldHeight * cam.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size;

        transform.localScale = new Vector3(
            (worldWidth * widthPercent) / spriteSize.x + padding,
            (worldHeight * heightPercent) / spriteSize.y + padding,
            1f
        );
    }
}
