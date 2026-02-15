#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Unity.Services.LevelPlay.Editor
{
    internal sealed class HttpContentJsonDeserializer : IHttpContentDeserializable
    {
        static readonly string[] s_SupportedMediaTypes = { "application/json", "text/json", "application/*+json" };

        static readonly JsonSerializerSettings s_Settings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public bool CanDeserialize(string mediaType) =>
            s_SupportedMediaTypes.Any(s => mediaType?.StartsWith(s, StringComparison.CurrentCultureIgnoreCase) == true);

        public async Task<T> DeserializeAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
        {
            await using var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonTextReader = new JsonTextReader(streamReader);

            var serializer = JsonSerializer.Create(s_Settings);

            var result = serializer.Deserialize<T>(jsonTextReader);
            if (result == null) throw new JsonSerializationException("Deserialization failed: Invalid or empty JSON response.");
            return result;
        }
    }
}
#endif
