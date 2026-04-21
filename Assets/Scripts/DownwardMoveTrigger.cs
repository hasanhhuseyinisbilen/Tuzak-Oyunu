using UnityEngine;
using System.Collections.Generic;

public class DownwardMoveTrigger : MonoBehaviour
{
    public List<GameObject> targetObjects = new List<GameObject>();

    private void Start()
    {
       
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null) obj.GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null) obj.GetComponent<Rigidbody2D>().simulated = true;
            }
        }
    }
}
