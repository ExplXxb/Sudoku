#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    sealed class AndroidBannerAd : IPlatformBannerAd, IUnityBannerAdListener
    {
        const string k_BannerAdClassName   = "com.ironsource.unity.androidbridge.BannerAd";

        const string k_FuncGetAdSize = "getAdSize";
        const string k_FuncLoad        = "load";
        const string k_FuncDestroy     = "destroy";
        const string k_FuncShowAd      = "showAd";
        const string k_FuncHideAd      = "hideAd";
        const string k_FuncPauseAutoRefresh = "pauseAutoRefresh";
        const string k_FuncResumeAutoRefresh = "resumeAutoRefresh";
        const string k_FuncGetAdId     = "getAdId";

        const string k_ErrorCallingAdUnitId    = "Cannot call AdUnitId";
        const string k_ErrorCallingLoad        = "Cannot call Load()";
        const string k_ErrorFailedToLoad       = "Failed to load - ";
        const string k_ErrorCreatingBanner     = "Error while creating Banner Ad. Banner Ad will not load. Please check your build settings, and make sure LevelPlay SDK is integrated properly.";
        const string k_ErrorDisposed           = "LevelPlay SDK: {0}: Instance of type {1} is disposed. Please create a new instance in order to call any method.";

        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdExpanded;
        public event Action<LevelPlayAdInfo> OnAdCollapsed;
        public event Action<LevelPlayAdInfo> OnAdLeftApplication;

        //IUnityBannerAdListener events
        public void onAdLoaded(string adInfo)
        {
            Debug.Log("onAdLoaded:" + adInfo);
            LevelPlayAdInfo info = new LevelPlayAdInfo(adInfo);
            OnAdLoaded?.Invoke(info);
        }

        public void onAdLoadFailed(string error)
        {
            OnAdLoadFailed?.Invoke(new LevelPlayAdError(error));
        }

        public void onAdClicked(string adInfo)
        {
            OnAdClicked?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdDisplayed(string adInfo)
        {
            OnAdDisplayed?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdDisplayFailed(string adInfo, string error)
        {
            OnAdDisplayFailed?.Invoke(new LevelPlayAdInfo(adInfo), new LevelPlayAdError(error));
        }

        public void onAdExpanded(string adInfo)
        {
            OnAdExpanded?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdCollapsed(string adInfo)
        {
            OnAdCollapsed?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdLeftApplication(string adInfo)
        {
            OnAdLeftApplication?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public string AdId => _mBannerAd.Call<string>(k_FuncGetAdId);
        public string AdUnitId { get; }

        public LevelPlayAdSize AdSize { get; }
        public LevelPlayBannerPosition Position { get; }

        public string PlacementName { get; }

        AndroidJavaObject _mBannerAd;
        IUnityBannerAdListener _mBannerAdListener;

        volatile bool _mDisposed;



        internal AndroidBannerAd(string adUnitId, Config config)
        {
            AdUnitId = adUnitId;
            AdSize = config.AdSize;
            Position = config.Position;
            PlacementName = config.PlacementName;

            ThreadUtil.Send(state =>
            {
                try
                {
                    if (_mBannerAdListener == null)
                    {
                        _mBannerAdListener = new UnityBannerAdListener(this);
                    }

                    _mBannerAd = new AndroidJavaObject(
                        k_BannerAdClassName,
                        adUnitId,
                        config.ConfigJavaObject,
                        _mBannerAdListener);
                }
                catch (Exception e)
                {
                    LevelPlayLogger.LogError(k_ErrorCreatingBanner);
                    LevelPlayLogger.LogException(e);
                }
            });
        }

        public void LoadAd()
        {
            if (!CheckDisposedAndLogError(k_ErrorCallingLoad))
            {
                ThreadUtil.Post(state =>
                {
                    try
                    {
                        _mBannerAd.Call(k_FuncLoad);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        OnAdLoadFailed?.Invoke(new LevelPlayAdError(AdUnitId, -1, k_ErrorFailedToLoad + e.Message));
                    }
                });
            }
        }

        public void ShowAd()
        {
            if (!CheckDisposedAndLogError(k_ErrorCallingLoad))
            {
                ThreadUtil.Post(state =>
                {
                    try
                    {
                        _mBannerAd.Call(k_FuncShowAd);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        OnAdLoadFailed?.Invoke(new LevelPlayAdError(AdUnitId, -1, k_ErrorFailedToLoad + e.Message));
                    }
                });
            }
        }

        public void HideAd()
        {
            if (!CheckDisposedAndLogError(k_ErrorCallingLoad))
            {
                ThreadUtil.Post(state =>
                {
                    try
                    {
                        _mBannerAd.Call(k_FuncHideAd);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        OnAdLoadFailed?.Invoke(new LevelPlayAdError(AdUnitId, -1, k_ErrorFailedToLoad + e.Message));
                    }
                });
            }
        }

        public void PauseAutoRefresh()
        {
            ThreadUtil.Post(state =>
            {
                try
                {
                    _mBannerAd.Call(k_FuncPauseAutoRefresh);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            });
        }

        public void ResumeAutoRefresh()
        {
            ThreadUtil.Post(state =>
            {
                try
                {
                    _mBannerAd.Call(k_FuncResumeAutoRefresh);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            });
        }

        public void DestroyAd()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AndroidBannerAd()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (!_mDisposed)
            {
                _mDisposed = true;
                if (disposing)
                {
                    //AndroidJavaObjects are created and destroyed with JNI's NewGlobalRef and DeleteGlobalRef,
                    //Therefore must be used on the same attached thread. In this case, it's Unity thread.
                    ThreadUtil.Post(state =>
                    {
                        _mBannerAd?.Call(k_FuncDestroy);
                        _mBannerAd?.Dispose();
                        _mBannerAdListener = null;
                        _mBannerAd = null;
                    });
                }
            }
        }

        bool CheckDisposedAndLogError(string message)
        {
            if (_mDisposed)
            {
                Debug.LogErrorFormat(k_ErrorDisposed, message, GetType().FullName);
            }
            return _mDisposed;
        }

        internal class Config : IPlatformBannerAd.IConfig
        {
            internal LevelPlayAdSize AdSize { get; }
            internal LevelPlayBannerPosition Position { get; }
            internal string PlacementName { get; }

            internal AndroidJavaObject ConfigJavaObject { get; }

            private Config(AndroidJavaObject config, LevelPlayAdSize adSize, LevelPlayBannerPosition position, string placementName)
            {
                ConfigJavaObject = config;
                AdSize = adSize;
                Position = position;
                PlacementName = placementName;
            }

            internal class Builder : IPlatformBannerAd.IConfigBuilder
            {
                private const string KBuilderClass = "com.ironsource.unity.androidbridge.BannerAd$Config$Builder";
                private readonly AndroidJavaObject m_BuilderJavaObject;
                private LevelPlayAdSize _size = LevelPlayAdSize.BANNER;
                private LevelPlayBannerPosition _position = LevelPlayBannerPosition.BottomCenter;
                private string _placementName;

                internal Builder()
                {
                    m_BuilderJavaObject = new AndroidJavaObject(KBuilderClass);
                }

                public void SetBidFloor(double bidFloor)
                {
                    m_BuilderJavaObject.Call("setBidFloor", bidFloor);
                }

                public void SetSize(LevelPlayAdSize size)
                {
                    _size = size;
                    var androidAdSize = ((AndroidLevelPlayAdSize)size.GetPlatformLevelPlayAdSize()).AndroidAdSize;
                    m_BuilderJavaObject.Call("setSize", androidAdSize);
                }

                public void SetPosition(LevelPlayBannerPosition position)
                {
                    _position = position;
                    m_BuilderJavaObject.Call("setPosition", position.Description, position.Position.x, position.Position.y);
                }

                public void SetPlacementName(string placementName)
                {
                    _placementName = placementName;
                    m_BuilderJavaObject.Call("setPlacementName", placementName);
                }

                public void SetDisplayOnLoad(bool displayOnLoad)
                {
                    m_BuilderJavaObject.Call("setDisplayOnLoad", displayOnLoad);
                }

                public void SetRespectSafeArea(bool respectSafeArea)
                {
                    m_BuilderJavaObject.Call("setRespectSafeArea", respectSafeArea);
                }

                public IPlatformBannerAd.IConfig Build()
                {
                    var androidConfig = m_BuilderJavaObject.Call<AndroidJavaObject>("build");

                    return new Config(androidConfig, _size, _position, _placementName);
                }
            }
        }
    }
}
#endif
