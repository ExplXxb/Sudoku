using System;


namespace Unity.Services.LevelPlay
{
    sealed class UnsupportedBannerAd : IPlatformBannerAd
    {
        internal UnsupportedBannerAd(string adUnitId, LevelPlayAdSize size, LevelPlayBannerPosition position, string placementId)
        {
        }

        internal UnsupportedBannerAd(string adUnitId, Config config)
        {
        }
#pragma warning disable 0067
        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdExpanded;
        public event Action<LevelPlayAdInfo> OnAdCollapsed;
        public event Action<LevelPlayAdInfo> OnAdLeftApplication;
#pragma warning restore 0067

        public LevelPlayBannerPosition Position { get; }

        public void LoadAd()
        {
        }

        public void DestroyAd()
        {
        }

        public void ShowAd()
        {
        }

        public void HideAd()
        {
        }

        public void PauseAutoRefresh()
        {
        }

        public void ResumeAutoRefresh()
        {
        }

        public void SetAutoRefresh(bool flag)
        {
        }

        public void Dispose()
        {
        }

        public string AdId { get; }
        public string AdUnitId { get; }
        public LevelPlayAdSize AdSize { get; }
        public LevelPlayAdSize Size { get; }
        public string PlacementName { get; }

        internal class Config : IPlatformBannerAd.IConfig
        {
            internal class Builder : IPlatformBannerAd.IConfigBuilder
            {
                public void SetBidFloor(double bidFloor) {}

                public void SetSize(LevelPlayAdSize size) {}

                public void SetPosition(LevelPlayBannerPosition position) {}

                public void SetPlacementName(string placementName) {}

                public void SetDisplayOnLoad(bool displayOnLoad) {}

                public void SetRespectSafeArea(bool respectSafeArea) {}

                public IPlatformBannerAd.IConfig Build()
                {
                    return new Config();
                }
            }
        }
    }
}
