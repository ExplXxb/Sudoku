using System;
using System.Collections.Generic;
using System.Globalization;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Represents data for an ad impression event.
    /// </summary>
    public sealed class LevelPlayImpressionData
    {
        /// <summary>
        /// The id for the auction.
        /// </summary>
        public string AuctionId => GetValueAsString(Constants.k_ImpressionDataKeyAuctionID);

        /// <summary>
        /// The creative id of the ad campaign.
        /// </summary>
        public string CreativeId => GetValueAsString(Constants.k_ImpressionDataKeyCreativeID);

        /// <summary>
        /// The format of the ad.
        /// </summary>
        public string AdFormat => GetValueAsString(Constants.k_ImpressionDataKeyAdFormat);

        /// <summary>
        /// The mediation ad unit name of the ad.
        /// </summary>
        public string MediationAdUnitName =>
            GetValueAsString(Constants.k_ImpressionDataKeyMediationAdUnitName);

        /// <summary>
        /// The mediation ad unit id of the ad.
        /// </summary>
        public string MediationAdUnitId =>
            GetValueAsString(Constants.k_ImpressionDataKeyMediationAdUnitID);

        /// <summary>
        /// Country code ISO 3166-1 format.
        /// </summary>
        public string Country => GetValueAsString(Constants.k_ImpressionDataKeyCountry);

        /// <summary>
        /// Indication if AB test was activated.
        /// </summary>
        public string Ab => GetValueAsString(Constants.k_ImpressionDataKeyAbtest);

        /// <summary>
        /// The segment the user is associated with.
        /// </summary>
        public string SegmentName => GetValueAsString(Constants.k_ImpressionDataKeySegmentName);

        /// <summary>
        /// The placement the user is associated with.
        /// </summary>
        public string Placement => GetValueAsString(Constants.k_ImpressionDataKeyPlacement);

        /// <summary>
        /// The ad network name that served the ad.
        /// </summary>
        public string AdNetwork => GetValueAsString(Constants.k_ImpressionDataKeyAdNetwork);

        /// <summary>
        /// The ad network instance name as defined on the platform.
        /// </summary>
        public string InstanceName => GetValueAsString(Constants.k_ImpressionDataKeyInstanceName);

        /// <summary>
        /// The ad network instance id as defined on the platform.
        /// </summary>
        public string InstanceId => GetValueAsString(Constants.k_ImpressionDataKeyInstanceId);

        /// <summary>
        /// The revenue generated for the impression.
        /// </summary>
        public double? Revenue => GetValueAsDouble(Constants.k_ImpressionDataKeyRevenue);

        /// <summary>
        /// The source value of the revenue field.
        /// </summary>
        public string Precision => GetValueAsString(Constants.k_ImpressionDataKeyPrecision);

        /// <summary>
        /// The encrypted cpm associated with the impression. Available for some of the ad networks.
        /// </summary>
        public string EncryptedCpm => GetValueAsString(Constants.k_ImpressionDataKeyEncryptedCpm);

        /// <summary>
        /// The conversion value associated with the impression.
        /// </summary>
        public int? ConversionValue => GetValueAsInt(Constants.k_ImpressionDataKeyConversionValue);

        /// <summary>
        /// All the data associated with the impression.
        /// </summary>
        public string AllData { get; }

        private readonly Dictionary<string, object> InternalDictionary;

        internal LevelPlayImpressionData(string levelplayImpressionJson)
        {
            AllData = levelplayImpressionJson;
            InternalDictionary = ParseJson(levelplayImpressionJson);
        }

        private Dictionary<string, object> ParseJson(string json)
        {
            try
            {
                return LevelPlayJson.Deserialize(json) as Dictionary<string, object> ??
                    new Dictionary<string, object>();
            }
            catch (Exception ex)
            {
                LevelPlayLogger.LogException(ex);
                return new Dictionary<string, object>();
            }
        }

        private string GetValueAsString(string key)
        {
            return InternalDictionary.TryGetValue(key, out var value) && value != null ? value.ToString() : null;
        }

        private double? GetValueAsDouble(string key)
        {
            CultureInfo invCulture = CultureInfo.InvariantCulture;
            if (InternalDictionary.TryGetValue(key, out var value) && value != null &&
                double.TryParse(string.Format(invCulture, "{0}", value), NumberStyles.Any, invCulture, out var result))
            {
                return result;
            }

            return null;
        }

        private int? GetValueAsInt(string key)
        {
            if (InternalDictionary.TryGetValue(key, out var value) && value != null &&
                int.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Returns a string representation of the impression data.
        /// </summary>
        /// <returns>A string representation of the impression data.</returns>
        public override string ToString()
        {
            return "LevelPlayImpressionData{" +
                   "AuctionId='" + AuctionId + '\'' +
                   ", CreativeId='" + CreativeId + '\'' +
                   ", AdFormat='" + AdFormat + '\'' +
                   ", MediationAdUnitName='" + MediationAdUnitName + '\'' +
                   ", MediationAdUnitId='" + MediationAdUnitId + '\'' +
                   ", Country='" + Country + '\'' +
                   ", Ab='" + Ab + '\'' +
                   ", SegmentName='" + SegmentName + '\'' +
                   ", Placement='" + Placement + '\'' +
                   ", AdNetwork='" + AdNetwork + '\'' +
                   ", InstanceName='" + InstanceName + '\'' +
                   ", InstanceId='" + InstanceId + '\'' +
                   ", Revenue=" + Revenue +
                   ", Precision='" + Precision + '\'' +
                   ", EncryptedCpm='" + EncryptedCpm + '\'' +
                   ", ConversionValue=" + ConversionValue +
                   '}';
        }
    }
}
