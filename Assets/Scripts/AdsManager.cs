using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    private BannerView _bannerView;
#if UNITY_ANDROID
    private string _bannerAdUnitId = "ca-app-pub-3651694872121645/3563184216"; 
#else
    private string _bannerAdUnitId = "unused";
#endif
    private void Start()
    {
        MobileAds.Initialize(initStatus => {
            LoadBanner();
        });
    }

    public void LoadBanner()
    {
        _bannerView = new BannerView(_bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest adRequest = new AdRequest();

        _bannerView.LoadAd(adRequest);
    }

}

