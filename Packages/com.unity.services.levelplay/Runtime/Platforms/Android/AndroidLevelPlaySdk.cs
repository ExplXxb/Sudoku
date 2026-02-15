#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    sealed class AndroidLevelPlaySdk : ILevelPlaySdk, IUnityLevelPlayInitListener, IUnityLevelPlayImpressionDataListener
    {
        internal static readonly AndroidLevelPlaySdk Instance = new();

        AndroidJavaObject m_LevelPlayBridge;
        const string k_LevelPlayBridge = "com.ironsource.unity.androidbridge.LevelPlayBridge";
        readonly IUnityLevelPlayInitListener m_InitListener;
        readonly IUnityLevelPlayImpressionDataListener m_ImpressionListener;

        public event Action<LevelPlayConfiguration> OnInitSuccess;
        public event Action<LevelPlayInitError> OnInitFailed;
        public event Action<LevelPlayImpressionData> OnImpressionDataReady;

        private AndroidLevelPlaySdk()
        {
            m_InitListener = new UnityLevelPlayInitListener(this);
            m_ImpressionListener = new UnityLevelPlayImpressionDataListener(this);
        }

        public void onInitSuccess(string configuration)
        {
            OnInitSuccess?.Invoke(new LevelPlayConfiguration(configuration));
        }

        public void onInitFailed(string error)
        {
            OnInitFailed?.Invoke(new LevelPlayInitError(error));
        }

        public void onImpressionSuccess(string impressionData)
        {
            OnImpressionDataReady?.Invoke(new LevelPlayImpressionData(impressionData));
        }

        AndroidJavaObject GetBridge()
        {
            if (m_LevelPlayBridge == null)
            {
                using var pluginClass = new AndroidJavaClass(k_LevelPlayBridge);
                m_LevelPlayBridge = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            return m_LevelPlayBridge;
        }

        public void Initialize(string appKey, string userId)
        {
            var bridge = GetBridge();
            bridge.Call("setPluginData", "Unity", LevelPlay.PluginVersion, LevelPlay.UnityVersion);
            bridge.Call("setUnityImpressionDataListener", m_ImpressionListener);
            bridge.Call("initialize", appKey, userId, m_InitListener);
        }

        public bool SetDynamicUserId(string dynamicUserId)
        {
            return GetBridge().Call<bool>("setDynamicUserId", dynamicUserId);
        }

        public void LaunchTestSuite()
        {
            GetBridge().Call("launchTestSuite");
        }

        public void SetAdaptersDebug(bool enabled)
        {
            GetBridge().Call("setAdaptersDebug", enabled);
        }

        public void ValidateIntegration()
        {
            GetBridge().Call("validateIntegration");
        }

        public void SetNetworkData(string networkKey, string networkData)
        {
            GetBridge().Call("setNetworkData", networkKey, networkData);
        }

        public void SetMetaData(string key, string value)
        {
            GetBridge().Call("setMetaData", key, value);
        }

        public void SetMetaData(string key, params string[] values)
        {
            GetBridge().Call("setMetaData", key, values);
        }

        public void SetConsent(bool consent)
        {
            GetBridge().Call("setConsent", consent);
        }

        public void SetSegment(LevelPlaySegment segment)
        {
            var dict = segment.GetSegmentAsDictionary();
            var json = LevelPlayJson.Serialize(dict);
            GetBridge().Call("setSegment", json);
        }

        public void SetPauseGame(bool pause)
        {
            LevelPlayLogger.LogWarning("Unexpected call to SetPauseGame on Android. This method is not implemented for Android.");
        }
    }
}
#endif
