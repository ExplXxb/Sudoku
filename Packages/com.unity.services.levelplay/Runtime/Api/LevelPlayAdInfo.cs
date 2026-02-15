using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Contains detailed information about a LevelPlay advertisement, including its dimensions, placement, and performance metrics.
    /// </summary>
    public sealed class LevelPlayAdInfo
    {
        // Constants for JSON keys
        const string AdIdKey = "adId";
        const string AdUnitIdKey = "adUnitId";
        const string AdUnitNameKey = "adUnitName";
        const string AdSizeKey = "adSize";
        const string AdFormatKey = "adFormat";
        const string PlacementNameKey = "placementName";
        const string AuctionIdKey = "auctionId";
        const string CountryKey = "country";
        const string AbKey = "ab";
        const string SegmentNameKey = "segmentName";
        const string AdNetworkKey = "adNetwork";
        const string InstanceNameKey = "instanceName";
        const string InstanceIdKey = "instanceId";
        const string RevenueKey = "revenue";
        const string PrecisionKey = "precision";
        const string EncryptedCpmKey = "encryptedCPM";

        const string AdSizeDescriptionKey = "description";
        const string AdSizeWidthKey = "width";
        const string AdSizeHeightKey = "height";

        public readonly string AdId;
        public readonly string AdUnitId;
        public readonly string AdUnitName;
        [CanBeNull] public readonly LevelPlayAdSize AdSize;
        public readonly string AdFormat;
        public readonly string PlacementName;
        public readonly string AuctionId;
        public readonly string CreativeId;
        public readonly string Country;
        public readonly string Ab;
        public readonly string SegmentName;
        public readonly string AdNetwork;
        public readonly string InstanceName;
        public readonly string InstanceId;
        public readonly double? Revenue;
        public readonly string Precision;
        public readonly string EncryptedCPM;

        internal LevelPlayAdInfo(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            object obj;
            double parsedDouble;
            try
            {
                CultureInfo invCulture = CultureInfo.InvariantCulture;

                Dictionary<string, object> jsonDic =
                    LevelPlayJson.Deserialize(json) as Dictionary<string, object>;
                if (jsonDic.TryGetValue(AdIdKey, out obj) && obj != null)
                {
                    AdId = obj.ToString();
                }

                if (jsonDic.TryGetValue(AdUnitIdKey, out obj) && obj != null)
                {
                    AdUnitId = obj.ToString();
                }

                if (jsonDic.TryGetValue(AdUnitNameKey, out obj) && obj != null)
                {
                    AdUnitName = obj.ToString();
                }

                if (jsonDic.TryGetValue(AdSizeKey, out obj) && obj != null)
                {
                    AdSize = GetAdSize(obj.ToString());
                }

                if (jsonDic.TryGetValue(AdFormatKey, out obj) && obj != null)
                {
                    AdFormat = obj.ToString();
                }

                if (jsonDic.TryGetValue(PlacementNameKey, out obj) && obj != null)
                {
                    PlacementName = obj.ToString();
                }

                if (jsonDic.TryGetValue(AuctionIdKey, out obj) && obj != null)
                {
                    AuctionId = obj.ToString();
                }

                if (jsonDic.TryGetValue(Constants.k_ImpressionDataKeyCreativeID, out obj) && obj != null)
                {
                    CreativeId = obj.ToString();
                }

                if (jsonDic.TryGetValue(CountryKey, out obj) && obj != null)
                {
                    Country = obj.ToString();
                }

                if (jsonDic.TryGetValue(AbKey, out obj) && obj != null)
                {
                    Ab = obj.ToString();
                }

                if (jsonDic.TryGetValue(SegmentNameKey, out obj) && obj != null)
                {
                    SegmentName = obj.ToString();
                }

                if (jsonDic.TryGetValue(AdNetworkKey, out obj) && obj != null)
                {
                    AdNetwork = obj.ToString();
                }

                if (jsonDic.TryGetValue(InstanceNameKey, out obj) && obj != null)
                {
                    InstanceName = obj.ToString();
                }

                if (jsonDic.TryGetValue(InstanceIdKey, out obj) && obj != null)
                {
                    InstanceId = obj.ToString();
                }

                if (jsonDic.TryGetValue(RevenueKey, out obj) && obj != null && double.TryParse(
                    string.Format(invCulture, "{0}", obj), NumberStyles.Any, invCulture, out parsedDouble))
                {
                    Revenue = parsedDouble;
                }

                if (jsonDic.TryGetValue(PrecisionKey, out obj) && obj != null)
                {
                    Precision = obj.ToString();
                }

                if (jsonDic.TryGetValue(EncryptedCpmKey, out obj) && obj != null)
                {
                    EncryptedCPM = obj.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("error parsing LevelPlayAdInfo" + ex.ToString());
            }
        }

        /// <summary>
        /// Retrieves the ad size from a JSON string describing the ad size.
        /// </summary>
        /// <param name="adSizeJson">The JSON string describing the ad size.</param>
        /// <returns>An instance of <see cref="LevelPlayAdSize"/> or null if parsing fails.</returns>
        static LevelPlayAdSize GetAdSize(string adSizeJson)
        {
            string description = "";
            int width = 0;
            int height = 0;
            if (!string.IsNullOrEmpty(adSizeJson))
            {
                try
                {
                    object obj;
                    Dictionary<string, object> jsonDic =
                        LevelPlayJson.Deserialize(adSizeJson) as Dictionary<string, object>;
                    if (jsonDic.TryGetValue(AdSizeDescriptionKey, out obj) && obj != null)
                    {
                        description = obj.ToString();
                    }

                    if (jsonDic.TryGetValue(AdSizeWidthKey, out obj) && obj != null)
                    {
                        width = Int32.Parse(obj.ToString());
                    }

                    if (jsonDic.TryGetValue(AdSizeHeightKey, out obj) && obj != null)
                    {
                        height = Int32.Parse(obj.ToString());
                    }

                    LevelPlayAdSize adSize = GetAdSize(description, width, height);
                    return adSize;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            return null;
        }

        static LevelPlayAdSize GetAdSize(string description, int width = 0, int height = 0)
        {
            switch (description)
            {
                case "BANNER":
                    return LevelPlayAdSize.BANNER;
                case "LARGE":
                    return LevelPlayAdSize.LARGE;
                case "MEDIUM_RECTANGLE":
                    return LevelPlayAdSize.MEDIUM_RECTANGLE;
                case "CUSTOM":
                    return LevelPlayAdSize.CreateCustomBannerSize(width, height);
                case "LEADERBOARD":
                    return LevelPlayAdSize.LEADERBOARD;
                case "ADAPTIVE":
                    return LevelPlayAdSize.CreateAdaptiveAdSize(width);
                default:
                    return LevelPlayAdSize.BANNER;
            }
        }

        public override string ToString()
        {
            return $"adId: {AdId}, adUnitId: {AdUnitId}, adUnitName: {AdUnitName}, adSize: {AdSize}, adFormat: {AdFormat}, placementName: {PlacementName}, auctionId: {AuctionId}, creativeId: {CreativeId}, country: {Country}, ab: {Ab}, segmentName: {SegmentName}, adNetwork: {AdNetwork}, instanceName: {InstanceName}, instanceId: {InstanceId}, revenue: {Revenue}, precision: {Precision}, encryptedCPM: {EncryptedCPM}";
        }
    }
}
