using UnityEngine;

[ExecuteAlways]
public class BackgroundResponsive : MonoBehaviour
{
    private SpriteRenderer sr;
    private Camera cam;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    void Update()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        if (sr != null && sr.sprite != null && cam != null)
        {
            float camHeight = cam.orthographicSize * 2.0f;
            float camWidth = camHeight * cam.aspect;

            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            float scaleX = camWidth / spriteWidth;
            float scaleY = camHeight / spriteHeight;

            float finalScale = Mathf.Max(scaleX, scaleY);
            
            transform.localScale = new Vector3(finalScale, finalScale, 1.0f);
        }
    }
}
