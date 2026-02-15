using System;
using System.Linq;
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Manages initialization and basic operations of the LevelPlay SDK.
    /// This class provides methods to initialize the SDK and handles global events for initialization success and failure.
    /// </summary>
    public class LevelPlay
    {
        private static readonly ILevelPlaySdk Sdk;

        static LevelPlay()
        {
            Sdk = LevelPlaySdkProvider.Create();
            if (Sdk == null)
            {
                return;
            }

            Sdk.OnInitSuccess += configuration =>
            {
                OnInitSuccessReceived?.Invoke(configuration);
            };

            Sdk.OnInitFailed += error =>
            {
                OnInitFailedReceived?.Invoke(error);
            };

            Sdk.OnImpressionDataReady += impressionData =>
            {
                OnImpressionDataReadyReceived?.Invoke(impressionData);
            };
        }

        /// <summary>
        /// Returns the Unity Editor version.
        /// </summary>
        public static string UnityVersion => Application.unityVersion;

        /// <summary>
        /// Returns the LevelPlay Package version.
        /// </summary>
        public static string PluginVersion = Constants.k_AnnotatedPackageVersion;

        static event Action<LevelPlayConfiguration> OnInitSuccessReceived;
        static event Action<LevelPlayInitError> OnInitFailedReceived;
        static event Action<LevelPlayImpressionData> OnImpressionDataReadyReceived;

        /// <summary>
        /// Adds or removes event handlers for the SDK initialization success event.
        /// Ensures that the same handler cannot be added multiple times.
        /// </summary>
        public static event Action<LevelPlayConfiguration> OnInitSuccess
        {
            add
            {
                if (OnInitSuccessReceived == null || !OnInitSuccessReceived.GetInvocationList().Contains(value))
                {
                    OnInitSuccessReceived += value;
                }
            }

            remove
            {
                if (OnInitSuccessReceived != null && OnInitSuccessReceived.GetInvocationList().Contains(value))
                {
                    OnInitSuccessReceived -= value;
                }
            }
        }

        /// <summary>
        /// Adds or removes event handlers for the SDK initialization failure event.
        /// Ensures that the same handler cannot be added multiple times.
        /// </summary>
        public static event Action<LevelPlayInitError> OnInitFailed
        {
            add
            {
                if (OnInitFailedReceived == null || !OnInitFailedReceived.GetInvocationList().Contains(value))
                {
                    OnInitFailedReceived += value;
                }
            }

            remove
            {
                if (OnInitFailedReceived != null && OnInitFailedReceived.GetInvocationList().Contains(value))
                {
                    OnInitFailedReceived -= value;
                }
            }
        }

        /// <summary>
        /// Event triggered when an impression event occurs.
        /// This event is triggered on a background thread, not the Unity main thread.
        /// </summary>
        public static event Action<LevelPlayImpressionData> OnImpressionDataReady
        {
            add
            {
                if (OnImpressionDataReadyReceived == null ||
                    !OnImpressionDataReadyReceived.GetInvocationList().Contains(value))
                {
                    OnImpressionDataReadyReceived += value;
                }
            }

            remove
            {
                if (OnImpressionDataReadyReceived != null &&
                    OnImpressionDataReadyReceived.GetInvocationList().Contains(value))
                {
                    OnImpressionDataReadyReceived -= value;
                }
            }
        }

        /// <summary>
        /// Initializes the LevelPlay SDK with the specified app key and optional user ID and ad format list.
        /// </summary>
        /// <param name="appKey">The application key for the SDK.</param>
        /// <param name="userId">Optional user identifier for use within the SDK.</param>
        public static void Init(string appKey, string userId = null)
        {
            if (Sdk == null)
            {
#if ENABLE_UNITY_SERVICES_LEVELPLAY_VERBOSE_LOGGING
                LevelPlayLogger.LogWarning("LevelPlay is unsupported on this platform");
#endif
                return;
            }

            Sdk.Initialize(appKey, userId);
        }

        /// <summary>
        /// When setting your PauseGame status to true, all your Unity 3D game activities will be paused (Except the ad callbacks).
        /// The game activity will be resumed automatically when the ad is closed.
        /// You should call the setPauseGame once in your session, before or after initializing the ironSource SDK,
        /// as it affects all ads (Rewarded Video and Interstitial ads) in the session.
        /// </summary>
        /// <param name="pause">Is the game paused</param>
        public static void SetPauseGame(bool pause) => Sdk?.SetPauseGame(pause);

        /// <summary>
        /// Sets a dynamic user ID that can be changed through the session and will be used in server to server rewarded
        /// ad callbacks.
        /// This parameter helps verify AdRewarded transactions and must be set before calling ShowRewardedVideo.
        /// </summary>
        /// <param name="dynamicUserId">The ID to be set</param>
        /// <returns>Was the dynamic user ID set successfully</returns>
        public static bool SetDynamicUserId(string dynamicUserId) => Sdk?.SetDynamicUserId(dynamicUserId) ?? false;

        /// <summary>
        /// Runs the integration validation.
        /// </summary>
        public static void ValidateIntegration() => Sdk?.ValidateIntegration();

        /// <summary>
        /// Launches the Test Suite. Mediation SDK must be initialized before calling this method.
        /// </summary>
        public static void LaunchTestSuite() => Sdk?.LaunchTestSuite();

        /// <summary>
        /// Enables or disables adapters debug info.
        /// </summary>
        /// <param name="enabled">Is adapters debug info enabled</param>
        public static void SetAdaptersDebug(bool enabled) => Sdk?.SetAdaptersDebug(enabled);

        /// <summary>
        /// Set custom network data.
        /// </summary>
        /// <param name="networkKey">The attribute key</param>
        /// <param name="networkData">The attribute value</param>
        public static void SetNetworkData(string networkKey, string networkData) =>
            Sdk?.SetNetworkData(networkKey, networkData);

        /// <summary>
        /// Allows setting extra flags, for example "do_not_sell" to allow or disallow selling or sharing personal information.
        /// </summary>
        /// <param name="key">The flag to set</param>
        /// <param name="value">The value for the flag</param>
        public static void SetMetaData(string key, string value) => Sdk?.SetMetaData(key, value);

        /// <summary>
        /// Allows setting extra flags, for example "do_not_sell" to allow or disallow selling or sharing personal information.
        /// </summary>
        /// <param name="key">The flag to set</param>
        /// <param name="values">The values for the flag</param>
        public static void SetMetaData(string key, params string[] values) => Sdk?.SetMetaData(key, values);

        /// <summary>
        /// Set user's GDPR consent.
        /// </summary>
        /// <param name="consent">Whether the user has granted consent</param>
        public static void SetConsent(bool consent) => Sdk?.SetConsent(consent);

        /// <summary>
        /// Set the segment a user belongs to.
        /// </summary>
        /// <param name="segment">Segment information for the current user</param>
        public static void SetSegment(LevelPlaySegment segment) => Sdk?.SetSegment(segment);
    }
}
