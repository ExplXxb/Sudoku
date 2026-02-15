using System.Net.Http;
using System.Net.Http.Headers;

#nullable enable

namespace Unity.Services.LevelPlay.Editor
{
    internal class ByteArrayContentProvider : IHttpContentProvider
    {
        readonly byte[] m_Bytes;
        readonly string? m_MimeType;

        public ByteArrayContentProvider(byte[] bytes, string? mimeType = null)
        {
            m_Bytes = bytes;
            m_MimeType = mimeType;
        }

        public HttpContent CreateHttpContent()
        {
            var content = PooledByteArrayHttpContent.Create(m_Bytes);
            if (m_MimeType != null)
                content.Headers.ContentType = new MediaTypeHeaderValue(m_MimeType);
            return content;
        }
    }
}
