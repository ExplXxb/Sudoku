using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

#nullable enable

namespace Unity.Services.LevelPlay.Editor
{
    internal class Request
    {
        public Uri Uri { get; }
        public HttpMethod Method { get; }
        public string? ApiKey { get; }
        public IReadOnlyDictionary<string, string>? Headers { get; }
        public IHttpContentProvider? ContentProvider { get; }

        public Request(
            Uri uri,
            HttpMethod method,
            string? apiKey = null,
            IReadOnlyDictionary<string, string>? headers = null,
            IHttpContentProvider? contentProvider = null)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            ApiKey = apiKey;
            Method = method;
            Headers = headers;
            ContentProvider = contentProvider;
        }
    }

    internal static class RequestExtensions
    {
        internal static HttpRequestMessage ToHttpRequestMessage(this Request request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var requestMessage = new HttpRequestMessage(request.Method, request.Uri);

            if (!string.IsNullOrEmpty(request.ApiKey))
                requestMessage.Headers.Add("x-api-key", request.ApiKey);

            if (request.Headers is { Count: > 0 })
                foreach (var header in request.Headers)
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (requestMessage.Headers.Accept == null || requestMessage.Headers.Accept.Count == 0)
                requestMessage.Headers.Accept?.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (request.ContentProvider != null)
                requestMessage.Content = request.ContentProvider.CreateHttpContent();

            return requestMessage;
        }
    }
}
