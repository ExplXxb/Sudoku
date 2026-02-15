#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Services.LevelPlay
{
    sealed class EditorRewardedAd : IPlatformRewardedAd
    {
        string m_PrefabPath =>
            Directory.Exists("Packages/com.unity.services.levelplay")
                ? "Packages/com.unity.services.levelplay/Runtime/Platforms/Editor/EditorAds/Prefabs/MockRewardedEditorAd.prefab"
                : "Assets/LevelPlay/Runtime/Platforms/Editor/EditorAds/Prefabs/MockRewardedEditorAd.prefab";

        static GameObject m_AdGameObject;
        static RewardedPrefab m_AdPrefab;

        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdClosed;
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;

        public string AdId => "EditorRewardedMockAdId";
        public string AdUnitId { get; }

        internal EditorRewardedAd(string adUnitId)
        {
            AdUnitId = adUnitId;

            var mockPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(m_PrefabPath);
            m_AdGameObject = UnityEngine.Object.Instantiate(mockPrefab);
            m_AdPrefab = m_AdGameObject.GetComponent<RewardedPrefab>();
            UnityEngine.Object.DontDestroyOnLoad(m_AdGameObject);
            m_AdPrefab.m_Preview = false;

            SetupPrefabCallbacks();
        }

        void SetupPrefabCallbacks()
        {
            m_AdPrefab.OnAdLoaded += (args) => OnAdLoaded?.Invoke(args);
            m_AdPrefab.OnAdLoadFailed += (error) => OnAdLoadFailed?.Invoke(error);
            m_AdPrefab.OnAdDisplayed += (args) => OnAdDisplayed?.Invoke(args);
            m_AdPrefab.OnAdClosed += (args) => OnAdClosed?.Invoke(args);
            m_AdPrefab.OnAdClicked += (args) => OnAdClicked?.Invoke(args);
            m_AdPrefab.OnAdDisplayFailed += (info, error) => OnAdDisplayFailed?.Invoke(info, error);
            m_AdPrefab.OnAdInfoChanged += (args) => OnAdInfoChanged?.Invoke(args);
            m_AdPrefab.OnAdRewarded += (adInfo, adReward) => OnAdRewarded?.Invoke(adInfo, adReward);
        }

        public void LoadAd()
        {
            m_AdPrefab.LoadAd();
        }

        public void ShowAd(string placementName)
        {
            m_AdPrefab.ShowAd(placementName);
        }

        public bool IsAdReady()
        {
            return m_AdPrefab.IsAdReady();
        }

        public void Dispose()
        {
            Object.DestroyImmediate(m_AdGameObject);
            m_AdPrefab = null;
#if ENABLE_UNITY_SERVICES_LEVELPLAY_VERBOSE_LOGGING
            LevelPlayLogger.Log("Rewarded Ad object has been disposed in the Editor");
#endif
        }

        internal static bool IsPlacementCapped(string placementName)
        {
            LevelPlayLogger.Log("This API is not available on this platform.");
            return false;
        }

        public LevelPlayReward GetReward(string placement)
        {
            LevelPlayLogger.Log("This API is not available on this platform.");
            return LevelPlayReward.Default;
        }
    }
}
#endif
