using UnityEngine;
using System.Collections.Generic;

public class BoxDestroyTrigger : MonoBehaviour
{
    public List<GameObject> boxesToDestroy = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject box in boxesToDestroy)
            {
                if (box != null)
                {
                    Destroy(box);
                }
            }
        }
    }
}
