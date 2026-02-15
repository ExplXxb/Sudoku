using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Services.LevelPlay.Editor
{
    internal interface IResponseHandler
    {
        Task<T> HandleAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default);
    }
}