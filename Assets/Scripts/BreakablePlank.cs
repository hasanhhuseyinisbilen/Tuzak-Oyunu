using UnityEngine;
using System.Collections;

public class BreakablePlank : MonoBehaviour
{
    [Tooltip("Tahtanın kırılması için geçecek süre (saniye)")]
    public float breakDelay = 1f;
    
    private bool isBreaking = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && !isBreaking)
        {
            StartCoroutine(BreakRoutine());
        }
    }

    private IEnumerator BreakRoutine()
    {
        isBreaking = true;
        
     
        yield return new WaitForSeconds(breakDelay);

        Destroy(gameObject);
    }
}
