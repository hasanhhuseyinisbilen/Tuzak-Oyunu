using UnityEngine;

[ExecuteAlways]
public class ResponseBackgraund : MonoBehaviour
{
    private SpriteRenderer sr;
    private Camera cam;

    void Update()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (cam == null) cam = Camera.main;

        if (sr != null && sr.sprite != null && cam != null)
        {
            float camHeight = cam.orthographicSize * 2f;
            float camWidth = camHeight * cam.aspect;
            Vector2 spriteSize = sr.sprite.bounds.size;

            float scale = Mathf.Max(camWidth / spriteSize.x, camHeight / spriteSize.y);
            transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
