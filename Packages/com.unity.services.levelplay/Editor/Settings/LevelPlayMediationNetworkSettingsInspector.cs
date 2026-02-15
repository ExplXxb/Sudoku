using System.IO;
using Unity.Services.LevelPlay;
using UnityEditor;

[CustomEditor(typeof(LevelPlayMediationNetworkSettings))]
public class LevelPlayMediationNetworkSettingsInspector : Editor
{
    static LevelPlayMediationNetworkSettings _levelPlayMediationNetworkSettings;

    public static LevelPlayMediationNetworkSettings LevelPlayMediationNetworkSettings
    {
        get
        {
            if (_levelPlayMediationNetworkSettings == null)
            {
                _levelPlayMediationNetworkSettings = AssetDatabase.LoadAssetAtPath<LevelPlayMediationNetworkSettings>(LevelPlayMediationNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH);
                if (_levelPlayMediationNetworkSettings == null)
                {
                    LevelPlayMediationNetworkSettings asset = CreateInstance<LevelPlayMediationNetworkSettings>();
                    Directory.CreateDirectory(Constants.k_LevelPlayResourcesPath);
                    AssetDatabase.CreateAsset(asset, LevelPlayMediationNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH);
                    _levelPlayMediationNetworkSettings = asset;
                }
            }
            return _levelPlayMediationNetworkSettings;
        }
    }
}
