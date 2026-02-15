using System.Threading;
using System.Threading.Tasks;

namespace Unity.Services.LevelPlay.Editor
{
    internal interface INetworkingService
    {
        Task<T> SendAsync<T>(Request request, CancellationToken cancellationToken = default);
    }
}