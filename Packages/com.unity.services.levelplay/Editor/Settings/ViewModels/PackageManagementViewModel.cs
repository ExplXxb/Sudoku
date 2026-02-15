#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;

namespace Unity.Services.LevelPlay.Editor
{
    internal class PackageManagementViewModel
    {
        internal enum Status
        {
            UpToDate,
            UpdateAvailable,
            NotInstalled,
            Error
        }

        public Status PackageStatus { get; set; }
        public Status SdkStatus { get; set; }
        public Status UnityAdsStatus { get; set; }

        string InstalledPackageVersion { get; set; }
        string LatestPackageVersion { get; set; }

        string InstalledSdkVersion { get; set; }
        string LatestSdkVersion { get; set; }

        string InstalledUnityAdsVersion { get; set; }
        string LatestUnityAdsVersion { get; set; }

        readonly ILevelPlayNetworkManager m_NetworkManager;
        readonly SdkInstaller m_SdkInstaller;
        readonly ILevelPlayLogger m_Logger;

        PackageManagementViewModel(ILevelPlayNetworkManager networkManager, ILevelPlayLogger logger)
        {
            m_NetworkManager = networkManager;
            m_SdkInstaller = new SdkInstaller(logger, networkManager);
            m_Logger = logger;

            CheckInstallStatus(networkManager);
        }

        async Task UpdateUnityPackageAsync()
        {
            if (PackageStatus != Status.UpdateAvailable) return;

            var latestPackageVersion = m_NetworkManager.LatestUnityPackageVersion();

            var packageType = LevelPlayDependenciesManager.LevelPlayPackageType();
            switch (packageType)
            {
                case LevelPlayDependenciesManager.PackageType.DotUnityPackage:
                    await m_NetworkManager.Install(latestPackageVersion);
                    break;
                case LevelPlayDependenciesManager.PackageType.Upm:
                {
                    var packageVersion = Constants.k_PackageName + "@" + latestPackageVersion.Version;
                    var request = Client.Add(packageVersion);
                    while (!request.IsCompleted)
                        await Task.Yield();

                    if (request.Status == StatusCode.Failure)
                    {
                        var message = $"Failed to install package {packageVersion} with : {request.Error}";
                        m_Logger.LogError(message);
                    }

                    break;
                }
                case LevelPlayDependenciesManager.PackageType.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            AssetDatabase.Refresh();
        }

        public async Task UpdateAllAsync()
        {
            EditorServices.Instance.EditorAnalyticsService.SendUpdateAll();
            await UpdateUnityPackageAsync();
            await m_SdkInstaller.PreInstallAsync();
            await m_SdkInstaller.InstallLatestIronSourceSdkAsync();
            await m_SdkInstaller.InstallUnityAdsAdapterAsync(true);

            CheckInstallStatus(m_NetworkManager);
        }

        public static async Task<PackageManagementViewModel> CreateAsync(ILevelPlayNetworkManager networkManager, ILevelPlayLogger logger)
        {
            await networkManager.GetVersionsWebRequest();
            networkManager.LoadVersionsFromJson();
            var viewModel = new PackageManagementViewModel(networkManager, logger);
            return viewModel;
        }

        public bool HasUpdate =>
            PackageStatus == Status.UpdateAvailable ||
            SdkStatus == Status.UpdateAvailable ||
            UnityAdsStatus == Status.UpdateAvailable;

        void CheckInstallStatus(ILevelPlayNetworkManager networkManager)
        {
            InstalledPackageVersion = networkManager.UnityPackageVersion();
            LatestPackageVersion = networkManager.LatestUnityPackageVersion().Version;
            PackageStatus = InstalledPackageVersion == LatestPackageVersion ? Status.UpToDate : Status.UpdateAvailable;

            InstalledSdkVersion = networkManager.InstalledSdkVersionString();
            LatestSdkVersion = networkManager.LatestSdkVersion().Version;
            if (InstalledSdkVersion == null)
                SdkStatus = Status.NotInstalled;
            else if (InstalledSdkVersion == LatestSdkVersion)
                SdkStatus = Status.UpToDate;
            else
                SdkStatus = Status.UpdateAvailable;

            var unityAdapter = networkManager.Adapters.Values.FirstOrDefault(adapter => adapter.KeyName == EditorConstants.k_UnityAdapterName);
            if (unityAdapter != null)
            {
                InstalledUnityAdsVersion = networkManager.InstalledAdapterVersionString(unityAdapter);
                var unityLatestAdapter = networkManager.CompatibleAdapterVersions(unityAdapter).FirstOrDefault();
                LatestUnityAdsVersion = unityLatestAdapter?.Version;
                if (InstalledUnityAdsVersion == null)
                    UnityAdsStatus = Status.NotInstalled;
                else if (InstalledUnityAdsVersion == LatestUnityAdsVersion)
                    UnityAdsStatus = Status.UpToDate;
                else
                    UnityAdsStatus = Status.UpdateAvailable;
            }
            else
            {
                InstalledUnityAdsVersion = null;
                LatestUnityAdsVersion = null;
                UnityAdsStatus = Status.Error;
            }
        }
    }
}
#endif
