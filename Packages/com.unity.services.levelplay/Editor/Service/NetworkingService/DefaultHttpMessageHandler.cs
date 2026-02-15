using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Unity.Services.LevelPlay.Editor
{
    internal class DefaultHttpMessageHandler : HttpMessageHandler
    {
        readonly IRetryPolicy<HttpRequestMessage, HttpResponseMessage> m_RetryPolicy;

        internal DefaultHttpMessageHandler()
        {
            var retryPolicy = new RetryPolicy<HttpRequestMessage, HttpResponseMessage>();
            var retrySettings = retryPolicy.Settings;
            m_RetryPolicy = retryPolicy
                .HandleException<HttpRequestException>()
                .WithRetryPredicate(response => retrySettings.ShouldRetryForStatusCode(response.StatusCode))
                .CreateRetryOperation(SendWithUnityWebRequestAsync);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            if (requestMessage.RequestUri == null) throw new ArgumentException("RequestUri must be set.", nameof(requestMessage));

            var timeout = TimeSpan.FromSeconds(30);
            return await m_RetryPolicy.RunAsync(requestMessage, timeout, cancellationToken);
        }

        static async Task<HttpResponseMessage> SendWithUnityWebRequestAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            var webRequestData = await CreateUnityWebRequestAsync(requestMessage);

            using (webRequestData.webRequest)
            {
                await using var registration = RegisterCancellationToken(cancellationToken, webRequestData.webRequest);

                try
                {
                    await ExecuteWebRequestAsync(webRequestData.webRequest, cancellationToken);
                    return CreateHttpResponseMessage(requestMessage, webRequestData.webRequest, webRequestData.downloadHandler);
                }
                catch (OperationCanceledException)
                {
                    webRequestData.webRequest.Abort();
                    throw;
                }
            }
        }

        static async Task<(UnityWebRequest webRequest, DownloadHandlerBuffer downloadHandler)> CreateUnityWebRequestAsync(HttpRequestMessage requestMessage)
        {
            var hasContent = requestMessage.Content != null;
            var method = requestMessage.Method;

            var requestContent = Array.Empty<byte>();
            if (hasContent)
                requestContent = await requestMessage.Content.ReadAsByteArrayAsync();

            UploadHandler uploadHandler = null;
            var isGetOrHead = method == HttpMethod.Get || method == HttpMethod.Head;
            if (requestContent.Length > 0 || !isGetOrHead)
            {
                var raw = new UploadHandlerRaw(requestContent);
                if (requestMessage.Content?.Headers.ContentType != null)
                    raw.contentType = requestMessage.Content.Headers.ContentType.ToString();
                uploadHandler = raw;
            }

            var downloadHandler = new DownloadHandlerBuffer();
            var webRequest = new UnityWebRequest(
                requestMessage.RequestUri,
                requestMessage.Method.ToString(),
                downloadHandler,
                uploadHandler)
            {
                disposeDownloadHandlerOnDispose = true,
                disposeUploadHandlerOnDispose = true
            };

            CopyHeaders(requestMessage.Headers, webRequest);

            if (hasContent && uploadHandler != null)
                CopyHeaders(requestMessage.Content.Headers, webRequest);

            return (webRequest, downloadHandler);
        }

        static CancellationTokenRegistration RegisterCancellationToken(CancellationToken cancellationToken, UnityWebRequest webRequest)
        {
            if (!cancellationToken.CanBeCanceled) return default;

            return cancellationToken.Register(state =>
            {
                var request = (UnityWebRequest)state;
                try
                {
                    request?.Abort();
                }
                catch (Exception)
                {
                    // Ignore exceptions during cancellation cleanup
                }
            }, webRequest);
        }

        static async Task ExecuteWebRequestAsync(UnityWebRequest webRequest, CancellationToken cancellationToken)
        {
            await webRequest.SendWebRequest();

            cancellationToken.ThrowIfCancellationRequested();

            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                var errorMessage = !string.IsNullOrEmpty(webRequest.error)
                    ? webRequest.error
                    : "Unknown error";

                throw new HttpRequestException($"UnityWebRequest failed with result: {webRequest.result}, Error: {errorMessage}");
            }
        }

        static HttpResponseMessage CreateHttpResponseMessage(HttpRequestMessage requestMessage, UnityWebRequest webRequest, DownloadHandlerBuffer downloadHandler)
        {
            var response = new HttpResponseMessage((HttpStatusCode)webRequest.responseCode)
            {
                RequestMessage = requestMessage,
                Content = PooledByteArrayHttpContent.ToHttpContent(downloadHandler),
                Version = requestMessage.Version
            };

            var responseHeaders = webRequest.GetResponseHeaders();
            if (responseHeaders != null)
            {
                foreach (var header in responseHeaders)
                    if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Value))
                    {
                        if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) ||
                            header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase) ||
                            header.Key.Equals("Content-Encoding", StringComparison.OrdinalIgnoreCase))
                        {
                            response.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                            continue;
                        }

                        response.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
            }

            return response;
        }

        static void CopyHeaders(HttpHeaders headers, UnityWebRequest webRequest)
        {
            if (headers == null) return;

            foreach (var header in headers)
                if (header.Value != null)
                    foreach (var value in header.Value)
                        if (!string.IsNullOrEmpty(value))
                            webRequest.SetRequestHeader(header.Key, value);
        }
    }
}
