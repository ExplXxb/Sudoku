using UnityEditor;

namespace Unity.Services.LevelPlay.Editor
{
    interface IAdapterSettings
    {
        void updateProject(BuildTarget buildTarget, string projectPath);
        void updateProjectPlist(BuildTarget buildTarget, string plistPath);
    }
}
