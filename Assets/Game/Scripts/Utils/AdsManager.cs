using GoogleMobileAds.Api;
using GoogleMobileAds.Api.AdManager;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private BannerView bannerView;
    private RewardedAd rewardedAd;

#if UNITY_ANDROID
    private const string BANNER_ID = "ca-app-pub-3940256099942544/9214589741";
    private const string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";
#else
    private const string BANNER_ID = "unused";
    private const string REWARDED_ID = "unused";
#endif

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        MobileAds.Initialize(_ => { });

        CreateBanner();
        LoadRewardedAd();
    }

    private void CreateBanner()
    {
        int width = MobileAds.Utils.GetDeviceSafeWidth();
        AdSize size = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width);

        bannerView = new BannerView(BANNER_ID, size, AdPosition.Top);
        bannerView.LoadAd(new AdRequest());
    }

    private void LoadRewardedAd()
    {
        var request = new AdRequest();

        RewardedAd.Load(REWARDED_ID, request, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Rewarded failed: " + error);
                return;
            }

            rewardedAd = ad;

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                LoadRewardedAd();
            };
        });
    }

    public void ShowRewarded(System.Action onReward)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(_ =>
            {
                Debug.Log("Reward earned");
                onReward?.Invoke();
            });
        }
        else
        {
            Debug.Log("Rewarded not ready");
        }
    }
}
