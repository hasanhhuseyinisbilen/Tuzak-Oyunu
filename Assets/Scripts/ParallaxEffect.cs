using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Hedef Kamera")]
    public GameObject cam;

    [Header("Parallax Ayarları")]
    public float parallaxFactorX = 0.5f;
    public float parallaxFactorY = 0.2f;
    public bool followY = true;
    
    [Header("Sonsuz Döngü")]
    public bool infiniteScrolling = true;

    private float lengthX, startPosX, startPosY;
    private float camStartPosX, camStartPosY;

    void Start()
    {
        TryInitializeCamera();

        startPosX = transform.position.x;
        startPosY = transform.position.y;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) 
        {
            lengthX = sr.bounds.size.x;
        }
        else 
        {
            Renderer rend = GetComponent<Renderer>();
            if (rend != null) lengthX = rend.bounds.size.x;
        }
    }

    private void TryInitializeCamera()
    {
        if (cam == null)
        {
            if (AutoCameraSetup.Instance != null) cam = AutoCameraSetup.Instance.gameObject;
            else if (Camera.main != null) cam = Camera.main.gameObject;
        }

        if (cam != null)
        {
            camStartPosX = cam.transform.position.x;
            camStartPosY = cam.transform.position.y;
        }
    }

    void LateUpdate()
    {
        if (cam == null)
        {
            TryInitializeCamera();
            if (cam == null) return;
        }

        float camDeltaX = cam.transform.position.x - camStartPosX;
        float camDeltaY = cam.transform.position.y - camStartPosY;

        float posX = startPosX + (camDeltaX * parallaxFactorX);
        float posY = transform.position.y;
        
        if (followY)
        {
            posY = startPosY + (camDeltaY * parallaxFactorY);
        }

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (infiniteScrolling && lengthX > 0)
        {
            float relativeCamDist = cam.transform.position.x * (1 - parallaxFactorX);
            
            if (relativeCamDist > startPosX + lengthX)
            {
                startPosX += lengthX;
            }
            else if (relativeCamDist < startPosX - lengthX)
            {
                startPosX -= lengthX;
            }
        }
    }
}
