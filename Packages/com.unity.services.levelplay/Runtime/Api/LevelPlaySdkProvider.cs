using System;

namespace Unity.Services.LevelPlay
{
    static class LevelPlaySdkProvider
    {
        internal static Func<ILevelPlaySdk> Factory { get; set; }

        static LevelPlaySdkProvider()
        {
            Reset();
        }

        internal static ILevelPlaySdk Create()
        {
            return Factory();
        }

        internal static void Reset()
        {
            Factory = () =>
            {
#if !UNITY_IOS && !UNITY_ANDROID
                return null;
#elif UNITY_EDITOR
                return EditorLevelPlaySdk.Instance;
#elif UNITY_ANDROID
                return AndroidLevelPlaySdk.Instance;
#elif UNITY_IOS
                return IosLevelPlaySdk.Instance;
#endif
            };
        }
    }
}
