using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    [Header("Düşecek Diken Ayarları")]
    [SerializeField] private BuzSarkiti spikeToFall;
    [SerializeField] private BuzSarkiti[] spikesToFall;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // Tekli diken düşürme
        if (spikeToFall != null)
        {
            spikeToFall.StartFalling();
        }

        // Çoklu diken düşürme (eğer diziye ekleme yapıldıysa)
        if (spikesToFall != null && spikesToFall.Length > 0)
        {
            foreach (var spike in spikesToFall)
            {
                if (spike != null)
                {
                    spike.StartFalling();
                }
            }
        }
    }
}
