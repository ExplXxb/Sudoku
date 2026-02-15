using System;

namespace Unity.Services.LevelPlay
{
    sealed class UnsupportedInterstitialAd : IPlatformInterstitialAd
    {
        public void Dispose()
        {
        }
#pragma warning disable 0067
        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo> OnAdClosed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;
#pragma warning restore 0067

        public string AdId { get; }
        public string AdUnitId { get; }

        public UnsupportedInterstitialAd(string adUnitId)
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

        internal class Config : IPlatformInterstitialAd.IConfig
        {
            internal class Builder : IPlatformInterstitialAd.IConfigBuilder
            {
                public void SetBidFloor(double bidFloor)
                {
                    // no-op
                }

                public IPlatformInterstitialAd.IConfig Build()
                {
                    return new Config();
                }
            }
        }
    }
}
