using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DusenZemin : MonoBehaviour
{
    public void StartFalling()
    {
        Debug.Log(gameObject.name + " anında yok edildi!");
        Destroy(gameObject);
    }
}
