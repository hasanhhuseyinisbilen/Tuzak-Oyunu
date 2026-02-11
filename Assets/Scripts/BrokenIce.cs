using UnityEngine;
using System.Collections;

public class IceBreak : MonoBehaviour
{
    public float breakTime = 2f;

    private SpriteRenderer sr;
    private Collider2D col;
    private bool triggered = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;
        if (!collision.collider.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(BreakAfterTime());
    }

    private IEnumerator BreakAfterTime()
    {
        yield return new WaitForSeconds(breakTime);

        sr.enabled = false;
        col.enabled = false;
    }
}
