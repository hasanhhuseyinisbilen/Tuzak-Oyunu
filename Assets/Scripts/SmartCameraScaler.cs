using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Cinemachine kamerasını tüm ekran oranlarına (mobile) tam uyumlu hale getirir.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(CinemachineCamera))]
public class SmartCameraScaler : MonoBehaviour
{
    [Header("Hedef Alan (Units)")]
    public float targetWidth = 18f;
    public float targetHeight = 10f;

    private CinemachineCamera _vcam;

    void Awake()
    {
        _vcam = GetComponent<CinemachineCamera>();
    }

    void LateUpdate()
    {
        if (_vcam == null) _vcam = GetComponent<CinemachineCamera>();
        if (_vcam == null) return;

        ApplyScaling();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        ApplyScaling();
    }
#endif

    private void ApplyScaling()
    {
        if (_vcam == null) _vcam = GetComponent<CinemachineCamera>();
        if (_vcam == null || Camera.main == null) return;

        // 1. Ekranın gerçek oranını al (Unity Game View veya Telefon Ekranı)
        float screenAspect = Camera.main.aspect;
        float targetAspect = targetWidth / targetHeight;
        if (screenAspect <= 0) return;

        // 2. Hem Genişliği hem Yüksekliği kurtaracak Ortho Size değerlerini hesapla
        // OrthoSize = Yüksekliğin yarısıdır.
        
        // Yükseklik (Height) için gereken: 
        float orthoHeight = targetHeight / 2f;
        
        // Genişlik (Width) için gereken: (Genişlik / EkranOranı) / 2
        float orthoWidth = (targetWidth / screenAspect) / 2f;

        // 3. İkisinin de ekrana sığması için "Mecburen" büyük olanı seçiyoruz.
        // Bu sayede hem iPad gibi kare ekranlarda hem de uzun ince telefonlarda 
        // senin o seçtiğin "Safe Area" (Kutu) asla kesilmez, tam sığar.
        float finalOrthoSize = Mathf.Max(orthoWidth, orthoHeight);

        // 4. Değeri uygula (Anlamsız küçük oynamaları titreme yapmasın diye filtrele)
        if (Mathf.Abs(_vcam.Lens.OrthographicSize - finalOrthoSize) > 0.0001f)
        {
            var lens = _vcam.Lens;
            lens.OrthographicSize = finalOrthoSize;
            _vcam.Lens = lens;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        // Kutu her zaman kameranın baktığı merkeze çizilir
        Gizmos.DrawWireCube(transform.position, new Vector3(targetWidth, targetHeight, 0));
    }
}
