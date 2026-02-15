using System;

namespace Unity.Services.LevelPlay
{
    sealed class UnsupportedRewardedAd : IPlatformRewardedAd
    {
        public void Dispose()
        {
        }
#pragma warning disable 0067
        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdClosed;
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;
#pragma warning restore 0067

        public string AdId { get; }
        public string AdUnitId { get; }

        public UnsupportedRewardedAd(string adUnitId)
        {
        }

        public void LoadAd()
        {
        }

        public void ShowAd(string placementName)
        {
        }

        public bool IsAdReady()
        {
            return false;
        }

        public LevelPlayReward GetReward(string placement)
        {
            return LevelPlayReward.Default;
        }

        internal class Config : IPlatformRewardedAd.IConfig
        {
            internal class Builder : IPlatformRewardedAd.IConfigBuilder
            {
                public void SetBidFloor(double bidFloor) {}

                public IPlatformRewardedAd.IConfig Build()
                {
                    return new Config();
                }
            }
        }
    }
}
