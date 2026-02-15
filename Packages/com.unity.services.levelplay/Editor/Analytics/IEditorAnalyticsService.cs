using System;

namespace Unity.Services.LevelPlay.Editor
{
    interface IEditorAnalyticsService
    {
        void SendEventEditorClick(string component, string action);
        void SendInstallAdapterEvent(string adapterName, string newVersion, string currentVersion);
        void SendInstallLPSDKEvent(string newVersion);
        void SendUpdateLPSDKEvent(string newVersion, string currentVersion);
        void SendUpdateAdapterEvent(string adapterName, string newVersion, string currentVersion);
        void SendUninstallAdapterEvent(string adapterName, string currentVersion);
        void SendUpdateAllAdaptersEvent();
        void SendNewSession(string packageType);
        void SendInstallPackage(string component);
        void SendInteractWithSkanIdCheckBox(bool action);
        void SendFailedToAddSkAdNetworkId(string adapterName);
        void SendInstantiateGameObject(string adFormat);
        void SendMdrEvent(string action);

        void SendCreateApps();
        void SendUpdateAll();
        void SendCopyAppKey();
        void SendCopyAdUnit();
        void SendNavigateApps();
        void SendCloseSettings();

        void SendProjectBound();
        void SendProjectNotBound();
        void SendProjectNotMapped();
        void SendUserUnauthorizedToCreate();
        void SendUserUnauthorizedToRead();
        void SendUpdateAvailable();
        void SendCreateAppsButtonDisplayed();
        void SendFailedToCreateApps();
        void SendFailedToCreateAdUnits();
        void SendAdUnitsAvailable();
        void SendAdUnitsNotAvailable();
        void SendAppsNotAvailable();
        void SendOpenDashboard(string userGroup);
    }
}
