#if UNITY_ANDROID
using System;
using System.IO;
using System.Xml;
using UnityEditor.Android;

namespace Unity.Services.LevelPlay.Editor
{
    class LevelPlayAndroidManifestProcessor : IPostGenerateGradleAndroidProject
    {
        const string k_AndroidXmlNs = "http://schemas.android.com/apk/res/android";

        const string k_MetaDataElementName = "meta-data";
        const string k_UsesPermissionElementName = "uses-permission";
        const string k_NameTag = "name";
        const string k_ValueTag = "value";

        const string k_ApplicationIdParam = "com.google.android.gms.ads.APPLICATION_ID";
        const string k_NetworkStateParam = "android.permission.ACCESS_NETWORK_STATE";
        const string k_AdIdParam = "com.google.android.gms.permission.AD_ID";

        public int callbackOrder { get; }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            var manifestFilePath = Path.Combine(path, "src", "main", "AndroidManifest.xml");
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(manifestFilePath);
            var manifestNode = xmlDoc.SelectSingleNode("manifest");
            var applicationNode = xmlDoc.SelectSingleNode("manifest/application");

            try
            {
                CreateXmlElement(xmlDoc, manifestNode, k_AndroidXmlNs, manifestFilePath, k_UsesPermissionElementName, k_NameTag,k_NetworkStateParam);
            }
            catch (Exception)
            {
                EditorServices.Instance.LevelPlayLogger.LogError("Failed to Add Network State permission to the manifest file.");
            }

            if (EditorServices.Instance.FileService.Exists(LevelPlayMediationNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH))
            {
                if (LevelPlayMediationNetworkSettingsInspector.LevelPlayMediationNetworkSettings.EnableAdmob)
                {
                    var appId = LevelPlayMediationNetworkSettingsInspector.LevelPlayMediationNetworkSettings
                        .AdmobAndroidAppId;

                    if (!string.IsNullOrEmpty(appId))
                    {
                        try
                        {
                            CreateXmlElement(xmlDoc, applicationNode,k_AndroidXmlNs, manifestFilePath, k_MetaDataElementName, k_NameTag,k_ApplicationIdParam,k_ValueTag,appId);
                        }
                        catch (Exception)
                        {
                            EditorServices.Instance.LevelPlayLogger.LogError("Failed to Add AdMob Application Id to the manifest file.");
                        }
                    }
                    else
                    {
                        EditorServices.Instance.LevelPlayLogger.LogWarning("AdMob Android App Id is Invalid, visit Ads Mediation/Developer Settings/Mediated Network Settings for more information.");
                    }
                }
            }

            if (EditorServices.Instance.FileService.Exists(LevelPlayMediationSettings.s_LevelPlaySettingsAssetPath))
            {
                if (LevelPlayMediationSettingsInspector.LevelPlayMediationSettings.DeclareAD_IDPermission)
                {
                    try
                    {
                        CreateXmlElement(xmlDoc, manifestNode,k_AndroidXmlNs, manifestFilePath, k_UsesPermissionElementName, k_NameTag,k_AdIdParam);
                    }
                    catch (Exception)
                    {
                        EditorServices.Instance.LevelPlayLogger.LogError("Failed to Add App Ad Id permission to the manifest file.");
                    }
                }
            }
        }
        void CreateXmlElement(XmlDocument doc, XmlNode parentNode,string xmlNamespace, string path, string tagName,
            string firstAttributeName, string firstAttributeValue, string secondAttributeName = null, string secondAttributeValue = null)
        {
            foreach (var childNode in parentNode.ChildNodes)
            {
                var childElement = childNode as XmlElement;
                if (childElement?.Name != tagName)
                {
                    continue;
                }

                if (childElement != null && childElement.HasAttributes)
                {
                    var firstAttribute = childElement.GetAttributeNode(firstAttributeName, xmlNamespace);
                    if (firstAttribute == null || firstAttribute.Value != firstAttributeValue)
                        continue;

                    if (secondAttributeName == null || secondAttributeValue == null)
                    {
                        return;
                    }

                    var secondAttribute = childElement.GetAttributeNode(secondAttributeName, xmlNamespace);
                    if (secondAttribute != null)
                    {
                        secondAttribute.Value = secondAttributeValue;
                        return;
                    }

                    childElement.SetAttribute(secondAttributeName, k_AndroidXmlNs, secondAttributeValue);
                    return;
                }
            }
            var metaDataElement = doc.CreateElement(tagName);
            metaDataElement.SetAttribute(firstAttributeName, k_AndroidXmlNs, firstAttributeValue);
            if (secondAttributeName != null && secondAttributeValue != null)
            {
                metaDataElement.SetAttribute(secondAttributeName, k_AndroidXmlNs, secondAttributeValue);
            }
            parentNode.AppendChild(metaDataElement);
            doc.Save(path);
        }
    }
}
#endif
