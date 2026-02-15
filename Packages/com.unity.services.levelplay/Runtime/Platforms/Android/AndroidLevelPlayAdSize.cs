#if UNITY_ANDROID
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    sealed class AndroidLevelPlayAdSize : IPlatformLevelPlayAdSize
    {
        AndroidJavaObject m_AndroidAdSize;
        internal AndroidJavaObject AndroidAdSize => m_AndroidAdSize;

        PlatformLevelPlayAdSizeType m_PlatformLevelPlayAdSizeType;

        internal AndroidLevelPlayAdSize(PlatformLevelPlayAdSizeType adSizeType)
        {
            m_PlatformLevelPlayAdSizeType = adSizeType;
            m_AndroidAdSize = CreateAndroidJavaObject(adSizeType, 0, 0);
        }

        internal AndroidLevelPlayAdSize(int width, int height)
        {
            m_PlatformLevelPlayAdSizeType = PlatformLevelPlayAdSizeType.Custom;
            m_AndroidAdSize = CreateAndroidJavaObject(PlatformLevelPlayAdSizeType.Custom, width, height);
        }

        AndroidLevelPlayAdSize(PlatformLevelPlayAdSizeType adSizeType, AndroidJavaObject androidAdSize)
        {
            m_PlatformLevelPlayAdSizeType = adSizeType;
            m_AndroidAdSize = androidAdSize;
        }

        AndroidJavaObject CreateAndroidJavaObject(PlatformLevelPlayAdSizeType adSizeType, int width, int height)
        {

            var adSizeClass = new AndroidJavaClass("com.unity3d.mediation.LevelPlayAdSize");
            switch(adSizeType)
            {
                case PlatformLevelPlayAdSizeType.Banner:
                    return adSizeClass.GetStatic<AndroidJavaObject>("BANNER");
                case PlatformLevelPlayAdSizeType.Large:
                    return adSizeClass.GetStatic<AndroidJavaObject>("LARGE");
                case PlatformLevelPlayAdSizeType.MediumRectangle:
                    return adSizeClass.GetStatic<AndroidJavaObject>("MEDIUM_RECTANGLE");
                case PlatformLevelPlayAdSizeType.LeaderBoard:
                    return adSizeClass.GetStatic<AndroidJavaObject>("LEADERBOARD");
                case PlatformLevelPlayAdSizeType.Custom:
                    return adSizeClass.CallStatic<AndroidJavaObject>("createCustomSize", width, height);
                case PlatformLevelPlayAdSizeType.Adaptive:
                    return CreateAdaptiveAndroidAdSize(width);
                default:
                    return adSizeClass.GetStatic<AndroidJavaObject>("BANNER");
            }
        }

        internal static AndroidJavaObject CreateAdaptiveAndroidAdSize(int width)
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var context = activity.Call<AndroidJavaObject>("getApplicationContext");
            var adSizeClass = new AndroidJavaClass("com.unity3d.mediation.LevelPlayAdSize");
            if (width > 0)
            {
                var bannerUtils = new AndroidJavaClass("com.ironsource.unity.androidbridge.BannerUtils");
                return bannerUtils.CallStatic<AndroidJavaObject>("getAdaptiveAdSize", width);
            } else {
                return adSizeClass.CallStatic<AndroidJavaObject>("createAdaptiveAdSize", context);
            }
        }

        internal static AndroidLevelPlayAdSize CreateAdaptiveAdSize(int width = 0)
        {
            AndroidJavaObject adaptiveSize = CreateAdaptiveAndroidAdSize(width);
            return new AndroidLevelPlayAdSize(PlatformLevelPlayAdSizeType.Adaptive, adaptiveSize);
        }

        public int Width
        {
            get
            {
                if (m_AndroidAdSize == null)
                {
                    return 0;
                }

                return m_AndroidAdSize.Call<int>("getWidth");
            }
        }

        public int Height
        {
            get
            {
                if (m_AndroidAdSize == null)
                {
                    return 0;
                }

                return m_AndroidAdSize.Call<int>("getHeight");
            }
        }

        public PlatformLevelPlayAdSizeType AdSizeType
        {
            get
            {
                return m_PlatformLevelPlayAdSizeType;
            }
        }
    }
}
#endif
