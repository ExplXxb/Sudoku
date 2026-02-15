using UnityEngine.Networking;

namespace Unity.Services.LevelPlay.Editor
{
    interface IWebRequest
    {
        bool IsDone();
        void SendWebRequest();
        UnityWebRequest.Result Result();
        string Error();
        string DownloadHandlerText();
        byte[] DownloadHandlerData();
    }

    interface IWebRequestService
    {
        IWebRequest Get(string uri);
    }
}
