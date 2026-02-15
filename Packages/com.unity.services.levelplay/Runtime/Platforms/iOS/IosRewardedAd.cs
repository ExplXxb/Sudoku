#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Unity.Services.LevelPlay
{
    sealed class IosRewardedAd : IosNativeObject, IPlatformRewardedAd
    {
        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdClosed;
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;

        public string AdUnitId { get; }
        public string AdId => GetAdId();

        IosRewardedAdListener m_RewardedAdListener;

        public IosRewardedAd(string adUnitId, Config config) : base(true)
        {
            AdUnitId = adUnitId;
            NativePtr = RewardedAdCreate(adUnitId, config.IosConfig);
            m_RewardedAdListener = new IosRewardedAdListener(this);
            RewardedAdSetDelegate(NativePtr, m_RewardedAdListener.NativePtr);
        }

        public void LoadAd()
        {
            if (CheckDisposedAndLogError("Cannot Load Rewarded Ad")) return;
            RewardedAdLoadAd(NativePtr);
        }

        public void ShowAd(string placementName)
        {
            if (CheckDisposedAndLogError("Cannot Show Rewarded Ad")) return;
            RewardedAdShowAd(NativePtr, placementName);
        }

        public bool IsAdReady()
        {
            if (CheckDisposedAndLogError("Cannot Check if Rewarded Ad is Ready")) return false;
            return RewardedAdIsAdReady(NativePtr);
        }

        public static bool IsPlacementCapped(string placementName)
        {
            return RewardedAdIsPlacementCapped(placementName);
        }

        public LevelPlayReward GetReward(string placement)
        {
            if (CheckDisposedAndLogError("Cannot Get Reward, instance is disposed")) return LevelPlayReward.Default;

            var rewardPtr = RewardedAdGetReward(NativePtr, placement);
            if (rewardPtr == IntPtr.Zero) return LevelPlayReward.Default;

            var rewardName = RewardedAdRewardGetName(rewardPtr) ?? string.Empty;
            var rewardAmount = RewardedAdRewardGetAmount(rewardPtr);
            return new LevelPlayReward(rewardName, rewardAmount);
        }

        public override void Dispose()
        {
            m_RewardedAdListener?.Dispose();
            m_RewardedAdListener = null;
            base.Dispose();
        }

        internal void InvokeLoadedEvent(string adInfo)
        {
            ThreadUtil.Post(_ => OnAdLoaded?.Invoke(new LevelPlayAdInfo(adInfo)));
        }

        internal void InvokeFailedLoadEvent(string error)
        {
            ThreadUtil.Post(_ => OnAdLoadFailed?.Invoke(new LevelPlayAdError(error)));
        }

        internal void InvokeDisplayedEvent(string adInfo)
        {
            ThreadUtil.Post(_ => OnAdDisplayed?.Invoke(new LevelPlayAdInfo(adInfo)));
        }

        internal void InvokeFailedDisplayEvent(string adInfo, string error)
        {
            ThreadUtil.Post(_ => OnAdDisplayFailed?.Invoke(new LevelPlayAdInfo(adInfo), new LevelPlayAdError(error)));
        }

        internal void InvokeRewardedEvent(string adInfo, string rewardName, int rewardAmount)
        {
            ThreadUtil.Post(_ => OnAdRewarded?.Invoke(new LevelPlayAdInfo(adInfo), new LevelPlayReward(rewardName, rewardAmount)));
        }

        internal void InvokeClickedEvent(string adInfo)
        {
            ThreadUtil.Post(_ => OnAdClicked?.Invoke(new LevelPlayAdInfo(adInfo)));
        }

        internal void InvokeClosedEvent(string adInfo)
        {
            ThreadUtil.Post(_ => OnAdClosed?.Invoke(new LevelPlayAdInfo(adInfo)));
        }

        internal void InvokeOnAdInfoChangedEvent(string adInfo)
        {
            ThreadUtil.Post(_ => OnAdInfoChanged?.Invoke(new LevelPlayAdInfo(adInfo)));
        }

        private string GetAdId()
        {
            if (CheckDisposedAndLogError("Cannot get Rewarded ad Id")) return "";
            return RewardedAdId(NativePtr);
        }

        ~IosRewardedAd()
        {
            Dispose();
        }

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdCreateWithConfig")]
        static extern IntPtr RewardedAdCreate(string adUnitId, IntPtr config);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdSetDelegate")]
        static extern void RewardedAdSetDelegate(IntPtr rewardedAd, IntPtr rewardedAdListener);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdLoadAd")]
        static extern void RewardedAdLoadAd(IntPtr rewardedAd);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdShowAd")]
        static extern void RewardedAdShowAd(IntPtr rewardedAd, string placementName);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdIsAdReady")]
        static extern bool RewardedAdIsAdReady(IntPtr rewardedAd);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdIsPlacementCapped")]
        static extern bool RewardedAdIsPlacementCapped(string placementName);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdAdId")]
        static extern string RewardedAdId(IntPtr rewardedAd);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdGetReward")]
        static extern IntPtr RewardedAdGetReward(IntPtr rewardedAd, string placement);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdRewardGetName")]
        static extern string RewardedAdRewardGetName(IntPtr reward);

        [DllImport("__Internal", EntryPoint = "LPMRewardedAdRewardGetAmount")]
        static extern int RewardedAdRewardGetAmount(IntPtr reward);

        internal class Config : IPlatformRewardedAd.IConfig
        {
            internal IntPtr IosConfig { get; }

            private Config(IntPtr iosConfig)
            {
                IosConfig = iosConfig;
            }

            internal class Builder : IPlatformRewardedAd.IConfigBuilder
            {
                private readonly IntPtr m_Builder = RewardedAdCreateConfigBuilder();

                public void SetBidFloor(double bidFloor)
                {
                    RewardedConfigBuilderSetBidFloor(m_Builder, bidFloor);
                }

                public IPlatformRewardedAd.IConfig Build()
                {
                    var iosConfig = RewardedConfigBuilderBuild(m_Builder);
                    return new Config(iosConfig);
                }

                [DllImport("__Internal", EntryPoint = "LPMRewardedAdCreateConfigBuilder")]
                static extern IntPtr RewardedAdCreateConfigBuilder();

                [DllImport("__Internal", EntryPoint = "LPMRewardedAdConfigBuilderSetBidFloor")]
                static extern IntPtr RewardedConfigBuilderSetBidFloor(IntPtr builder, double bidFloor);

                [DllImport("__Internal", EntryPoint = "LPMRewardedAdConfigBuilderBuild")]
                static extern IntPtr RewardedConfigBuilderBuild(IntPtr builder);
            }
        }
    }
}
#endif
