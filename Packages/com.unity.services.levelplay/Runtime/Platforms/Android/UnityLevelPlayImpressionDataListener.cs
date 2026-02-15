#if UNITY_ANDROID
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    sealed class UnityLevelPlayImpressionDataListener : AndroidJavaProxy, IUnityLevelPlayImpressionDataListener
    {
        const string k_ILevelPlayImpressionDataListenerName =
            "com.ironsource.unity.androidbridge.UnityImpressionDataListener";

        IUnityLevelPlayImpressionDataListener m_Listener;

        public UnityLevelPlayImpressionDataListener(IUnityLevelPlayImpressionDataListener listener) : base(
            k_ILevelPlayImpressionDataListenerName)
        {
            m_Listener = listener;
        }

        public void onImpressionSuccess(string impressionData)
        {
            if (m_Listener == null)
            {
#if ENABLE_UNITY_SERVICES_LEVELPLAY_VERBOSE_LOGGING
                LevelPlayLogger.LogError("ImpressionDataListener is null");
#endif
                return;
            }

            m_Listener.onImpressionSuccess(impressionData);
        }
    }
}
#endif
