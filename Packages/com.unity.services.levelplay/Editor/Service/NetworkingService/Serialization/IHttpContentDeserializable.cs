using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Services.LevelPlay.Editor
{
    internal interface IHttpContentDeserializable : IDeserializable
    {
        Task<T> DeserializeAsync<T>(HttpContent content, CancellationToken cancellationToken = default);
    }
}
