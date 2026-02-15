#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Unity.Services.LevelPlay.Editor
{
    internal class Apps : List<App>
    {
        public Apps(IEnumerable<App> apps) : base(new List<App>(apps)) {}

        public App FindByPlatform(string platform) => this.FirstOrDefault(app => app.Platform == platform);
    }

    internal class App : IEquatable<App>
    {
        [JsonProperty("growAppId")]
        public string GrowAppId { get; set; }

        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("adUnits")]
        public List<AdUnit> AdUnits { get; set; }

        public bool Equals(App other)
        {
            if (other == null) return false;
            return string.Equals(GrowAppId, other.GrowAppId)
                && string.Equals(AppName, other.AppName);
        }
    }

    internal class AdUnit
    {
        [JsonProperty("adUnitName")]
        public string AdUnitName { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("adUnitId")]
        public string AdUnitId { get; set; }
    }
}
#endif
