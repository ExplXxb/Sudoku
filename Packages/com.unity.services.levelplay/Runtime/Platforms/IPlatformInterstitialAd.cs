using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Unity.Services.LevelPlay.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Unity.Services.LevelPlay
{
    interface IPlatformInterstitialAd : IDisposable
    {
        event Action<LevelPlayAdInfo> OnAdLoaded;
        event Action<LevelPlayAdError> OnAdLoadFailed;
        event Action<LevelPlayAdInfo> OnAdDisplayed;
        event Action<LevelPlayAdInfo> OnAdClosed;
        event Action<LevelPlayAdInfo> OnAdClicked;
        event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        event Action<LevelPlayAdInfo> OnAdInfoChanged;

        string AdId { get; }
        string AdUnitId { get; }

        void LoadAd();

        void ShowAd(string placementName);

        bool IsAdReady();

        internal interface IConfig {}

        internal interface IConfigBuilder
        {
            void SetBidFloor(double bidFloor);

            IConfig Build();
        }
    }
}
