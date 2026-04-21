using UnityEngine;
using System.Collections.Generic;

public class Level32CageButton : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float pressDepth = 0.2f;
    [SerializeField] private float cageRiseAmount = 3f;
    [SerializeField] private string targetCageName = "kafes";

    private Vector3 initialButtonPosition;
    private List<GameObject> detectedCages = new List<GameObject>();
    private List<Vector3> targetCagePositions = new List<Vector3>();
    private bool isActivated = false;

    private void Start()
    {
        initialButtonPosition = transform.position;

        GameObject[] sceneObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in sceneObjects)
        {
            if (obj.name.ToLower().Contains(targetCageName.ToLower()) && obj != gameObject)
            {
                detectedCages.Add(obj);
                targetCagePositions.Add(obj.transform.position);
            }
        }
    }

    private void Update()
    {
        if (isActivated)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialButtonPosition, movementSpeed * Time.deltaTime);
            
            for (int i = 0; i < detectedCages.Count; i++)
            {
                if (detectedCages[i] != null)
                {
                    detectedCages[i].transform.position = Vector3.MoveTowards(detectedCages[i].transform.position, targetCagePositions[i], movementSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            initialButtonPosition = transform.position + new Vector3(0, -pressDepth, 0);
            
            for (int i = 0; i < detectedCages.Count; i++)
            {
                if (detectedCages[i] != null)
                {
                    targetCagePositions[i] = detectedCages[i].transform.position + new Vector3(0, cageRiseAmount, 0);
                }
            }
        }
    }
}
