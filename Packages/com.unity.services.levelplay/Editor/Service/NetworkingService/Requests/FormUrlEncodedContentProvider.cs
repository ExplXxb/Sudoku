using System.Collections.Generic;
using System.Net.Http;

namespace Unity.Services.LevelPlay.Editor
{
    internal class FormUrlEncodedContentProvider : IHttpContentProvider
    {
        readonly IEnumerable<KeyValuePair<string, string>> m_Data;
        
        public FormUrlEncodedContentProvider(IEnumerable<KeyValuePair<string, string>> data) => m_Data = data;
        
        public HttpContent CreateHttpContent() => new FormUrlEncodedContent(m_Data);
    }
}