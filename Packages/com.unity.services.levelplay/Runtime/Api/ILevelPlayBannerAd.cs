using System;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Defines the interface for banner ads in the LevelPlay mediation system.
    /// </summary>
    public interface ILevelPlayBannerAd : IDisposable
    {
        /// <summary>
        /// Loads the banner ad.
        /// </summary>
        void LoadAd();

        /// <summary>
        /// Destroys the banner ad and releases resources.
        /// </summary>
        void DestroyAd();

        /// <summary>
        /// Displays the banner ad to the user.
        /// </summary>
        void ShowAd();

        /// <summary>
        /// Hides the banner ad from the user.
        /// </summary>
        void HideAd();

        /// <summary>
        /// Gets the ad ID associated with this ad.
        /// </summary>
        /// <returns>The ID of the ad.</returns>
        string GetAdId();

        /// <summary>
        /// Gets the ad unit ID associated with this ad.
        /// </summary>
        /// <returns>The ID of the ad unit.</returns>
        string GetAdUnitId();

        /// <summary>
        /// Retrieves the size of the ad.
        /// </summary>
        /// <returns>The size of the ad.</returns>
        LevelPlayAdSize GetAdSize();

        /// <summary>
        /// Retrieves the position of the banner ad.
        /// </summary>
        /// <returns>The position of the ad.</returns>
        LevelPlayBannerPosition GetPosition();

        /// <summary>
        /// Retrieves the placement name associated with this ad.
        /// </summary>
        /// <returns>The placement name of the ad.</returns>
        string GetPlacementName();

        /// <summary>
        /// Pauses the auto-refreshing of the banner ad.
        /// </summary>
        void PauseAutoRefresh();

        /// <summary>
        /// Resumes the auto-refreshing of the banner ad that was previously paused.
        /// </summary>
        void ResumeAutoRefresh();

        /// <summary>
        /// Raised when an ad has successfully loaded.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdLoaded;

        /// <summary>
        /// Raised when an ad fails to load.
        /// </summary>
        event Action<LevelPlayAdError> OnAdLoadFailed;

        /// <summary>
        /// Raised when the user clicks on an ad.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdClicked;

        /// <summary>
        /// Raised when an ad is displayed on screen.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdDisplayed;

        /// <summary>
        /// Raised when an attempt to display an ad fails.
        /// Provides both ad info and the error details.
        /// </summary>
        event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;

        /// <summary>
        /// Raised when an expandable ad has been expanded.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdExpanded;

        /// <summary>
        /// Raised when an expandable ad has been collapsed back to its original state.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdCollapsed;

        /// <summary>
        /// Raised when an ad causes the user to leave the application.
        /// </summary>
        event Action<LevelPlayAdInfo> OnAdLeftApplication;

    }
}
