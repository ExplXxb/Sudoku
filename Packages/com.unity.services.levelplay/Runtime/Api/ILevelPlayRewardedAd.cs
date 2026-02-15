using System;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// APIs for LevelPlay Rewarded Ad in the Unity package.
    /// </summary>
    public interface ILevelPlayRewardedAd : IDisposable
    {
        /// <summary>
        /// Invoked when the Rewarded ad is loaded.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdLoaded;

        /// <summary>
        /// Invoked when the Rewarded ad fails to load.
        /// </summary>
        event Action<LevelPlayAdError> OnAdLoadFailed;

        /// <summary>
        /// Invoked when the Rewarded ad is displayed.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdDisplayed;

        /// <summary>
        /// Invoked when the Rewarded ad fails to display.
        /// </summary>
        event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;

        /// <summary>
        /// Invoked when the Rewarded ad receives a reward.
        /// </summary>
        event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded;

        /// <summary>
        /// Invoked when the Rewarded ad when the user clicks on the ad.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdClicked;

        /// <summary>
        /// Invoked when the Rewarded ad is closed.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdClosed;

        /// <summary>
        /// Invoked when the Rewarded ad info is changed.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdInfoChanged;

        /// <summary>
        /// Gets the ad ID associated with this ad.
        /// </summary>
        /// <returns>The ID of the ad.</returns>
        string GetAdId();

        /// <summary>
        /// Gets the ad unit id of the ad.
        /// </summary>
        string AdUnitId { get; }

        /// <summary>
        /// Loads the Rewarded Ad.
        /// </summary>
        void LoadAd();

        /// <summary>
        /// Shows the Rewarded Ad.
        /// </summary>
        /// <param name="placementName"><i><b>(Optional)</b></i>Placement Name for the Rewarded Ad.</param>
        void ShowAd(string placementName = null);

        /// <summary>
        /// Destroys the Rewarded Ad.
        /// </summary>
        void DestroyAd();

        /// <summary>
        /// Checks if the Rewarded ad is ready
        /// </summary>
        /// <returns>Returns true if the Rewarded ad is ready, returns false if not.</returns>
        bool IsAdReady();

        /// <summary>
        /// Retrieves the reward associated with the ad.
        /// Use this method to obtain the reward configured for the ad unit or placement. The
        /// placement-specific reward takes precedence over the ad unit reward when a valid placement name
        /// is provided.
        /// </summary>
        /// <param name="placement">The placement name to retrieve the reward for, or null to use the ad unit's reward.</param>
        /// <returns>A <see cref="LevelPlayReward"/> object. Returns an empty reward on failures (name: "" and amount: 0).</returns>
        LevelPlayReward GetReward(string placement);
    }
}
