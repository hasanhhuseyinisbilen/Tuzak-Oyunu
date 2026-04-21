using System.Collections.Generic;
using UnityEngine;

public class BoxFallTrigger : MonoBehaviour
{
    [Header("Düşecek Objeler")]
    [SerializeField] private FallingBox boxToFall;
    [SerializeField] private FallingBox[] boxesToFall;
    [SerializeField] private BuzSarkiti spikeToFall;
    [SerializeField] private BuzSarkiti[] spikesToFallArray;

    private List<FallingBox> registeredBoxes = new List<FallingBox>();
    private bool isTriggered = false;

    public void RegisterBox(FallingBox box)
    {
        if (box != null && !registeredBoxes.Contains(box))
        {
            registeredBoxes.Add(box);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        
        bool isPlayer = other.CompareTag("Player") || other.gameObject.name.ToLower().Contains("player");
        if (!isPlayer) return;

        isTriggered = true;
        TriggerFall();
    }

    private void TriggerFall()
    {
        if (boxToFall != null) boxToFall.StartFalling();
        if (boxesToFall != null)
        {
            foreach (var box in boxesToFall) if (box != null) box.StartFalling();
        }

        if (spikeToFall != null) spikeToFall.StartFalling();
        if (spikesToFallArray != null)
        {
            foreach (var spike in spikesToFallArray) if (spike != null) spike.StartFalling();
        }

        foreach (var box in registeredBoxes)
        {
            if (box != null) box.StartFalling();
        }
    }
}
