using System;
using System.Net;

namespace Unity.Services.LevelPlay.Editor
{
    internal sealed class HttpResponseException : Exception
    {
        internal HttpStatusCode StatusCode { get; }
        internal string Body { get; }

        internal HttpResponseException(HttpStatusCode statusCode, string reason, string body)
            : base($"HTTP {(int)statusCode}: {reason}. Body: {body})")
        {
            StatusCode = statusCode;
            Body = body;
        }
    }
}
