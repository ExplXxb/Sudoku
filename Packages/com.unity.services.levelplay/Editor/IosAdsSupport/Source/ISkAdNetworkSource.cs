using System.IO;

namespace Unity.Services.LevelPlay.Editor
{
    interface ISkAdNetworkSource
    {
        string Path { get; }
        Stream Open();
    }
}
