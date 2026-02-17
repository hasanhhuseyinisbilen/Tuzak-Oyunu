using UnityEngine;

public class BuzSarkiti : MonoBehaviour
{
    private Vector3 startPos;
    private bool hasFallen = false;
    public float minFallDistance = 0.5f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
       
        if (transform.position.y < startPos.y - minFallDistance)
        {
            hasFallen = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (hasFallen)
        {
            Destroy(gameObject);
        }
    }
}
