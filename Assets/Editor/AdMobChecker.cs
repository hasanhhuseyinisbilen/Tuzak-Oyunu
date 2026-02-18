#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

[InitializeOnLoad]
public class AdMobChecker
{
    static AdMobChecker()
    {
        // Delay checking to ensure compilation is finished
        EditorApplication.delayCall += CheckAdMob;
    }

    static void CheckAdMob()
    {
        Debug.Log("--------------------------------------------------");
        Debug.Log("[AdMobChecker] Kontrol Başlatılıyor...");

        // Try to find the GoogleMobileAds assembly
        bool found = false;
        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.GetName().Name.Contains("GoogleMobileAds"))
            {
                Debug.Log($"[AdMobChecker] ✅ GoogleMobileAds Kütüphanesi Bulundu: {assembly.FullName}");
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogError("[AdMobChecker] ❌ GoogleMobileAds Kütüphanesi BULUNAMADI! Plugin yüklü değil veya hatalı.");
        }
        else
        {
            Debug.Log("[AdMobChecker] ✅ AdMob Plugin yüklü görünüyor.");
            Debug.Log("[AdMobChecker] Lütfen üst menüde 'Assets > Google Mobile Ads > Settings' yolunu kontrol edin.");
        }
        Debug.Log("--------------------------------------------------");
    }
}
#endif
