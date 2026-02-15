using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unity.Services.LevelPlay.Editor
{
    internal class ResponseHandler : IResponseHandler
    {
        readonly IHttpContentDeserializable m_Deserializer;

        internal ResponseHandler(IHttpContentDeserializable deserializer) => m_Deserializer = deserializer;

        public async Task<T> HandleAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var mediaType = response.Content.Headers.ContentType?.MediaType;
            if (!m_Deserializer.CanDeserialize(mediaType))
                throw new NotSupportedException($"Unsupported content type: { mediaType }");

            if (response.IsSuccessStatusCode)
                return await m_Deserializer.DeserializeAsync<T>(response.Content, cancellationToken)
                    .ConfigureAwait(false);

            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new HttpResponseException(response.StatusCode, response.ReasonPhrase ?? string.Empty, body);
        }
    }
}
