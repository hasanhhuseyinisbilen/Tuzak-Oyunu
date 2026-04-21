using UnityEngine;

public class KnifeThrower : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private float throwInterval = 2f;
    [SerializeField] private float throwRange = 30f; // Menzili genişlettik ki hemen atsın
    [SerializeField] private Transform throwPoint;

    private Transform player;
    private float throwTimer;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        // İlk atış için zamanlayıcıyı hazırla
        throwTimer = throwInterval;
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                return;
            }
        }

        float distance = Vector2.Distance(transform.position, player.position);
      
        
        if (distance <= throwRange)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer <= 0f)
            {
                ThrowKnife();
                throwTimer = throwInterval;
            }
        }
    }

    private void ThrowKnife()
    {
        if (knifePrefab != null)
        {

            Vector3 spawnPoint = throwPoint != null ? throwPoint.position : transform.position;
            GameObject knife = Instantiate(knifePrefab, spawnPoint, Quaternion.identity);
            
            KnifeProjectile proj = knife.GetComponent<KnifeProjectile>();
            if (proj != null)
            {
                Vector2 dir = (player.position - spawnPoint).normalized;
                proj.SetDirection(dir);
            }
          
        }
    }
}
