namespace Unity.Services.LevelPlay.Editor
{
    interface IEditorAnalyticsSender
    {
        void Send(string component, string action);
    }
}
