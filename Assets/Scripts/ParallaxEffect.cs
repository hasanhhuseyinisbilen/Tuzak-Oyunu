using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float parallaxFactor; // 0: Hiç hareket etmez, 1: Kamera ile aynı hızda (sabit durur gibi)

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<Renderer>().bounds.size.x;
        if (cam == null) cam = Camera.main.gameObject;
    }

    void Update()
    {
        float dist = (cam.transform.position.x * parallaxFactor);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
