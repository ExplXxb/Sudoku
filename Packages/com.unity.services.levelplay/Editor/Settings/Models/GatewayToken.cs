#if LEVELPLAY_DEPENDENCIES_INSTALLED
using Newtonsoft.Json;

namespace Unity.Services.LevelPlay.Editor
{
    internal class GatewayToken
    {
        [JsonProperty("token")]
        internal string Token { get; private set; }
    }
}
#endif
