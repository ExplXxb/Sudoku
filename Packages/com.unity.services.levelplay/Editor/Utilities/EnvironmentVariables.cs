#if UNITY_EDITOR
using System.IO;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor
{
    static class EnvironmentVariables
    {
        const string k_LevelPlayPackageName = Constants.k_PackageName;
        const string k_AndroidLibPath = "Runtime/Plugins/Android/AndroidManifest.xml";
        static readonly string k_DotUnityPackageManifestPath = Path.Combine(Constants.k_UnityPackageDirectoryName, k_AndroidLibPath);
        static readonly string k_UpmManifestPath = Path.Combine(FilePaths.UpmPackageDirectoryPath, k_AndroidLibPath);

        internal static string androidManifestPath { get; private set; }

        internal static void BuildManifestPath()
        {
            LevelPlayPackmanQuerier.instance.CheckIfPackageIsInstalledWithUpm(k_LevelPlayPackageName, isInstalled =>
            {
                androidManifestPath = isInstalled ? Path.GetFullPath(k_UpmManifestPath) : Path.Combine(Application.dataPath, k_DotUnityPackageManifestPath);
            });
        }
    }
}
#endif
