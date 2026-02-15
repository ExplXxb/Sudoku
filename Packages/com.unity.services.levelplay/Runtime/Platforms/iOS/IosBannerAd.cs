#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Unity.Services.LevelPlay
{
    sealed class IosBannerAd : IosNativeObject, IPlatformBannerAd
    {
        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdExpanded;
        public event Action<LevelPlayAdInfo> OnAdCollapsed;
        public event Action<LevelPlayAdInfo> OnAdLeftApplication;

        public string AdUnitId { get; }
        public LevelPlayAdSize AdSize { get; }
        public string PlacementName { get; }
        public LevelPlayBannerPosition Position { get; }
        private bool DisplayOnLoad { get; }

        public string AdId => GetAdId();

        IosBannerAdListener _mBannerAdListener;

        internal IosBannerAd(string adUnitId, Config config) : base(true)
        {
            AdUnitId = adUnitId;
            AdSize = config.AdSize;
            Position = config.Position;
            PlacementName = config.PlacementName;
            DisplayOnLoad = config.DisplayOnLoad;

            IosLevelPlayAdSize iosAdSize = (IosLevelPlayAdSize)AdSize.GetPlatformLevelPlayAdSize();
            NativePtr = BannerAdCreate(adUnitId, config.IosConfig, iosAdSize.NativePtr);
            if (_mBannerAdListener == null)
            {
                _mBannerAdListener = new IosBannerAdListener(this);
            }

            BannerAdSetDelegate(NativePtr, _mBannerAdListener.NativePtr);
        }

        public void PauseAutoRefresh()
        {
            if (CheckDisposedAndLogError("Cannot pause auto-refresh")) return;
            BannerAdPauseAutoRefresh(NativePtr);
        }

        public void ResumeAutoRefresh()
        {
            if (CheckDisposedAndLogError("Cannot resume auto-refresh")) return;
            BannerAdResumeAutoRefresh(NativePtr);
        }

        public void LoadAd()
        {
            if (CheckDisposedAndLogError("Cannot call Load()")) return;
            BannerAdLoad(NativePtr);
            SetPosition();
            if (DisplayOnLoad)
            {
                ShowAd();
            }
            else
            {
                HideAd();
            }
        }

        public void DestroyAd()
        {
            if (NativePtr != IntPtr.Zero)
            {
                BannerAdDestroy(NativePtr);
                NativePtr = IntPtr.Zero;
            }

            base.Dispose();
        }

        public void SetPosition()
        {
            if (CheckDisposedAndLogError("Cannot set Banner Position")) return;
            BannerAdSetPosition(NativePtr, Position.Description, Position.Position.x, Position.Position.y);
        }

        public void ShowAd()
        {
            BannerAdViewShow(NativePtr);
        }

        public void HideAd()
        {
            BannerAdViewHide(NativePtr);
        }

        //Invoke events defined in IosBannerAdListener.cs
        internal void InvokeLoadedEvent(LevelPlayAdInfo adInfo)
        {
            ThreadUtil.Post(state => OnAdLoaded?.Invoke(adInfo));
        }

        internal void InvokeFailedLoadEvent(LevelPlayAdError error)
        {
            ThreadUtil.Post(state => OnAdLoadFailed?.Invoke(error));
        }

        internal void InvokeClickedEvent(LevelPlayAdInfo adInfo)
        {
            ThreadUtil.Post(state => OnAdClicked?.Invoke(adInfo));
        }

        internal void InvokeDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            ThreadUtil.Post(state => OnAdDisplayed?.Invoke(adInfo));
        }

        internal void InvokeFailedDisplayEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
        {
            ThreadUtil.Post(state => OnAdDisplayFailed?.Invoke(adInfo, error));
        }

        internal void InvokeExpandedEvent(LevelPlayAdInfo adInfo)
        {
            ThreadUtil.Post(state => OnAdExpanded?.Invoke(adInfo));
        }

        internal void InvokeCollapsedEvent(LevelPlayAdInfo adInfo)
        {
            ThreadUtil.Post(state => OnAdCollapsed?.Invoke(adInfo));
        }

        internal void InvokeLeftApplicationEvent(LevelPlayAdInfo adInfo)
        {
            ThreadUtil.Post(state => OnAdLeftApplication?.Invoke(adInfo));
        }

        private string GetAdId()
        {
            if (CheckDisposedAndLogError("Cannot get Banner ad Id")) return "";
            return BannerAdId(NativePtr);
        }

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewCreateWithConfig")]
        static extern IntPtr BannerAdCreate(string adUnitId, IntPtr config, IntPtr adSize);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewSetDelegate")]
        static extern void BannerAdSetDelegate(IntPtr bannerAdView, IntPtr bannerAdListener);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewLoadAd")]
        static extern void BannerAdLoad(IntPtr bannerAdView);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewDestroy")]
        static extern void BannerAdDestroy(IntPtr bannerAdView);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewSetPosition")]
        private static extern void BannerAdSetPosition(IntPtr bannerAdView, string position, float x, float y);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewShow")]
        private static extern void BannerAdViewShow(IntPtr bannerAdView);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewHide")]
        private static extern void BannerAdViewHide(IntPtr bannerAdView);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewPauseAutoRefresh")]
        static extern void BannerAdPauseAutoRefresh(IntPtr bannerAdView);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewResumeAutoRefresh")]
        static extern void BannerAdResumeAutoRefresh(IntPtr bannerAdView);

        [DllImport("__Internal", EntryPoint = "LPMBannerAdViewAdId")]
        static extern string BannerAdId(IntPtr bannerAdView);

        internal class Config : IPlatformBannerAd.IConfig
        {
            internal LevelPlayAdSize AdSize { get; }
            internal LevelPlayBannerPosition Position { get; }
            internal string PlacementName { get; }
            internal bool DisplayOnLoad { get; }
            internal IntPtr IosConfig { get; }

            private Config(
                LevelPlayAdSize adSize,
                LevelPlayBannerPosition position,
                string placementName,
                bool displayOnLoad,
                IntPtr iosConfig)
            {
                AdSize = adSize;
                Position = position;
                PlacementName = placementName;
                DisplayOnLoad = displayOnLoad;
                IosConfig = iosConfig;
            }

            internal class Builder : IPlatformBannerAd.IConfigBuilder
            {
                private LevelPlayAdSize _adSize = LevelPlayAdSize.BANNER;
                private LevelPlayBannerPosition _position = LevelPlayBannerPosition.BottomCenter;
                private string _placementName;
                private bool _displayOnLoad = true;
                private readonly IntPtr m_Builder = BannerAdCreateConfigBuilder();

                public void SetBidFloor(double bidFloor)
                {
                    BannerAdConfigBuilderSetBidFloor(m_Builder, bidFloor);
                }

                public void SetSize(LevelPlayAdSize size)
                {
                    _adSize = size;
                    IosLevelPlayAdSize iosAdSize = (IosLevelPlayAdSize)size.GetPlatformLevelPlayAdSize();
                    BannerAdConfigBuilderSetSize(m_Builder, iosAdSize.NativePtr);
                }

                public void SetPosition(LevelPlayBannerPosition position)
                {
                    _position = position;
                }

                public void SetPlacementName(string placementName)
                {
                    _placementName = placementName;
                    BannerAdConfigBuilderSetPlacementName(m_Builder, placementName);
                }

                public void SetDisplayOnLoad(bool displayOnLoad)
                {
                    _displayOnLoad = displayOnLoad;
                }

                public void SetRespectSafeArea(bool respectSafeArea)
                {
                    // unused
                }

                public IPlatformBannerAd.IConfig Build()
                {
                    var iosConfig = BannerAdConfigBuilderBuild(m_Builder);
                    return new Config(_adSize, _position, _placementName, _displayOnLoad, iosConfig);
                }

                [DllImport("__Internal", EntryPoint = "LPMBannerAdAdCreateConfigBuilder")]
                static extern IntPtr BannerAdCreateConfigBuilder();

                [DllImport("__Internal", EntryPoint = "LPMBannerAdAdConfigBuilderSetBidFloor")]
                static extern IntPtr BannerAdConfigBuilderSetBidFloor(IntPtr builder, double bidFloor);

                [DllImport("__Internal", EntryPoint = "LPMBannerAdConfigBuilderSetSize")]
                static extern IntPtr BannerAdConfigBuilderSetSize(IntPtr builder, IntPtr size);

                [DllImport("__Internal", EntryPoint = "LPMBannerAdConfigBuilderSetPlacementName")]
                static extern IntPtr BannerAdConfigBuilderSetPlacementName(IntPtr builder, string placementName);

                [DllImport("__Internal", EntryPoint = "LPMBannerAdAdConfigBuilderBuild")]
                static extern IntPtr BannerAdConfigBuilderBuild(IntPtr builder);
            }
        }
    }
}
#endif
