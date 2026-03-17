using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class AutoCameraSetup : MonoBehaviour
{
    private CinemachineCamera vcam;
    private bool targetFound = false;

    [Header("Responsive Ayarları")]
    [SerializeField] private float targetWidth = 20f;  // Minimum yatay birim
    [SerializeField] private float targetHeight = 11f; // Minimum dikey birim
    [SerializeField] private bool autoResponsive = true;

    private void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
        if (vcam == null) Debug.LogError("DİKKAT: Bu objede 'Cinemachine Camera' bileşeni bulunamadı!");
        
        // Sahne kontrolü: Main Camera'da Cinemachine Brain var mı?
        if (Camera.main != null && Camera.main.TryGetComponent(out CinemachineBrain brain) == false)
        {
            Debug.LogError("DİKKAT: Main Camera üzerinde 'Cinemachine Brain' bileşeni eksik! Kamera çalışmayacaktır.");
        }
    }

    private void LateUpdate()
    {
        // Responsive Ayarı: Hem Genişlik Hem Yükseklik Koruması
        if (autoResponsive && vcam != null)
        {
            float currentAspect = (float)Screen.width / Screen.height;
            
            // Genişliği sığdırmak için gereken boyut
            float orthoWidth = targetWidth / (currentAspect * 2f);
            // Yüksekliği sığdırmak için gereken boyut
            float orthoHeight = targetHeight / 2f;
            
            // İkisinden hangisi daha çok alan gerektiriyorsa onu seç (Best Fit)
            var lens = vcam.Lens;
            lens.OrthographicSize = Mathf.Max(orthoWidth, orthoHeight);
            vcam.Lens = lens;
        }

        if (targetFound) return;

        // 1. Önce Tag ile ara
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        // 2. Fallback: PlayerMovement2D script'i ile ara
        if (player == null)
        {
            var pm = Object.FindFirstObjectByType<PlayerMovement2D>();
            if (pm != null) player = pm.gameObject;
        }

        if (player != null)
        {
            vcam.Follow = player.transform;
            targetFound = true;
            Debug.Log("Cinemachine: Oyuncu (" + player.name + ") başarıyla bulundu ve bağlandı!");
        }
    }
}
