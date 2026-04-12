using GoogleMobileAds.Api;
using GoogleMobileAds.Api.AdManager;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

#if UNITY_ANDROID
    private const string BANNER_ID = "ca-app-pub-3940256099942544/9214589741";
    private const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    private const string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private const string BANNER_ID = "ca-app-pub-3940256099942544/2435281174";
    private const string INTERSTITIAL_ID = "ca-app-pub-3940256099942544/4411468910";
    private const string REWARDED_ID = "ca-app-pub-3940256099942544/1712485313";
#else
    private const string BANNER_ID = "unused";
    private const string INTERSTITIAL_ID = "unused";
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


    public void Start()
    {
        // Initialize Google Mobile Ads Unity Plugin.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        CreateAnchoredAdaptiveBannerView();
        LoadBannerView();
        LoadRewardedAd();

    }

    private void CreateBannerView()
    {
        // [START create_banner_view]
        // Create a 320x50 banner at top of the screen.
        bannerView = new BannerView(BANNER_ID, AdSize.Banner, AdPosition.Top);
        // [END create_banner_view]
    }

    private void CreateBannerViewWithCustomPostition()
    {
        // [START create_banner_view_position]
        // Create a 320x50 banner views at coordinate (0,50) on screen.
        bannerView = new BannerView(BANNER_ID, AdSize.Banner, 0, 50);
        // [END create_banner_view_position]
    }

    private void CreateBannerViewWithCustomSize()
    {
        // [START create_banner_view_size]
        // Create a 250x250 banner at the bottom of the screen.
        AdSize adSize = new AdSize(250, 250);
        bannerView = new BannerView(BANNER_ID, adSize, AdPosition.Bottom);
        // [END create_banner_view_size]
    }

    private void CreateAdManagerBannerViewWithCustomSizes(
        AdManagerBannerView adManagerBannerView)
    {
        // [START create_ad_manager_banner_view_sizes]
        // Create a 250x250 banner at the bottom of the screen.
        adManagerBannerView = new AdManagerBannerView(BANNER_ID, AdSize.Banner, AdPosition.Top);

        // Add multiple ad sizes.
        adManagerBannerView.ValidAdSizes = new List<AdSize>
            {
                AdSize.Banner,
                new AdSize(120, 20),
                new AdSize(250, 250),
            };
        // [END create_ad_manager_banner_view_sizes]
    }

    private void LoadBannerView()
    {
        // [START load_banner_view]
        // Send a request to load an ad into the banner view.
        bannerView.LoadAd(new AdRequest());
        // [END load_banner_view]
    }

    private void ListenToBannerViewEvents()
    {
        // [START listen_to_events]
        bannerView.OnBannerAdLoaded += () =>
        {
            // Raised when an ad is loaded into the banner view.
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            // Raised when an ad fails to load into the banner view.
        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        bannerView.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            // Raised when an ad opened full screen content.
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
        };
        // [END listen_to_events]
    }

    private void DestroyBannerView()
    {
        // [START destroy_banner_view]
        if (bannerView != null)
        {
            // Always destroy the banner view when no longer needed.
            bannerView.Destroy();
            bannerView = null;
        }
        // [END destroy_banner_view]
    }

    private void CreateAnchoredAdaptiveBannerView()
    {
        // [START create_anchored_adaptive_banner_view]
        // Get the device safe width in density-independent pixels.
        int deviceWidth = MobileAds.Utils.GetDeviceSafeWidth();

        // Define the anchored adaptive ad size.
        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth);

        // Create an anchored adaptive banner view.
        bannerView = new BannerView(BANNER_ID, adaptiveSize, AdPosition.Top);
        // [END create_anchored_adaptive_banner_view]
    }

    void LoadRewardedAd()
    {
        var adRequest = new AdRequest();

        RewardedAd.Load(REWARDED_ID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Rewarded failed to load: " + error);
                return;
            }

            rewardedAd = ad;

            Debug.Log("Rewarded loaded");

            ListenToAdEvents(rewardedAd);

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                LoadRewardedAd();
            };
        });
    }


    void ServerSideVerification(RewardedAd rewardedAd)
    {
        // [START ssv]
        // Create and pass the SSV options to the rewarded ad.
        var options = new ServerSideVerificationOptions
        {
            CustomData = "SAMPLE_CUSTOM_DATA_STRING"
        };

        rewardedAd.SetServerSideVerificationOptions(options);
        // [END ssv]
    }

    void ShowAd(RewardedAd rewardedAd)
    {
        // [START show_ad]
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // The ad was showen and the user earned a reward.
            });
        }
        // [END show_ad]]
    }

    void ListenToAdEvents(RewardedAd rewardedAd)
    {
        // [START ad_events]
        rewardedAd.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        rewardedAd.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        rewardedAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
        };
        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Raised when the ad failed to open full screen content.
        };
        // [END ad_events]]
    }

    void DestroyAd(RewardedAd rewardedAd)
    {
        // [START destroy_ad]
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
        // [END destroy_ad]]
    }

    void ReloadAd(RewardedAd rewardedAd)
    {
        // [START reload_ad]
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            // Reload the ad so that we can show another as soon as possible.
            var adRequest = new AdRequest();
            RewardedAd.Load(REWARDED_ID, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // Handle ad loading here.
            });
        };
        // [END reload_ad]]
    }

    public void ShowRewarded(System.Action rewardCallback)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Reward earned");

                rewardCallback?.Invoke();
            });
        }
        else
        {
            Debug.Log("Ad not ready yet");
        }
    }

}
