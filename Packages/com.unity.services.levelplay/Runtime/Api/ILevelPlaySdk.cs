using System;

namespace Unity.Services.LevelPlay
{
    interface ILevelPlaySdk
    {
        event Action<LevelPlayConfiguration> OnInitSuccess;
        event Action<LevelPlayInitError> OnInitFailed;
        event Action<LevelPlayImpressionData> OnImpressionDataReady;
        void Initialize(string appKey, string userId);
        void SetPauseGame(bool pause);
        bool SetDynamicUserId(string dynamicUserId);
        void ValidateIntegration();
        void LaunchTestSuite();
        void SetAdaptersDebug(bool enabled);
        void SetNetworkData(string networkKey, string networkData);
        void SetMetaData(string key, string value);
        void SetMetaData(string key, params string[] values);
        void SetConsent(bool consent);
        void SetSegment(LevelPlaySegment segment);
    }
}
