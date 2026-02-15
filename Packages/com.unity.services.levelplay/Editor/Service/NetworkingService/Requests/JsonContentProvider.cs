#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System.Net.Http;
using System.Text;

namespace Unity.Services.LevelPlay.Editor
{
    internal class JsonContentProvider<T> : IHttpContentProvider
    {
        readonly T m_Data;
        readonly HttpContentJsonSerializer m_Serializer = new();

        public JsonContentProvider(T data) => m_Data = data;

        public HttpContent CreateHttpContent()
        {
            var json = m_Serializer.SerializeToString(m_Data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }

    internal class JsonContentProvider : JsonContentProvider<object>
    {
        public JsonContentProvider(object data) : base(data) {}
    }
}
#endif
