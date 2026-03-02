using UnityEngine;

[ExecuteAlways]
public class FitBackground : MonoBehaviour
{
    void Update()
    {
        var sr = GetComponent<SpriteRenderer>();
        var cam = Camera.main;

        if (sr && cam)
        {
            var bounds = sr.sprite.bounds;
            var height = cam.orthographicSize * 2f;
            var width = height * cam.aspect;
            transform.localScale = new Vector3(width / bounds.size.x, height / bounds.size.y, 1f);
        }
    }
}
