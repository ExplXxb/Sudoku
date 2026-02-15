using System.Net.Http;

namespace Unity.Services.LevelPlay.Editor
{
    internal interface IHttpContentProvider
    {
        HttpContent CreateHttpContent();
    }
}