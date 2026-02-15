namespace Unity.Services.LevelPlay.Editor
{
    class EditorServices : IEditorServices
    {
        static readonly EditorServices k_Instance = new EditorServices();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static EditorServices()
        {
        }

        internal EditorServices()
        {
        }

        // For Testing Only
        internal EditorServices(IPackageTypeService packageTypeService)
        {
            m_PackageTypeService = packageTypeService;
        }

        internal static EditorServices Instance => k_Instance;

        readonly object m_PropertyLock = new object();

        IFileService m_FileService;
        public IFileService FileService
        {
            get
            {
                var directoryService = this.DirectoryService;
                lock (m_PropertyLock) {
                    if (m_FileService == null)
                    {
                        m_FileService = new FileService(directoryService);
                    }
                    return m_FileService;
                }
            }
        }

        IAssetDatabaseService m_AssetDatabaseService;
        public IAssetDatabaseService AssetDatabaseService
        {
            get
            {
                lock (m_PropertyLock) {
                    if (m_AssetDatabaseService == null)
                    {
                        m_AssetDatabaseService = new AssetDatabaseService();
                    }
                    return m_AssetDatabaseService;
                }
            }
        }

        IDirectoryService m_DirectoryService;
        public IDirectoryService DirectoryService
        {
            get
            {
                lock (m_PropertyLock) {
                    if (m_DirectoryService == null)
                    {
                        m_DirectoryService = new DirectoryService();
                    }
                    return m_DirectoryService;
                }
            }
        }

        IXmlDocumentFactory m_XmlDocumentFactory;
        public IXmlDocumentFactory XmlDocumentFactory
        {
            get
            {
                lock (m_PropertyLock) {
                    if (m_XmlDocumentFactory == null)
                    {
                        m_XmlDocumentFactory = new XmlDocumentFactory();
                    }
                    return m_XmlDocumentFactory;
                }
            }
        }

        INetworkingService m_NetworkingService;
        public INetworkingService NetworkingService
        {
            get
            {
                lock (m_PropertyLock)
                {
                    return m_NetworkingService ?? (m_NetworkingService = new NetworkingService());
                }
            }
        }

        IWebRequestService m_WebRequestService;
        public IWebRequestService WebRequestService
        {
            get
            {
                lock (m_PropertyLock) {
                    if (m_WebRequestService == null)
                    {
                        m_WebRequestService = new WebRequestService();
                    }
                    return m_WebRequestService;
                }
            }
        }

        ILevelPlayNetworkManager m_LevelPlayNetworkManager;
        public ILevelPlayNetworkManager LevelPlayNetworkManager
        {
            get
            {
                var fileService = this.FileService;
                var assetDataBaseService = this.AssetDatabaseService;
                var xmlDocumentFactory = this.XmlDocumentFactory;
                var webRequestService = this.WebRequestService;
                var levelPlayLogger = this.LevelPlayLogger;
                lock (m_PropertyLock) {
                    if (m_LevelPlayNetworkManager == null)
                    {
                        m_LevelPlayNetworkManager = new LevelPlayNetworkManager(fileService, assetDataBaseService, xmlDocumentFactory, webRequestService, levelPlayLogger);
                    }
                    return m_LevelPlayNetworkManager;
                }
            }
        }

        IEditorAnalyticsSender m_EditorAnalyticsSender;
        public IEditorAnalyticsSender EditorAnalyticsSender
        {
            get
            {
                lock (m_PropertyLock) {
                    if (m_EditorAnalyticsSender == null)
                    {
                        m_EditorAnalyticsSender = new EditorAnalyticsSender();
                    }
                    return m_EditorAnalyticsSender;
                }
            }
        }

        IEditorAnalyticsService m_EditorAnalyticsService;
        public IEditorAnalyticsService EditorAnalyticsService
        {
            get
            {
                var editorAnalyticsSender = this.EditorAnalyticsSender;
                lock (m_PropertyLock) {
                    if (m_EditorAnalyticsService == null)
                    {
                        m_EditorAnalyticsService = new EditorAnalyticsService(editorAnalyticsSender);
                    }
                    return m_EditorAnalyticsService;
                }
            }
        }

        ILevelPlayLogger m_LevelPlayLogger;
        public ILevelPlayLogger LevelPlayLogger
        {
            get
            {
                lock (m_PropertyLock) {
                    if (m_LevelPlayLogger == null)
                    {
                        m_LevelPlayLogger = new LevelPlayLogger();
                    }
                    return m_LevelPlayLogger;
                }
            }
        }

        IOnLoadService m_OnLoadService;
        public IOnLoadService OnLoadService
        {
            get
            {
                var ironSourceSdkInstaller = this.LevelPlaySdkInstaller;
                lock (m_PropertyLock) {
                    if (m_OnLoadService == null)
                    {
                        m_OnLoadService = new OnLoadService(ironSourceSdkInstaller);
                    }
                    return m_OnLoadService;
                }
            }
        }

        ILevelPlaySdkInstaller m_LevelPlaySdkInstaller;
        public ILevelPlaySdkInstaller LevelPlaySdkInstaller
        {
            get
            {
                var packageTypeService = this.PackageTypeService;
                var logger = this.LevelPlayLogger;
                var levelPlayNetworkManager = this.LevelPlayNetworkManager;
                var fileService = this.FileService;
                var editorAnalyticsService = this.EditorAnalyticsService;
                lock (m_PropertyLock) {
                    if (m_LevelPlaySdkInstaller == null)
                    {
                        switch (packageTypeService.PackageType)
                        {
                            case PackageType.Upm:
                                m_LevelPlaySdkInstaller = new UpmLevelPlaySdkInstaller(logger, levelPlayNetworkManager);
                                break;
                            case PackageType.DotUnityPackage:
                                m_LevelPlaySdkInstaller = new DotUnityPackageLevelPlaySdkInstaller(logger, levelPlayNetworkManager, fileService, editorAnalyticsService);
                                break;
                        }
                    }
                    return m_LevelPlaySdkInstaller;
                }
            }
        }

        IPackageTypeService m_PackageTypeService;
        public IPackageTypeService PackageTypeService
        {
            get
            {
                var directoryService = this.DirectoryService;
                lock (m_PropertyLock) {
                    if (m_PackageTypeService == null)
                    {
                        m_PackageTypeService = new PackageTypeService(directoryService);
                    }
                    return m_PackageTypeService;
                }
            }
        }
    }
}
