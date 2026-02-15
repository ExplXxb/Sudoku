#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Unity.Services.LevelPlay.Editor
{
    internal class HttpContentJsonSerializer : IHttpContentSerializable
    {
        static readonly string[] s_SupportedMediaTypes = { "application/json", "text/json", "application/*+json" };

        static readonly JsonSerializerSettings s_Settings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public bool CanSerialize(string mediaType) =>
            s_SupportedMediaTypes.Any(s => mediaType?.StartsWith(s, StringComparison.CurrentCultureIgnoreCase) == true);

        public byte[] Serialize<T>(T model)
        {
            var json = SerializeToString(model);
            return Encoding.UTF8.GetBytes(json);
        }

        public string SerializeToString<T>(T model)
            => JsonConvert.SerializeObject(model, s_Settings);
    }
}
#endif
