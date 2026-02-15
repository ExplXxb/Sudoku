#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity.Services.LevelPlay;
using Unity.Services.LevelPlay.Editor;

class LevelPlayMenu : Editor
{
    const string k_AdsMediationRoot = "Ads Mediation/";
    const string k_ServicesAdsMediationRoot = "Services/Ads Mediation (LevelPlay)/";
    const string k_NetworkManager = "Network Manager";
    const string k_AppsAndAdUnits = "Apps and Ad Units (Alpha)";
    const string k_DeveloperSettings = "Developer Settings/";
    const string k_LevelPlayMediationSettings = "LevelPlay Mediation Settings";
    const string k_MediatedNetworkSettings = "Mediated Network Settings";
    const string k_Support = "Support/";
    const string k_Documentation = "Documentation";
    const string k_Changelog = "Changelog";
    const string k_Troubleshooting = "Troubleshooting";

    [MenuItem(k_ServicesAdsMediationRoot + k_NetworkManager, false, 0)]
    public static void OpenNetworkManagerAdsMediation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.ServicesMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenNetworkManager);

        OpenNetworkManager();
    }

    [MenuItem(k_AdsMediationRoot + k_NetworkManager, false, 0)]
    public static void OpenNetworkManagerServices()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenNetworkManager);

        OpenNetworkManager();
    }

    private static void OpenNetworkManager()
    {
        LevelPlayDependenciesManager.ShowDependenciesManager();
    }

    [MenuItem(k_AdsMediationRoot + k_AppsAndAdUnits, false, 1)]
    public static void OpenAppsAndAdUnitsAdsMediation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenAppsAndAdUnits);

        OpenAppsAndAdUnits();
    }

    [MenuItem(k_ServicesAdsMediationRoot + k_AppsAndAdUnits, false, 1)]
    public static void OpenAppsAndAdUnitsServices()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.ServicesMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenAppsAndAdUnits);

        OpenAppsAndAdUnits();
    }

    private static void OpenAppsAndAdUnits()
    {
        SettingsService.OpenProjectSettings("Project/Services/Ads Mediation (LevelPlay)");
    }

    [MenuItem(k_AdsMediationRoot + k_DeveloperSettings + k_LevelPlayMediationSettings, false, 2)]
    public static void OpenLevelPlayMediationSettingsAdsMediation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuDeveloperSettings,
            EditorAnalyticsService.LevelPlayAction.OpenLevelPlayMediationSettings);

        OpenLevelPlayMediationSettings();
    }

    [MenuItem(k_ServicesAdsMediationRoot + k_DeveloperSettings + k_LevelPlayMediationSettings, false, 2)]
    public static void OpenLevelPlayMediationSettingsServices()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.ServicesMenuDeveloperSettings,
            EditorAnalyticsService.LevelPlayAction.OpenLevelPlayMediationSettings);

        OpenLevelPlayMediationSettings();
    }

    private static void OpenLevelPlayMediationSettings()
    {
        var path = "Assets/LevelPlay/Resources";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }


        var levelPlayMediationSettings = Resources.Load<LevelPlayMediationSettings>(Constants.k_LevelPlaySettingName);
        if (levelPlayMediationSettings == null)
        {
            levelPlayMediationSettings = CreateInstance<LevelPlayMediationSettings>();
            AssetDatabase.CreateAsset(levelPlayMediationSettings, LevelPlayMediationSettings.s_LevelPlaySettingsAssetPath);
            levelPlayMediationSettings = Resources.Load<LevelPlayMediationSettings>(Constants.k_LevelPlaySettingName);
        }

        Selection.activeObject = levelPlayMediationSettings;
    }

    [MenuItem(k_AdsMediationRoot + k_DeveloperSettings + k_MediatedNetworkSettings, false, 3)]
    public static void OpenMediatedNetworkSettingsAdsMediation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuDeveloperSettings,
            EditorAnalyticsService.LevelPlayAction.OpenMediatedNetworkSettings);

        OpenMediatedNetworkSettings();
    }

    [MenuItem(k_ServicesAdsMediationRoot + k_DeveloperSettings + k_MediatedNetworkSettings, false, 3)]
    public static void OpenMediatedNetworkSettingsServices()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.ServicesMenuDeveloperSettings,
            EditorAnalyticsService.LevelPlayAction.OpenMediatedNetworkSettings);

        OpenMediatedNetworkSettings();
    }

    private static void OpenMediatedNetworkSettings()
    {
        var path = Constants.k_LevelPlayResourcesPath;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var levelPlayMediationNetworkSettings = Resources.Load<LevelPlayMediationNetworkSettings>(Constants.k_LevelPlayNetworkSettingName);
        if (levelPlayMediationNetworkSettings == null)
        {
            levelPlayMediationNetworkSettings = CreateInstance<LevelPlayMediationNetworkSettings>();
            AssetDatabase.CreateAsset(levelPlayMediationNetworkSettings, LevelPlayMediationNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH);
            levelPlayMediationNetworkSettings = Resources.Load<LevelPlayMediationNetworkSettings>(Constants.k_LevelPlayNetworkSettingName);
        }

        Selection.activeObject = levelPlayMediationNetworkSettings;
    }

    [MenuItem(k_AdsMediationRoot + k_Support + k_Documentation, false, 13)]
    public static void OpenDocumentationAdsMediation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenDocumentation);

        OpenDocumentation();
    }

    [MenuItem(k_ServicesAdsMediationRoot + k_Support + k_Documentation, false, 13)]
    public static void OpenDocumentationServices()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.ServicesMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenDocumentation);

        OpenDocumentation();
    }

    private static void OpenDocumentation()
    {
        Application.OpenURL("https://developers.is.com/ironsource-mobile/unity/unity-plugin/");
    }

    [MenuItem(k_AdsMediationRoot + k_Support + k_Changelog, false, 14)]
    public static void OpenChangelogAdsMediation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenChangelog);

        OpenChangelog();
    }

    [MenuItem(k_ServicesAdsMediationRoot + k_Support + k_Changelog, false, 14)]
    public static void OpenChangelogServices()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.ServicesMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenChangelog);

        OpenChangelog();
    }

    private static void OpenChangelog()
    {
        Application.OpenURL("https://developers.is.com/ironsource-mobile/unity/sdk-change-log/");
    }

    [MenuItem(k_AdsMediationRoot + k_Support + k_Troubleshooting, false, 15)]
    public static void OpenTroubleShootingGuideAdsMediation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenTroubleShootingGuide);

        OpenTroubleShootingGuide();
    }

    [MenuItem(k_ServicesAdsMediationRoot + k_Support + k_Troubleshooting, false, 15)]
    public static void OpenTroubleShootingGuideServices()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.ServicesMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenTroubleShootingGuide);

        OpenTroubleShootingGuide();
    }

    private static void OpenTroubleShootingGuide()
    {
        Application.OpenURL("https://docs.unity.com/en-us/grow/dashboard/levelplay/troubleshoot");
    }
}
#endif
