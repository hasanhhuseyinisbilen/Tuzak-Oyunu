using UnityEngine;
using System.Collections;

public class TimedSmokeTrap : MonoBehaviour
{
    private ParticleSystem ps;

    [Header("Zaman Ayarları")]
    [Tooltip("Dumanın aktif kalacağı süre (Saniye)")]
    [SerializeField] private float onDuration = 3f;
    
    [Tooltip("Dumanın kapalı kalacağı süre (Saniye)")]
    [SerializeField] private float offDuration = 3f;

    [Header("Debug Bilgisi")]
    [SerializeField] private bool startsOn = true;

    private Collider2D col;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        col = GetComponent<Collider2D>();
        
        if (ps != null)
        {
            StartCoroutine(SmokeCycleRoutine());
        }
        else
        {
            Debug.LogWarning("TimedSmokeTrap: Bu objede ParticleSystem bulunamadı!");
        }
    }

    private IEnumerator SmokeCycleRoutine()
    {
        if (!startsOn)
        {
            ps.Stop();
            if (col != null) col.enabled = false;
            yield return new WaitForSeconds(offDuration);
        }

        while (true)
        {
            ps.Play();
            if (col != null) col.enabled = true;
            yield return new WaitForSeconds(onDuration);

            ps.Stop();
            if (col != null) col.enabled = false;
            yield return new WaitForSeconds(offDuration);
        }
    }
}
