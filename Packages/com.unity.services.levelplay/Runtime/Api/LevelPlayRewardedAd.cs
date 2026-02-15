using System;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Implements ILevelPlayRewardedAd to provide functionality for managing rewarded ads.
    /// </summary>
    public sealed class LevelPlayRewardedAd : ILevelPlayRewardedAd
    {
        /// <summary>
        /// Invoked when the Rewarded ad is loaded.
        /// </summary>
        public event Action<LevelPlayAdInfo> OnAdLoaded;

        /// <summary>
        /// Invoked when the Rewarded ad fails to load.
        /// </summary>
        public event Action<LevelPlayAdError> OnAdLoadFailed;

        /// <summary>
        /// Invoked when the Rewarded ad is displayed.
        /// </summary>
        public event Action<LevelPlayAdInfo> OnAdDisplayed;

        /// <summary>
        /// Invoked when the Rewarded ad fails to display.
        /// </summary>
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;

        /// <summary>
        /// Invoked when the Rewarded ad receives a reward.
        /// </summary>
        public event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded;

        /// <summary>
        /// Invoked when the user clicks on the Rewarded ad.
        /// </summary>
        public event Action<LevelPlayAdInfo> OnAdClicked;

        /// <summary>
        /// Invoked when the Rewarded ad is closed.
        /// </summary>
        public event Action<LevelPlayAdInfo> OnAdClosed;

        /// <summary>
        /// Invoked when the Rewarded ad info is changed.
        /// </summary>
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;

        readonly IPlatformRewardedAd m_RewardedAd;

        /// <summary>
        /// Gets the ad unit id of the ad.
        /// </summary>
        public string AdUnitId => m_RewardedAd.AdUnitId;

        /// <summary>
        /// Creates and Initializes a new instance of the LevelPlay Rewarded Ad.
        /// </summary>
        /// <param name="adUnitId">The unique ID for the ad unit.</param>
        /// <param name="config">The ad unit configuration.</param>
        public LevelPlayRewardedAd(string adUnitId, Config config = null)
        {
            config ??= new Config.Builder().Build();

#if !UNITY_IOS && !UNITY_ANDROID
            m_RewardedAd = new UnsupportedRewardedAd(adUnitId);
#elif UNITY_EDITOR
            m_RewardedAd = new EditorRewardedAd(adUnitId);
#elif UNITY_ANDROID
            m_RewardedAd = new AndroidRewardedAd(adUnitId, config?.PlatformConfig as AndroidRewardedAd.Config);
#elif UNITY_IOS
            m_RewardedAd = new IosRewardedAd(adUnitId, config?.PlatformConfig as IosRewardedAd.Config);
#endif

            SetupEvents();
        }

        private void SetupEvents()
        {
            m_RewardedAd.OnAdLoaded += (info) => OnAdLoaded?.Invoke(info);
            m_RewardedAd.OnAdLoadFailed += (error) => OnAdLoadFailed?.Invoke(error);
            m_RewardedAd.OnAdDisplayed += (info) => OnAdDisplayed?.Invoke(info);
            m_RewardedAd.OnAdDisplayFailed += (info, error) => OnAdDisplayFailed?.Invoke(info, error);
            m_RewardedAd.OnAdRewarded += (info, reward) => OnAdRewarded?.Invoke(info, reward);
            m_RewardedAd.OnAdClicked += (info) => OnAdClicked?.Invoke(info);
            m_RewardedAd.OnAdClosed += (info) => OnAdClosed?.Invoke(info);
            m_RewardedAd.OnAdInfoChanged += (info) => OnAdInfoChanged?.Invoke(info);
        }

        internal LevelPlayRewardedAd(IPlatformRewardedAd platformRewardedAd)
        {
            m_RewardedAd = platformRewardedAd;
        }

        /// <summary>
        /// Loads the Rewarded Ad.
        /// </summary>
        public void LoadAd()
        {
            m_RewardedAd.LoadAd();
        }

        /// <summary>
        /// Shows the Rewarded Ad.
        /// </summary>
        /// <param name="placementName"><i><b>(Optional)</b></i>Placement Name for the Rewarded Ad.</param>
        public void ShowAd(string placementName = null)
        {
            m_RewardedAd.ShowAd(placementName);
        }

        /// <summary>
        /// Destroys the Rewarded Ad.
        /// </summary>
        public void DestroyAd()
        {
            Dispose();
        }

        /// <summary>
        /// Checks if the Rewarded ad is ready
        /// </summary>
        /// <returns>Returns true if the Rewarded ad is ready, returns false if not.</returns>
        public bool IsAdReady()
        {
            return m_RewardedAd.IsAdReady();
        }

        /// <summary>
        /// Checks if a given Placement Name is capped.
        /// </summary>
        /// <param name="placementName">Placement Name for the Rewarded Ad.</param>
        /// <returns>Returns true if placement is capped, returns false if not.</returns>
        public static bool IsPlacementCapped(string placementName)
        {
#if !UNITY_IOS && !UNITY_ANDROID
            return false;
#elif UNITY_EDITOR
            return EditorRewardedAd.IsPlacementCapped(placementName);
#elif UNITY_ANDROID
            return AndroidRewardedAd.IsPlacementCapped(placementName);
#elif UNITY_IOS
            return IosRewardedAd.IsPlacementCapped(placementName);
#else
            return false;
#endif
        }

        /// <summary>
        /// Dispose the rewarded ad
        /// </summary>
        public void Dispose()
        {
            m_RewardedAd.Dispose();
        }

        /// <summary>
        /// Gets the ad ID associated with this ad.
        /// </summary>
        /// <returns>The ID of the ad.</returns>
        public string GetAdId()
        {
            return m_RewardedAd.AdId;
        }

        /// <summary>
        /// Retrieves the reward associated with the ad.
        /// Use this method to obtain the reward configured for the ad unit or placement. The
        /// placement-specific reward takes precedence over the ad unit reward when a valid placement name
        /// is provided.
        /// </summary>
        /// <param name="placement">The placement name to retrieve the reward for, or null to use the ad unit's reward.</param>
        /// <returns>A <see cref="LevelPlayReward"/> object. Returns an empty reward on failures (name: "" and amount: 0).</returns>
        public LevelPlayReward GetReward(string placement = null)
        {
            return m_RewardedAd.GetReward(placement);
        }

        /// <summary>
        /// Rewarded ad configuration, use a <see cref="LevelPlayRewardedAd.Config.Builder"/> to initialize
        ///<br/>
        /// </summary>
        public class Config
        {
            internal IPlatformRewardedAd.IConfig PlatformConfig { get; }

            private Config(IPlatformRewardedAd.IConfig platformConfig)
            {
                PlatformConfig = platformConfig;
            }

            /// <summary>
            /// A config builder
            /// </summary>
            public class Builder
            {
                private readonly IPlatformRewardedAd.IConfigBuilder m_Builder;

                /// <summary>
                /// Initialize a new config builder
                /// </summary>
                public Builder()
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                    m_Builder = new AndroidRewardedAd.Config.Builder();
#elif UNITY_IOS && !UNITY_EDITOR
                    m_Builder = new IosRewardedAd.Config.Builder();
#else
                    m_Builder = new UnsupportedRewardedAd.Config.Builder();
#endif
                }

                /// <summary>
                /// Set a bid floor
                /// <param name="bidFloor">bid floor in USD</param>
                /// </summary>
                public Builder SetBidFloor(double bidFloor)
                {
                    m_Builder.SetBidFloor(bidFloor);
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
