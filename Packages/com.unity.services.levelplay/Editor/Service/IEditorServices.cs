namespace Unity.Services.LevelPlay.Editor
{
    interface IEditorServices
    {
        IFileService FileService { get; }
        IDirectoryService DirectoryService { get; }
        IXmlDocumentFactory XmlDocumentFactory { get; }
        IWebRequestService WebRequestService { get; }
        ILevelPlayNetworkManager LevelPlayNetworkManager { get; }
        IEditorAnalyticsSender EditorAnalyticsSender { get; }
        IEditorAnalyticsService EditorAnalyticsService { get; }
        ILevelPlayLogger LevelPlayLogger { get; }
        IOnLoadService OnLoadService { get; }
        ILevelPlaySdkInstaller LevelPlaySdkInstaller { get; }
        IPackageTypeService PackageTypeService { get; }
    }
}
