using System;
using System.Collections.Generic;

namespace Unity.Services.LevelPlay.Editor
{
    class EditorAnalyticsService : IEditorAnalyticsService
    {
        private readonly IEditorAnalyticsSender m_EditorAnalyticsSender;

        internal EditorAnalyticsService(IEditorAnalyticsSender editorAnalyticsSender)
        {
            m_EditorAnalyticsSender = editorAnalyticsSender;
        }

        public void SendEventEditorClick(string component, string action)
        {
            SendEvent(component, action);
        }

        public void SendInstallAdapterEvent(string adapterName,
            string newVersion, string currentVersion)
        {
            SendEvent(LevelPlayComponent.LevelPlayNetworkManager,
                LevelPlayAction.Install + "_" + adapterName.Replace("_", "-") + "_" + newVersion);
        }

        public void SendUpdateAdapterEvent(string adapterName,
            string newVersion, string currentVersion)
        {
            SendEvent(LevelPlayComponent.LevelPlayNetworkManager,
                LevelPlayAction.Update + "_" + adapterName.Replace("_", "-") + "_" + newVersion);
        }

        public void SendUninstallAdapterEvent(string adapterName, string currentVersion)
        {
            SendEvent(LevelPlayComponent.LevelPlayNetworkManager,
                LevelPlayAction.Uninstall + "_" + adapterName.Replace("_", "-") + "_" + currentVersion);
        }

        public void SendUpdateAllAdaptersEvent()
        {
            SendEvent(LevelPlayComponent.LevelPlayNetworkManager,
                LevelPlayAction.UpdateAllAdapters);
        }

        public void SendNewSession(string packageType)
        {
            SendEvent(packageType,
                LevelPlayAction.NewSession);
        }

        public void SendInstallPackage(string component)
        {
            SendEvent(component,
                LevelPlayAction.Install
            );
        }

        public void SendInstallLPSDKEvent(string newVersion)
        {
            SendEvent(LevelPlayComponent.LevelPlayNetworkManager,
                LevelPlayAction.Install + "_Ironsource_" + newVersion);
        }

        public void SendUpdateLPSDKEvent(string newVersion, string currentVersion)
        {
            SendEvent(LevelPlayComponent.LevelPlayNetworkManager, LevelPlayAction.Update + "_Ironsource_" + newVersion);
        }

        public void SendMdrEvent(string action)
        {
            SendEvent(LevelPlayComponent.MDR,
                action
            );
        }

        public void SendCreateApps()
        {
            SendEvent(LevelPlayComponent.ProjectSettings,
                LevelPlayAction.CreateApps
            );
        }

        public void SendUpdateAll()
        {
            SendEvent(LevelPlayComponent.ProjectSettings,
                LevelPlayAction.UpdateAll);
        }

        public void SendCopyAppKey()
        {
            SendEvent(LevelPlayComponent.ProjectSettings, LevelPlayAction.CopyAppKey);
        }

        public void SendCopyAdUnit()
        {
            SendEvent(LevelPlayComponent.ProjectSettings,
                LevelPlayAction.CopyAdUnitId);
        }

        public void SendNavigateApps()
        {
            SendEvent(LevelPlayComponent.ProjectSettings, LevelPlayAction.NavigateAppKeys);
        }

        public void SendCloseSettings()
        {
            SendEvent(LevelPlayComponent.ProjectSettings,
                LevelPlayAction.CloseSettings);
        }

