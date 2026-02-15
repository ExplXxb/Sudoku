using System;
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    sealed class AndroidRewardedAd : IPlatformRewardedAd, IUnityRewardedAdListener
    {
        const string k_AndroidRewardedAdClass = "com.ironsource.unity.androidbridge.RewardedAd";
        const string k_AndroidLoadAdFunction = "loadAd";
        const string k_AndroidShowAdFunction = "showAd";
        const string k_IsAdReadyFunction = "isAdReady";
        const string k_IsPlacementCappedStaticFunction = "isPlacementCapped";
        const string k_FuncGetAdId       = "getAdId";
        const string k_FuncGetReward     = "getReward";
        const string k_FuncGetName       = "getName";
        const string k_FuncGetAmount     = "getAmount";

        const string k_ErrorDisposed = "Instance is disposed. Please create a new instance in order to call any method.";

        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdClosed;
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;

        AndroidJavaObject m_RewardedAdJavaObject;
        IUnityRewardedAdListener m_RewardedAdListener;

        volatile bool m_Disposed;
        volatile bool m_IsReady;

        public string AdUnitId { get; }

        public string AdId => m_RewardedAdJavaObject.Call<string>(k_FuncGetAdId);

        internal AndroidRewardedAd(string adUnitId)
        {
            AdUnitId = adUnitId;
            ThreadUtil.Send(state =>
            {
                try
                {
                    if (m_RewardedAdListener == null)
                    {
                        m_RewardedAdListener =
                            new UnityRewardedAdListener(this);
                    }
                    m_RewardedAdJavaObject =
                        new AndroidJavaObject(k_AndroidRewardedAdClass, adUnitId, m_RewardedAdListener);
                }
                catch (Exception e)
                {
                    LevelPlayLogger.LogException(e);
                }
            });
        }

        internal AndroidRewardedAd(string adUnitId, Config config)
        {
            AdUnitId = adUnitId;
            ThreadUtil.Send(state =>
            {
                try
                {
                    if (m_RewardedAdListener == null)
                    {
                        m_RewardedAdListener = new UnityRewardedAdListener(this);
                    }

                    m_RewardedAdJavaObject =
                        new AndroidJavaObject(k_AndroidRewardedAdClass, adUnitId, config.ConfigJavaObject, m_RewardedAdListener);
                }
                catch (Exception e)
                {
                    LevelPlayLogger.LogException(e);
                }
            });
        }

        public void LoadAd()
        {
            if (!CheckDisposedAndLogError())
            {
                ThreadUtil.Post(state =>
                {
                    try
                    {
                        m_RewardedAdJavaObject.Call(k_AndroidLoadAdFunction);
                    }
                    catch (Exception e)
                    {
                        LevelPlayLogger.LogException(e);
                    }
                });
            }
        }

        public void ShowAd(string placementName)
        {
            if (!CheckDisposedAndLogError())
            {
                ThreadUtil.Post(state =>
                {
                    try
                    {
                        m_RewardedAdJavaObject.Call(k_AndroidShowAdFunction, placementName);
                    }
                    catch (Exception e)
                    {
                        LevelPlayLogger.LogException(e);
                    }
                });
            }
        }

        public bool IsAdReady()
        {
            if (!CheckDisposedAndLogError())
            {
                ThreadUtil.Send(state =>
                {
                    try
                    {
                        m_IsReady = m_RewardedAdJavaObject.Call<bool>(k_IsAdReadyFunction);
                    }
                    catch (Exception e)
                    {
                        LevelPlayLogger.LogException(e);
                    }
                });
            }
            return m_IsReady;
        }

        public static bool IsPlacementCapped(string placementName)
        {
            var isPlacementCapped = false;
            try
            {
                using (var rewardedAdJavaClass = new AndroidJavaClass(k_AndroidRewardedAdClass))
                {
                    isPlacementCapped = rewardedAdJavaClass.CallStatic<bool>(k_IsPlacementCappedStaticFunction, placementName);
                }
            }
            catch (Exception e)
            {
                LevelPlayLogger.LogException(e);
            }
            return isPlacementCapped;
        }

        public LevelPlayReward GetReward(string placement)
        {
            if (CheckDisposedAndLogError()) return LevelPlayReward.Default;

            try
            {
                using var rewardJavaObject = m_RewardedAdJavaObject.Call<AndroidJavaObject>(k_FuncGetReward, placement);
                if (rewardJavaObject == null) return LevelPlayReward.Default;

                var rewardName = rewardJavaObject.Call<string>(k_FuncGetName);
                var rewardAmount = rewardJavaObject.Call<int>(k_FuncGetAmount);
                return new LevelPlayReward(rewardName, rewardAmount);
            }
            catch (Exception e)
            {
                LevelPlayLogger.LogException(e);
                return LevelPlayReward.Default;
            }
        }

        public void onAdLoaded(string adInfo)
        {
            OnAdLoaded?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdLoadFailed(string error)
        {
            OnAdLoadFailed?.Invoke(new LevelPlayAdError(error));
        }

        public void onAdDisplayed(string adInfo)
        {
            OnAdDisplayed?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdDisplayFailed(string error, string adInfo)
        {
            OnAdDisplayFailed?.Invoke(new LevelPlayAdInfo(adInfo), new LevelPlayAdError(error));
        }

        public void onAdRewarded(string adInfo, string rewardName, int rewardAmount)
        {
            OnAdRewarded?.Invoke(new LevelPlayAdInfo(adInfo), new LevelPlayReward(rewardName, rewardAmount));
        }

        public void onAdClicked(string adInfo)
        {
            OnAdClicked?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdClosed(string adInfo)
        {
            OnAdClosed?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        public void onAdInfoChanged(string adInfo)
        {
            OnAdInfoChanged?.Invoke(new LevelPlayAdInfo(adInfo));
        }

        void Dispose(bool disposing)
        {
            if (m_Disposed) return;
            m_Disposed = true;
            if (disposing)
            {
                ThreadUtil.Post(state =>
                {
                    m_RewardedAdJavaObject?.Dispose();
                    m_IsReady = false;
                    m_RewardedAdListener = null;
                    m_RewardedAdJavaObject = null;
                });
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AndroidRewardedAd()
        {
            Dispose(false);
        }

        bool CheckDisposedAndLogError()
        {
            if (m_Disposed)
            {
                LevelPlayLogger.LogError(k_ErrorDisposed);
            }
            return m_Disposed;
        }

        internal class Config : IPlatformRewardedAd.IConfig
        {
            internal AndroidJavaObject ConfigJavaObject { get; }

            Config(AndroidJavaObject config)
            {
                ConfigJavaObject = config;
            }

            internal class Builder : IPlatformRewardedAd.IConfigBuilder
            {
                private const string KBuilderClass = "com.ironsource.unity.androidbridge.RewardedAd$ConfigBuilder";
                private readonly AndroidJavaObject m_BuilderJavaObject;

                internal Builder()
                {
                    m_BuilderJavaObject = new AndroidJavaObject(KBuilderClass);
                }

                public void SetBidFloor(double bidFloor)
                {
                    m_BuilderJavaObject.Call("setBidFloor", bidFloor);
                }

                public IPlatformRewardedAd.IConfig Build()
                {
                    var androidConfig = m_BuilderJavaObject.Call<AndroidJavaObject>("build");
                    return new Config(androidConfig);
                }
            }
        }
    }
}
