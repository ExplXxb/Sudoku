using System;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Implements ILevelPlayBannerAd to provide functionality for managing banner ads.
    /// </summary>
    public sealed class LevelPlayBannerAd : ILevelPlayBannerAd
    {
        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdExpanded;
        public event Action<LevelPlayAdInfo> OnAdCollapsed;
        public event Action<LevelPlayAdInfo> OnAdLeftApplication;

        bool m_autoRefresh;
        readonly IPlatformBannerAd m_BannerAd;

        /// <summary>
        /// Initializes a new instance of the LevelPlayBannerAd with a config.
        /// </summary>
        /// <param name="adUnitId">The unique ID for the ad unit.</param>
        /// <param name="config">The ad unit configuration.</param>
        public LevelPlayBannerAd(string adUnitId, Config config = null)
        {
            config ??= new Config.Builder().Build();

#if !UNITY_ANDROID && !UNITY_IOS
            m_BannerAd = new UnsupportedBannerAd(adUnitId, config?.PlatformConfig as UnsupportedBannerAd.Config);
#elif UNITY_EDITOR
            m_BannerAd = new EditorBannerAd(adUnitId, config?.PlatformConfig as EditorBannerAd.Config);
#elif UNITY_ANDROID
            m_BannerAd = new AndroidBannerAd(adUnitId, config?.PlatformConfig as AndroidBannerAd.Config);
#elif UNITY_IOS
            m_BannerAd = new IosBannerAd(adUnitId, config?.PlatformConfig as IosBannerAd.Config);
#endif

            SetupCallbacks();
        }

        internal LevelPlayBannerAd(IPlatformBannerAd platformBannerAd)
        {
            m_BannerAd = platformBannerAd;
        }

        /// <summary>
        /// Loads the banner ad.
        /// </summary>
        public void LoadAd()
        {
            m_BannerAd.LoadAd();
        }

        /// <summary>
        /// Destroys the banner ad and releases resources.
        /// </summary>
        public void DestroyAd()
        {
            m_BannerAd.DestroyAd();
        }

        /// <summary>
        /// Displays the banner ad to the user.
        /// </summary>
        public void ShowAd()
        {
            m_BannerAd.ShowAd();
        }

        /// <summary>
        /// Hides the banner ad from the user.
        /// </summary>
        public void HideAd()
        {
            m_BannerAd.HideAd();
        }

        /// <summary>
        /// Gets the ad ID associated with this ad.
        /// </summary>
        /// <returns>The ID of the ad.</returns>
        public string GetAdId()
        {
            return m_BannerAd.AdId;
        }

        /// <summary>
        /// Gets the ad unit ID associated with this ad.
        /// </summary>
        /// <returns>The ID of the ad unit.</returns>
        public string GetAdUnitId()
        {
            return m_BannerAd.AdUnitId;
        }

        /// <summary>
        /// Retrieves the size of the ad.
        /// </summary>
        /// <returns>The size of the ad.</returns>
        public LevelPlayAdSize GetAdSize()
        {
            return m_BannerAd.AdSize;
        }

        /// <summary>
        /// Retrieves the position of the banner ad.
        /// </summary>
        /// <returns>The position of the ad.</returns>
        public LevelPlayBannerPosition GetPosition()
        {
            return m_BannerAd.Position;
        }

        /// <summary>
        /// Retrieves the placement name associated with this ad.
        /// </summary>
        /// <returns>The placement name of the ad.</returns>
        public string GetPlacementName()
        {
            return m_BannerAd.PlacementName;
        }

        /// <summary>
        /// Pauses the auto-refreshing of the banner ad.
        /// </summary>
        public void PauseAutoRefresh()
        {
            m_BannerAd.PauseAutoRefresh();
        }

        /// <summary>
        /// Resumes the auto-refreshing of the banner ad that was previously paused.
        /// </summary>
        public void ResumeAutoRefresh()
        {
            m_BannerAd.ResumeAutoRefresh();
        }

        void SetupCallbacks()
        {
            m_BannerAd.OnAdLoaded += (info) => OnAdLoaded?.Invoke(info);
            m_BannerAd.OnAdLoadFailed += (error) => OnAdLoadFailed?.Invoke(error);
            m_BannerAd.OnAdClicked += (info) => OnAdClicked?.Invoke(info);
            m_BannerAd.OnAdDisplayed += (info) => OnAdDisplayed?.Invoke(info);
            m_BannerAd.OnAdExpanded += (info) => OnAdExpanded?.Invoke(info);
            m_BannerAd.OnAdCollapsed += (info) => OnAdCollapsed?.Invoke(info);
            m_BannerAd.OnAdLeftApplication += (info) => OnAdLeftApplication?.Invoke(info);
            m_BannerAd.OnAdDisplayFailed += (info, error) => OnAdDisplayFailed?.Invoke(info, error);
        }

        public void Dispose()
        {
            m_BannerAd?.DestroyAd();
        }

        public class Config
        {
            internal IPlatformBannerAd.IConfig PlatformConfig { get; }

            private Config(IPlatformBannerAd.IConfig platformConfig)
            {
                PlatformConfig = platformConfig;
            }

            public class Builder
            {
                private readonly IPlatformBannerAd.IConfigBuilder m_Builder;

                public Builder()
                {
#if !UNITY_ANDROID && !UNITY_IOS
                    m_Builder = new UnsupportedBannerAd.Config.Builder();
#elif UNITY_EDITOR
                    m_Builder = new EditorBannerAd.Config.Builder();
#elif UNITY_ANDROID
                    m_Builder = new AndroidBannerAd.Config.Builder();
#elif UNITY_IOS
                    m_Builder = new IosBannerAd.Config.Builder();
#endif
                }

                /// <summary>
                /// Set a Bid floor
                /// </summary>
                /// <param name="bidFloor">Bid floor in USD</param>
                /// <returns>this builder</returns>
                public Builder SetBidFloor(double bidFloor)
                {
                    m_Builder.SetBidFloor(bidFloor);
                    return this;
                }

                /// <summary>
                /// Set a size
                /// </summary>
                /// <param name="size">Size of the banner ad. Defaults to <see cref="LevelPlayAdSize.BANNER"/>.</param>
                /// <returns>this builder</returns>
                public Builder SetSize(LevelPlayAdSize size)
                {
                    m_Builder.SetSize(size);
                    return this;
                }

                /// <summary>
                /// Set a position
                /// </summary>
                /// <param name="position">Position on the screen where the ad will be displayed.
                /// Defaults to <see cref="LevelPlayBannerPosition.BottomCenter"/>.</param>
                /// <returns>this builder</returns>
                public Builder SetPosition(LevelPlayBannerPosition position)
                {
                    m_Builder.SetPosition(position);
                    return this;
                }

                /// <summary>
                /// Set a placement name, ignores `null`
                /// </summary>
                /// <param name="placementName">Name used for reporting and targeting</param>
                /// <returns>this builder</returns>
                public Builder SetPlacementName(string placementName)
                {
                    if (placementName != null)
                    {
                        m_Builder.SetPlacementName(placementName);
                    }
                    return this;
                }

                /// <summary>
                /// Set the "displayOnLoad" flag
                /// </summary>
                /// <param name="displayOnLoad">Determines whether the ad should be displayed immediately after loading.
                /// Defaults to true.</param>
                /// <returns>this builder</returns>
                public Builder SetDisplayOnLoad(bool displayOnLoad)
                {
                    m_Builder.SetDisplayOnLoad(displayOnLoad);
                    return this;
                }

                /// <summary>
                /// Set the "respectSafeArea" flag
                /// </summary>
                /// <param name="respectSafeArea">Determine whether the ad should be displayed within the safe area of the screen,
                /// where no notch, status bar or camera is present. Defaults to false</param>
                /// <returns>this builder</returns>
                public Builder SetRespectSafeArea(bool respectSafeArea)
                {
                    m_Builder.SetRespectSafeArea(respectSafeArea);
                    return this;
                }

                /// <summary>
                /// Build a new config object
                /// </summary>
                public Config Build()
                {
                    var platformConfig = m_Builder.Build();
                    return new Config(platformConfig);
                }
            }
        }
    }
}
