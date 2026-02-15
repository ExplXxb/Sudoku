#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

namespace Unity.Services.LevelPlay
{
    sealed class IosLevelPlaySdk : ILevelPlaySdk
    {
        internal static readonly IosLevelPlaySdk Instance = new();
        public event Action<LevelPlayConfiguration> OnInitSuccess;
        public event Action<LevelPlayInitError> OnInitFailed;
        public event Action<LevelPlayImpressionData> OnImpressionDataReady;

        IosLevelPlaySdk() { }

        public void Initialize(string appKey, string userId)
        {
            setPluginData("Unity", LevelPlay.PluginVersion, LevelPlay.UnityVersion);
            LPMInitialize(appKey, userId, OnInitializationSuccess, OnInitializationFailed, OnImpressionSuccess);
        }

        public void SetPauseGame(bool pause)
        {
            LPMSetPauseGame(pause);
        }

        public bool SetDynamicUserId(string dynamicUserId)
        {
            return LPMSetDynamicUserId(dynamicUserId);
        }

        public void ValidateIntegration()
        {
            LPMValidateIntegration();
        }
        public void LaunchTestSuite()
        {
            LPMLaunchTestSuite();
        }

        public void SetAdaptersDebug(bool enabled) {
            LPMSetAdaptersDebug(enabled);
        }

        public void SetNetworkData(string networkKey, string networkData) {
            LPMSetNetworkData(networkKey, networkData);
        }

        public void SetMetaData(string key, string value)
        {
            LPMSetMetaData(key, value);
        }

        public void SetMetaData(string key, params string[] values)
        {
            LPMSetMetaDataWithValues(key, values);
        }

        public void SetConsent(bool consent)
        {
            LPMSetConsent(consent);
        }

        public void SetSegment(LevelPlaySegment segment)
        {
            var dict = segment.GetSegmentAsDictionary();
            var json = LevelPlayJson.Serialize(dict);
            LPMSetSegment(json);
        }

        delegate void InitSuccessCallback(string configuration);
        delegate void InitFailureCallback(string error);
        delegate void ImpressionSuccessCallback(string impressionData);

        [AOT.MonoPInvokeCallback(typeof(InitSuccessCallback))]
        static void OnInitializationSuccess(string configuration)
        {
            Instance?.OnInitSuccess?.Invoke(new LevelPlayConfiguration(configuration));
        }

        [AOT.MonoPInvokeCallback(typeof(InitFailureCallback))]
        static void OnInitializationFailed(string error)
        {
            Instance?.OnInitFailed?.Invoke(new LevelPlayInitError(error));
        }

        [AOT.MonoPInvokeCallback(typeof(ImpressionSuccessCallback))]
        static void OnImpressionSuccess(string impressionData)
        {
            Instance?.OnImpressionDataReady?.Invoke(new LevelPlayImpressionData(impressionData));
        }

        [DllImport("__Internal")]
        private static extern void LPMInitialize(string appKey,
            string userId,
            InitSuccessCallback initSuccessCallback,
            InitFailureCallback initFailureCallback,
            ImpressionSuccessCallback impressionSuccessCallback);

        [DllImport("__Internal")]
        private static extern void setPluginData(string pluginType, string pluginVersion, string pluginFrameworkVersion);

        [DllImport("__Internal")]
        private static extern void LPMSetPauseGame(bool pause);

        [DllImport("__Internal")]
        private static extern bool LPMSetDynamicUserId(string dynamicUserId);

        [DllImport("__Internal")]
        private static extern void LPMValidateIntegration();

        [DllImport("__Internal")]
        private static extern void LPMLaunchTestSuite();

        [DllImport("__Internal")]
        private static extern void LPMSetAdaptersDebug(bool enabled);

        [DllImport("__Internal")]
        private static extern void LPMSetNetworkData(string networkKey, string networkData);

        [DllImport("__Internal")]
        private static extern void LPMSetMetaData(string key, string value);

        [DllImport("__Internal")]
        private static extern void LPMSetMetaDataWithValues(string key, params string[] values);

        [DllImport("__Internal")]
        private static extern void LPMSetConsent(bool consent);

        [DllImport("__Internal")]
        private static extern void LPMSetSegment(string json);
    }
}
#endif
