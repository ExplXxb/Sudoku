#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Services.LevelPlay
{
    sealed class EditorBannerAd : IPlatformBannerAd
    {
        GameObject m_AdGameObject;
        BannerPrefab m_AdPrefab;

        bool m_DisplayOnLoad;
        bool m_RespectSafeArea;

        string m_PrefabPath =>
            Directory.Exists("Packages/com.unity.services.levelplay")
            ? "Packages/com.unity.services.levelplay/Runtime/Platforms/Editor/EditorAds/Prefabs/MockBannerEditorAd.prefab"
            : "Assets/LevelPlay/Runtime/Platforms/Editor/EditorAds/Prefabs/MockBannerEditorAd.prefab";

        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdExpanded;
        public event Action<LevelPlayAdInfo> OnAdCollapsed;
        public event Action<LevelPlayAdInfo> OnAdLeftApplication;

        public string AdId => "EditorPrefabMockAdId";
        public string AdUnitId { get; }
        public LevelPlayAdSize AdSize { get; }
        public string PlacementName { get; }
        public LevelPlayBannerPosition Position { get; }

        public EditorBannerAd(string adUnitId, Config config)
        {
            AdUnitId = adUnitId;
            AdSize = config.AdSize;
            Position = config.Position;
            PlacementName = config.PlacementName;
            m_DisplayOnLoad = config.DisplayOnLoad;
            m_RespectSafeArea = config.RespectSafeArea;

            Setup();
        }

        private void Setup()
        {
            var bannerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(m_PrefabPath);
            m_AdGameObject = Object.Instantiate(bannerPrefab);
            m_AdPrefab = m_AdGameObject.GetComponent<BannerPrefab>();
            Object.DontDestroyOnLoad(m_AdGameObject);

            m_AdPrefab.m_Preview = false;
            m_AdPrefab.m_BannerPosition = Position;
            m_AdPrefab.m_BannerAdSize = AdSize;

            SetupPrefabCallbacks();
        }

        void SetupPrefabCallbacks()
        {
            m_AdPrefab.OnAdLoaded += (args) => OnAdLoaded?.Invoke(args);
            m_AdPrefab.OnAdLoadFailed += (args) => OnAdLoadFailed?.Invoke(args);
            m_AdPrefab.OnAdClicked += (args) => OnAdClicked?.Invoke(args);
            m_AdPrefab.OnAdDisplayed += (args) => OnAdDisplayed?.Invoke(args);
            m_AdPrefab.OnAdDisplayFailed += (info, error) => OnAdDisplayFailed?.Invoke(info, error);
            m_AdPrefab.OnAdExpanded += (args) => OnAdExpanded?.Invoke(args);
            m_AdPrefab.OnAdCollapsed += (args) => OnAdCollapsed?.Invoke(args);
            m_AdPrefab.OnAdLeftApplication += (args) => OnAdLeftApplication?.Invoke(args);
        }

        public void LoadAd()
        {
            m_AdPrefab.LoadAd();
            if (m_DisplayOnLoad)
            {
                m_AdPrefab.ShowAd();
            }
        }

        public void ShowAd()
        {
            m_AdPrefab.ShowAd();
        }

        public void DestroyAd()
        {
            Dispose();
        }

        public void HideAd()
        {
            m_AdPrefab.HideAd();
        }

        public void PauseAutoRefresh()
        {
#if ENABLE_UNITY_SERVICES_LEVELPLAY_VERBOSE_LOGGING
            LevelPlayLogger.Log("Pause Auto Refresh has been called in the Editor");
#endif
        }

        public void ResumeAutoRefresh()
        {
#if ENABLE_UNITY_SERVICES_LEVELPLAY_VERBOSE_LOGGING
            LevelPlayLogger.Log("Resume Auto Refresh has been called in the Editor");
#endif
        }

        public void Dispose()
        {
            Object.DestroyImmediate(m_AdGameObject);
            m_AdPrefab = null;
#if ENABLE_UNITY_SERVICES_LEVELPLAY_VERBOSE_LOGGING
            LevelPlayLogger.Log("Banner Ad object has been disposed in the Editor");
#endif
        }

        internal class Config : IPlatformBannerAd.IConfig
        {
            internal LevelPlayAdSize AdSize { get; }
            internal LevelPlayBannerPosition Position { get; }
            internal string PlacementName { get; }
            internal bool DisplayOnLoad { get; }
            internal bool RespectSafeArea { get; }

            private Config(
                LevelPlayAdSize adSize,
                LevelPlayBannerPosition position,
                string placementName,
                bool displayOnLoad,
                bool respectSafeArea)
            {
                AdSize = adSize;
                Position = position;
                PlacementName = placementName;
                DisplayOnLoad = displayOnLoad;
                RespectSafeArea = respectSafeArea;
            }

            internal class Builder : IPlatformBannerAd.IConfigBuilder
            {
                private LevelPlayAdSize _adSize = LevelPlayAdSize.BANNER;
                private LevelPlayBannerPosition _position = LevelPlayBannerPosition.BottomCenter;
                private string _placementName;
                private bool _displayOnLoad = true;
                private bool _respectSafeArea = false;
                private double _bidFloor;

                public void SetBidFloor(double bidFloor)
                {
                    _bidFloor = bidFloor;
                }

                public void SetSize(LevelPlayAdSize size)
                {
                    _adSize = size;
                }

                public void SetPosition(LevelPlayBannerPosition position)
                {
                    _position = position;
                }

                public void SetPlacementName(string placementName)
                {
                    _placementName = placementName;
                }

                public void SetDisplayOnLoad(bool displayOnLoad)
                {
                    _displayOnLoad = displayOnLoad;
                }

                public void SetRespectSafeArea(bool respectSafeArea)
                {
                    _respectSafeArea = respectSafeArea;
                }

                public IPlatformBannerAd.IConfig Build()
                {
                    return new Config(_adSize, _position, _placementName, _displayOnLoad, _respectSafeArea);
                }
            }
        }
    }
}
#endif
