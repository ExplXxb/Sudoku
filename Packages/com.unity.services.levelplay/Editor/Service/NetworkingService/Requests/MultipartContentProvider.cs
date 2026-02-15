using System.Net.Http;

namespace Unity.Services.LevelPlay.Editor
{
    internal class MultipartContentProvider : IHttpContentProvider
    {
        MultipartFormDataContent Content { get; } = new();

        public HttpContent CreateHttpContent() => Content;
    }
}