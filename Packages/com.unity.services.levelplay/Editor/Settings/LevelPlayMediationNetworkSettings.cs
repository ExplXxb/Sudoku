using System.IO;
using Unity.Services.LevelPlay;
using UnityEngine;

public class LevelPlayMediationNetworkSettings : ScriptableObject
{
    public static readonly string MEDIATION_SETTINGS_ASSET_PATH = Path.Combine(Constants.k_LevelPlayResourcesPath, Constants.k_LevelPlayNetworkSettingName + ".asset");

    [Header("")]
    [Header("AdMob Integration")]
    [SerializeField]
    [Tooltip("This will add AdMob Application ID to AndroidManifest.xml/info.plist")]
    public bool EnableAdmob = false;

    [SerializeField]
    [Tooltip("This Will be added to your AndroidManifest.xml")]
    public string AdmobAndroidAppId = string.Empty;

    [SerializeField]
    [Tooltip("This will be added to your info.plist")]
    public string AdmobIOSAppId = string.Empty;
}
