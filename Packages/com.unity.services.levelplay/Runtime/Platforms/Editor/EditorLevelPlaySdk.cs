#if UNITY_EDITOR
using System;

namespace Unity.Services.LevelPlay
{
    sealed class EditorLevelPlaySdk : ILevelPlaySdk
    {
        public static readonly EditorLevelPlaySdk Instance = new();

        bool m_IsInitialized = false;
#pragma warning disable 0067
        public event Action<LevelPlayConfiguration> OnInitSuccess;
        public event Action<LevelPlayInitError> OnInitFailed;
        public event Action<LevelPlayImpressionData> OnImpressionDataReady;
#pragma warning restore 0067

        EditorLevelPlaySdk() { }

        // Public implementation of interface methods
        public void Initialize(string appKey, string userId)
        {
            // Simulate successful initialization
            OnInitSuccess?.Invoke(new LevelPlayConfiguration(string.Empty));
            m_IsInitialized = true;
        }

        public void SetPauseGame(bool pause)
        {
        }

        public bool SetDynamicUserId(string dynamicUserId)
        {
            return false;
        }

        public void ValidateIntegration()
        {
        }

        public void LaunchTestSuite()
        {
        }

        public void SetAdaptersDebug(bool enabled)
        {
        }

        public void SetNetworkData(string networkKey, string networkData)
        {
        }

        public void SetMetaData(string key, string value)
        {
        }

        public void SetMetaData(string key, params string[] values)
        {
        }

        public void SetConsent(bool consent)
        {
        }

        public void SetSegment(LevelPlaySegment segment)
        {
        }

        internal void CheckMockIsInitializedAndWarn()
        {
            if (m_IsInitialized)
            {
                return;
            }

            LevelPlayLogger.LogWarning("LevelPlay SDK is not Initialized");
        }
    }
}
#endif
