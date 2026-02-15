using System.IO;
using Unity.Services.LevelPlay;
using UnityEditor;

[CustomEditor(typeof(LevelPlayMediationSettings))]
public class LevelPlayMediationSettingsInspector : UnityEditor.Editor
{
    static LevelPlayMediationSettings s_levelPlayMediationSettings;

    public static LevelPlayMediationSettings LevelPlayMediationSettings
    {
        get
        {
            if (s_levelPlayMediationSettings == null)
            {
                s_levelPlayMediationSettings = AssetDatabase.LoadAssetAtPath<LevelPlayMediationSettings>(LevelPlayMediationSettings.s_LevelPlaySettingsAssetPath);
                if (s_levelPlayMediationSettings == null)
                {
                    LevelPlayMediationSettings asset = CreateInstance<LevelPlayMediationSettings>();
                    Directory.CreateDirectory(Constants.k_LevelPlayResourcesPath);
                    AssetDatabase.CreateAsset(asset, LevelPlayMediationSettings.s_LevelPlaySettingsAssetPath);
                    s_levelPlayMediationSettings = asset;
                }
            }

            return s_levelPlayMediationSettings;
        }
    }

    SerializedObject serializedSettings;

    void OnEnable()
    {
        serializedSettings = new SerializedObject(LevelPlayMediationSettings);
    }

    public override void OnInspectorGUI()
    {
        serializedSettings.Update();

        var toggleProp = serializedSettings.FindProperty(nameof(LevelPlayMediationSettings.EnableIronsourceSDKInitAPI));
        var androidKeyProp = serializedSettings.FindProperty(nameof(LevelPlayMediationSettings.AndroidAppKey));
        var iosKeyProp = serializedSettings.FindProperty(nameof(LevelPlayMediationSettings.IOSAppKey));

        EditorGUILayout.PropertyField(toggleProp);

        using (new EditorGUI.DisabledScope(!toggleProp.boolValue))
        {
            EditorGUILayout.PropertyField(androidKeyProp);
            EditorGUILayout.PropertyField(iosKeyProp);
        }

        var adIdPermission =
            serializedSettings.FindProperty(nameof(LevelPlayMediationSettings.DeclareAD_IDPermission));
        EditorGUILayout.PropertyField(adIdPermission);

        var adapterDebug = serializedSettings.FindProperty(nameof(LevelPlayMediationSettings.EnableAdapterDebug));
        EditorGUILayout.PropertyField(adapterDebug);

        var integrationHelper = serializedSettings.FindProperty(nameof(LevelPlayMediationSettings.EnableIntegrationHelper));
        EditorGUILayout.PropertyField(integrationHelper);

        serializedSettings.ApplyModifiedProperties();
    }
}
