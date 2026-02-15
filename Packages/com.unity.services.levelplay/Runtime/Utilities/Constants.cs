namespace Unity.Services.LevelPlay
{
    static class Constants
    {
        internal const string k_PackageName = "com.unity.services.levelplay";
        internal const string k_PackageVersion = "9.3.0";
        const string k_PackageVersionAnnotation = "";
        internal const string k_AnnotatedPackageVersion = k_PackageVersion + k_PackageVersionAnnotation;
        internal const string k_PackageAnalyticsIdentifier = "UnityLevelPlay";
        internal const string k_UnityPackageDirectoryName = "LevelPlay";

        internal const string k_BridgeClass = "com.ironsource.unity.androidbridge.AndroidBridge";

        internal const string k_ImpressionDataKeyAuctionID = "auctionId";
        internal const string k_ImpressionDataKeyCreativeID = "creativeId";
        internal const string k_ImpressionDataKeyAdFormat = "adFormat";
        internal const string k_ImpressionDataKeyMediationAdUnitName = "mediationAdUnitName";
        internal const string k_ImpressionDataKeyMediationAdUnitID = "mediationAdUnitId";
        internal const string k_ImpressionDataKeyCountry = "country";
        internal const string k_ImpressionDataKeyAbtest = "ab";
        internal const string k_ImpressionDataKeySegmentName = "segmentName";
        internal const string k_ImpressionDataKeyPlacement = "placement";
        internal const string k_ImpressionDataKeyAdNetwork = "adNetwork";
        internal const string k_ImpressionDataKeyInstanceName = "instanceName";
        internal const string k_ImpressionDataKeyInstanceId = "instanceId";
        internal const string k_ImpressionDataKeyRevenue = "revenue";
        internal const string k_ImpressionDataKeyPrecision = "precision";
        internal const string k_ImpressionDataKeyEncryptedCpm = "encryptedCPM";
        internal const string k_ImpressionDataKeyConversionValue = "conversionValue";

        internal const string k_LevelPlaySettingName = "LevelPlayMediationSettings";
        internal const string k_LevelPlayNetworkSettingName = "LevelPlayMediatedNetworkSettings";
        internal const string k_LevelPlayResourcesPath = "Assets/LevelPlay/Resources";
    }
}
