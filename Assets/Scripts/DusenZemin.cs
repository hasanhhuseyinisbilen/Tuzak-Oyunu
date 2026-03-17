using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DusenZemin : MonoBehaviour
{
    public void StartFalling()
    {
        // Karmaşaya gerek yok, direkt yok et gitsin
        Debug.Log(gameObject.name + " anında yok edildi!");
        Destroy(gameObject);
    }
}
