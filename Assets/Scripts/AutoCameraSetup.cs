using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class AutoCameraSetup : MonoBehaviour
{
    public static AutoCameraSetup Instance { get; private set; }
    private CinemachineCamera vcam;
    private bool targetFound = false;

    [Header("Responsive Ayarları")]
    [SerializeField] private float targetWidth = 20f;  
    [SerializeField] private float targetHeight = 11f; 
    [SerializeField] private bool autoResponsive = true;

    private void Awake()
    {
        Instance = this;
        vcam = GetComponent<CinemachineCamera>();
        if (vcam == null) Debug.LogError("Bu objede 'Cinemachine Camera' bileşeni bulunamadı!");
        
        if (Camera.main != null && Camera.main.TryGetComponent(out CinemachineBrain brain) == false)
        {
            Debug.LogError("DİKKAT: Main Camera üzerinde 'Cinemachine Brain' bileşeni eksik! Kamera çalışmayacaktır.");
        }
    }

    private void LateUpdate()
    {
        if (autoResponsive && vcam != null)
        {
            float currentAspect = (float)Screen.width / Screen.height;
            float orthoWidth = targetWidth / (currentAspect * 2f);
            float orthoHeight = targetHeight / 2f;
            
            var lens = vcam.Lens;
            lens.OrthographicSize = Mathf.Max(orthoWidth, orthoHeight);
            vcam.Lens = lens;
        }

        if (targetFound && (vcam == null || vcam.Follow == null))
        {
            targetFound = false;
        }

        if (targetFound) return;

        if (PlayerMovement2D.Instance != null)
        {
            vcam.Follow = PlayerMovement2D.Instance.transform;
            targetFound = true;
        }
    }
}
