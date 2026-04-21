using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    
    [Header("Ayarlar")]
    [SerializeField] private bool _useTestAds = true;
    
    [Header("Gerçek Unit ID'ler")]
    [SerializeField] private string _bannerAdUnitId = "ca-app-pub-3651694872121645/3563184216";
    [SerializeField] private string _rewardedAdUnitId = "ca-app-pub-3651694872121645/6345250482";

    // Google'ın standart test ID'leri
    private const string TEST_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    private const string TEST_REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";

    private BannerView _bannerView;
    private RewardedAd _rewardedAd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        try
        {
            MobileAds.Initialize(initStatus =>
            {
                Debug.Log("AdsManager: AdMob başlatıldı.");
                LoadBanner();
                LoadRewardedAd();
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("AdsManager Başlatma Hatası: " + ex.Message);
        }
    }

    #region Rewarded Ad Logic

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        string adUnitId = _useTestAds ? TEST_REWARDED_ID : _rewardedAdUnitId;
        Debug.Log("AdsManager: Ödüllü reklam yükleniyor... ID: " + adUnitId);

        var adRequest = new AdRequest();
        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("AdsManager: Reklam Yükleme HATASI: " + error?.GetMessage());
                return;
            }

            Debug.Log("AdsManager: Ödüllü reklam başarıyla yüklendi.");
            _rewardedAd = ad;
            
            // Reklam kapatıldığında yenisini yükle
            _rewardedAd.OnAdFullScreenContentClosed += () => 
            {
                Debug.Log("AdsManager: Reklam kapatıldı, yenisi isteniyor.");
                LoadRewardedAd();
            };

            // Reklam gösterilemediğinde (hata durumunda) yenisini yükle
            _rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("AdsManager: Reklam Gösterim HATASI: " + adError.GetMessage());
                LoadRewardedAd();
            };
        });
    }

    public void ShowRewardedAd(Action onComplete)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                onComplete?.Invoke();
            });
        }
        else
        {
            LoadRewardedAd();
        }
    }

    public bool IsRewardedAdReady()
    {
        return _rewardedAd != null && _rewardedAd.CanShowAd();
    }

    #endregion

    #region Banner Logic

    public void LoadBanner()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        string adUnitId = _useTestAds ? TEST_BANNER_ID : _bannerAdUnitId;

        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }

    #endregion

    private void OnDestroy()
    {
        if (_bannerView != null) _bannerView.Destroy();
        if (_rewardedAd != null) _rewardedAd.Destroy();
    }
}