        public void SendProjectBound()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings, LevelPlayAction.ProjectBound);
        }

        public void SendProjectNotBound()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings, LevelPlayAction.ProjectNotBound);
        }

        public void SendProjectNotMapped()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings, LevelPlayAction.ProjectNotMapped);
        }

        public void SendUserUnauthorizedToCreate()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings,
                LevelPlayAction.UserDenyCreate);
        }

        public void SendUserUnauthorizedToRead()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings,
                LevelPlayAction.UserDenyView);
        }

        public void SendUpdateAvailable()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings,
                LevelPlayAction.DisplayUpdateAll);
        }

        public void SendCreateAppsButtonDisplayed()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings, LevelPlayAction.DisplayCreateApps);
        }

        public void SendFailedToCreateApps()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings,
                LevelPlayAction.FailCreateApp);
        }

        public void SendFailedToCreateAdUnits()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings,
                LevelPlayAction.FailCreateAdUnit);
        }

        public void SendAdUnitsAvailable()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings, LevelPlayAction.AdUnitsAvailable);
        }

        public void SendAdUnitsNotAvailable()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings, LevelPlayAction.AdUnitsNotAvailable);
        }

        public void SendAppsNotAvailable()
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings,
                LevelPlayAction.AppsNotAvailable);
        }

        public void SendOpenDashboard(string action)
        {
            SendEvent(LevelPlayComponent.SystemProjectSettings,
                action);
        }

        public void SendInteractWithSkanIdCheckBox(bool action)
        {
            SendEvent(LevelPlayComponent.LevelPlayNetworkManager, action ? LevelPlayAction.EnableSkAdNetworkId : LevelPlayAction.DisableSkAdNetworkId);
        }

        public void SendFailedToAddSkAdNetworkId(string adapterName)
        {
            SendEvent(LevelPlayComponent.PostBuild,
                $"{LevelPlayAction.FailedToAddSkanId}_{adapterName}");
        }

        public void SendInstantiateGameObject(string adFormat)
        {
            SendEvent(LevelPlayComponent.GameObject,
                $"{LevelPlayAction.Instantiate}_{adFormat}");
        }

        private void SendEvent(string component, string action)
        {
            m_EditorAnalyticsSender.Send(component, action);
        }

        internal static class LevelPlayComponent
        {
            public const string TopMenuAdsMediation = "TopMenu_AdsMediation";
            public const string ServicesMenuAdsMediation = "ServicesMenu_AdsMediation";
            public const string TopMenuDeveloperSettings = "TopMenu_DeveloperSettings";
            public const string ServicesMenuDeveloperSettings = "ServicesMenu_DeveloperSettings";
            public const string LevelPlayNetworkManager = "LevelPlay_Network_Manager";

            public const string MDR = "MDR";

            public const string UpmPackage = "upm";
            public const string UnityPackage = ".unitypackage";

            public const string PostBuild = "Post_Build";

            public const string GameObject = "GameObject";

            public const string ProjectSettings = "Project_Settings";
            public const string SystemProjectSettings = "System_Project_Settings";
        }

        internal static class LevelPlayAction
        {
            public const string OpenAppsAndAdUnits = "Open_AppsAndAdUnits";
            public const string OpenChangelog = "Open_SDKChangelog";
            public const string OpenLevelPlayMediationSettings = "Open_LevelPlayMediationSettings";
            public const string OpenMediatedNetworkSettings = "Open_MediatedNetworkSettings";
            public const string OpenNetworkManager = "Open_NetworkManager";
            public const string OpenDocumentation = "Open_Documentation";
            public const string OpenTroubleShootingGuide = "Open_TroubleShootingGuide";

            public const string ClickButton_UpdatePackage = "ClickButton_UpdatePackage";

            public const string NewSession = "NewSession";
            public const string Install = "Install";
            public const string Update = "Update";
            public const string Uninstall = "Uninstall";
            public const string UpdateAllAdapters = "UpdateAllAdapters";

            public const string EnableSkAdNetworkId = "Enable_SkAdNetworkId";
            public const string DisableSkAdNetworkId = "Disable_SkAdNetworkId";

            public const string FailedToAddSkanId = "FailedToAddSkanId";

            public const string Instantiate = "Instantiate";

            public const string MDRWindowDisplayed = "Display_Import_Window";
            public const string MDRImport = "Click_Import";
            public const string MDRIgnore = "Click_Ignore";
            public const string MDRCancel = "Click_Cancel";

            public const string CreateApps = "Create_Apps";
            public const string UpdateAll = "Update_All";
            public const string CopyAppKey = "Copy_AppKey";
            public const string CopyAdUnitId = "Copy_AdUnitId";
            public const string NavigateAppKeys = "Navigate_AppKeys";
            public const string CloseSettings = "Close_Settings";

            public const string ProjectBound = "Project_Bound";
            public const string ProjectNotBound = "Project_Not_Bound";
            public const string UserDenyCreate = "User_Deny_Create";
            public const string UserDenyView = "User_Deny_View";
            public const string DisplayUpdateAll = "Display_Update_All";
            public const string DisplayCreateApps = "Display_Create_Apps";
            public const string FailCreateApp = "Fail_Create_App";
            public const string FailCreateAdUnit = "Fail_Create_AdUnit";

            public const string AdUnitsAvailable = "Ad_Units_Available";
            public const string AdUnitsNotAvailable = "Ad_Units_Not_Available";
            public const string AppsNotAvailable = "Apps_Not_Available";
            public const string ProjectNotMapped = "Project_Not_Mapped";

            public const string OpenDashboardNotMapped = "Open_Dashboard_Not_Mapped";
            public const string OpenDashboardWithNoApps = "Open_Dashboard_With_No_Apps";
            public const string OpenDashboardWithNoAdUnits = "Open_Dashboard_With_No_AdUnits";
        }
    }
}
