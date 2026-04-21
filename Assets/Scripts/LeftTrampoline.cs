using UnityEngine;

public class LeftTrampoline : MonoBehaviour
{
    [SerializeField] private float launchForceX = -20f;
    [SerializeField] private float scaleAmount = 0.8f;
    [SerializeField] private float scaleDuration = 0.1f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement2D move = collision.gameObject.GetComponent<PlayerMovement2D>();
            if (move != null)
            {
                move.ApplyLaunch(new Vector2(launchForceX, 0f), 0.3f);
                StopAllCoroutines();
                StartCoroutine(BounceEffect());
            }
        }
    }

    private System.Collections.IEnumerator BounceEffect()
    {
        transform.localScale = new Vector3(originalScale.x * scaleAmount, originalScale.y, originalScale.z);
        yield return new WaitForSeconds(scaleDuration);
        transform.localScale = originalScale;
    }
}
