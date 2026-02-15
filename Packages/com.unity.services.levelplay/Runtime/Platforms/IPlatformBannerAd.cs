using System;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Interface of a Banner Ad.
    /// </summary>
    interface IPlatformBannerAd : IDisposable
    {
        event Action<LevelPlayAdInfo> OnAdLoaded;
        event Action<LevelPlayAdError> OnAdLoadFailed;
        event Action<LevelPlayAdInfo> OnAdClicked;
        event Action<LevelPlayAdInfo> OnAdDisplayed;
        event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        event Action<LevelPlayAdInfo> OnAdExpanded;
        event Action<LevelPlayAdInfo> OnAdCollapsed;
        event Action<LevelPlayAdInfo> OnAdLeftApplication;

        string AdId { get; }
        string AdUnitId { get; }
        LevelPlayAdSize AdSize { get; }
        string PlacementName { get; }
        LevelPlayBannerPosition Position { get; }

        void LoadAd();
        void DestroyAd();
        void ShowAd();
        void HideAd();
        void PauseAutoRefresh();
        void ResumeAutoRefresh();

        internal interface IConfig
        {
        }

        internal interface IConfigBuilder
        {
            void SetBidFloor(double bidFloor);

            void SetSize(LevelPlayAdSize size);

            void SetPosition(LevelPlayBannerPosition position);

            void SetPlacementName(string placementName);

            void SetDisplayOnLoad(bool displayOnLoad);

            void SetRespectSafeArea(bool respectSafeArea);

            IConfig Build();
        }
    }
}
