using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequentialComponentActivator : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();
    [SerializeField] private float delayBetweenObjects = 0.5f;
    [SerializeField] private bool activateOnStart = false;

    private bool hasTriggered = false;

    private void Start()
    {
        // Başlangıçta her şeyi kapat
        foreach (var obj in targetObjects)
        {
            if (obj != null) SetComponentsActive(obj, false);
        }

        if (activateOnStart) StartActivation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            StartActivation();
        }
    }

    public void StartActivation()
    {
        if (hasTriggered) return;
        hasTriggered = true;
        StartCoroutine(ActivationRoutine());
    }

    private IEnumerator ActivationRoutine()
    {
        foreach (var obj in targetObjects)
        {
            if (obj != null)
            {
                SetComponentsActive(obj, true);
                Debug.Log(obj.name + " aktifleşti!");
                yield return new WaitForSeconds(delayBetweenObjects);
            }
        }
    }

    private void SetComponentsActive(GameObject obj, bool state)
    {
        // SpriteRenderer kontrolü
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = state;

        // Collider kontrolü (Box, Polygon, Circle vb. hepsini kapsar)
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            // Eğer objenin kendisi bir Trigger ise (tespit için), onu kapatma
            if (col.isTrigger && !state) continue; 
            col.enabled = state;
        }
    }

    // Reset gerekirse
    public void ResetActivator()
    {
        hasTriggered = false;
        StopAllCoroutines();
        foreach (var obj in targetObjects)
        {
            if (obj != null) SetComponentsActive(obj, false);
        }
    }
}
