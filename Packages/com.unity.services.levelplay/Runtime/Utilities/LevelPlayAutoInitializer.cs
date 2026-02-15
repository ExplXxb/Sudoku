using UnityEngine;

namespace Unity.Services.LevelPlay
{
    class LevelPlayAutoInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            var developerSettings = Resources.Load<LevelPlayMediationSettings>(Constants.k_LevelPlaySettingName);
            InitializeWithSettings(developerSettings);
        }

        internal static void InitializeWithSettings(LevelPlayMediationSettings developerSettings)
        {
            if (developerSettings == null) return;
            string appKey = null;

#if UNITY_ANDROID
            appKey = developerSettings.AndroidAppKey;
#elif UNITY_IOS
            appKey = developerSettings.IOSAppKey;
#elif UNITY_EDITOR
            if(developerSettings.AndroidAppKey != null)
            {
                appKey = developerSettings.AndroidAppKey;
            }
            else if (developerSettings.IOSAppKey != null)
            {
                appKey = developerSettings.IOSAppKey;
            }
#endif
            if (developerSettings.EnableIronsourceSDKInitAPI)
            {
                if (string.IsNullOrEmpty(appKey))
                {
                    LevelPlayLogger.LogWarning("LevelPlay SDK cannot initialize without AppKey");
                }
                else
                {
                    LevelPlay.PluginVersion += LevelPlay.PluginVersion.Contains("-r") ? "i" : "-i";
                    LevelPlay.Init(appKey);
                }
            }

            if (developerSettings.EnableAdapterDebug)
            {
                LevelPlay.SetAdaptersDebug(true);
            }

            if (developerSettings.EnableIntegrationHelper)
            {
                LevelPlay.ValidateIntegration();
            }
        }
    }
}
