using UnityEngine;
using System.Collections.Generic;

public class TrapActivator : MonoBehaviour
{
    [Header("Tetiklenecek Nesneler")]
    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();

    [Header("Ayarlar")]
    [SerializeField] private bool disableOnStart = true;

    private bool hasTriggered = false;

    private void Start()
    {
        if (disableOnStart)
        {
            foreach (var obj in targetObjects)
            {
                if (obj != null) SetState(obj, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            foreach (var obj in targetObjects)
            {
                if (obj != null) SetState(obj, true);
            }
            Debug.Log("Tetikleyici: Nesnelerin collider ve renderlari aktif edildi!");
        }
    }

    private void SetState(GameObject obj, bool state)
    {
        // SpriteRenderer (Görsel) aç/kapat
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = state;

        // BÜTÜN Collider2D'leri (Polygon, Box, Sprite Collider vb.) aç/kapat
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            // Eğer objenin kendisi bir trigger ise onu kapatmayalım ki sistem çalışmaya devam etsin
            if (col.isTrigger && col.gameObject == gameObject) continue;
            
            col.enabled = state;
        }
    }
}
