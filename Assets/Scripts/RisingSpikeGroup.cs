using UnityEngine;
using System.Collections;

public class RisingSpikeGroup : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] private GameObject[] spikes;
    [SerializeField] private int[] sequence = { 0, 1, 3, 4, 2, 6, 7, 5, 8, 9 };
    [SerializeField] private float delayBetweenSpikes = 0.3f;
    [SerializeField] private float activeHoldTime = 1.0f;
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private bool loop = true;

    [Header("Position Settings")]
    [SerializeField] private float idleY = -1.0f;
    [SerializeField] private float activeY = 0.7f;

    private bool isRunning = false;

    void Start()
    {
        foreach (var spike in spikes)
        {
            if (spike != null)
            {
                Vector3 pos = spike.transform.localPosition;
                pos.y = idleY;
                spike.transform.localPosition = pos;
            }
        }

        StartSequence();
    }

    public void StartSequence()
    {
        if (!isRunning)
        {
            StartCoroutine(SequenceCycle());
        }
    }

    private IEnumerator SequenceCycle()
    {
        isRunning = true;

        do
        {
            foreach (int index in sequence)
            {
                if (index >= 0 && index < spikes.Length && spikes[index] != null)
                {
                    StartCoroutine(SpikeActionRoutine(spikes[index]));
                }
                yield return new WaitForSeconds(delayBetweenSpikes);
            }

            if (loop) yield return new WaitForSeconds(activeHoldTime + 1f);

        } while (loop);

        isRunning = false;
    }

    private IEnumerator SpikeActionRoutine(GameObject spike)
    {
        yield return StartCoroutine(MoveToY(spike, activeY));

        yield return new WaitForSeconds(activeHoldTime);

        yield return StartCoroutine(MoveToY(spike, idleY));
    }

    private IEnumerator MoveToY(GameObject obj, float targetY)
    {
        if (obj == null) yield break;

        Vector3 targetPos = new Vector3(obj.transform.localPosition.x, targetY, obj.transform.localPosition.z);
        
        while (Vector3.Distance(obj.transform.localPosition, targetPos) > 0.001f)
        {
            obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        obj.transform.localPosition = targetPos;
    }
}
