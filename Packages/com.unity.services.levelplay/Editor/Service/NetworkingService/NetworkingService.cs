using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Unity.Services.LevelPlay.Editor
{
    internal class NetworkingService : INetworkingService, IDisposable
    {
        readonly HttpClient m_HttpClient;
        readonly IResponseHandler m_ResponseHandler;

        public bool ConfigureAwait { get; set; } = false;

        public NetworkingService(HttpMessageHandler? handler = null, IResponseHandler? responseHandler = null)
        {
            m_HttpClient = new HttpClient(handler ?? new DefaultHttpMessageHandler());

#if LEVELPLAY_DEPENDENCIES_INSTALLED
            var deserializer = new HttpContentJsonDeserializer();
            m_ResponseHandler = responseHandler ?? new ResponseHandler(deserializer);
#endif
        }

        public async Task<T> SendAsync<T>(Request request, CancellationToken cancellationToken = default)
        {
            using var responseMessage = await m_HttpClient.SendAsync(request.ToHttpRequestMessage(), cancellationToken).ConfigureAwait(ConfigureAwait);
            return await m_ResponseHandler.HandleAsync<T>(responseMessage, cancellationToken).ConfigureAwait(ConfigureAwait);
        }

        public void Dispose() => m_HttpClient.Dispose();
    }
}
