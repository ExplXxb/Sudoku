#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor
{
    static class ServicesCoreDependencyInstaller
    {
        const string k_CorePackageName = "com.unity.services.core";

        internal static void InstallServicesCoreIfNotFound()
        {
            if (Application.isBatchMode)
            {
                return;
            }
#if !LEVELPLAY_DEPENDENCIES_INSTALLED
            {
                LevelPlayPackmanQuerier.instance.CheckIfPackageIsInstalledWithUpm(k_CorePackageName, isInstalled =>
                {
                    if (!isInstalled)
                    {
                        Client.Add(k_CorePackageName);
                    }

                    AddScriptingSymbol();
                });
            }
#endif
        }

        static void AddScriptingSymbol()
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
            var defines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);

            if (!defines.Contains("LEVELPLAY_DEPENDENCIES_INSTALLED"))
            {
                defines = string.IsNullOrEmpty(defines)
                    ? "LEVELPLAY_DEPENDENCIES_INSTALLED"
                    : defines + ";LEVELPLAY_DEPENDENCIES_INSTALLED";

                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, defines);
            }
        }
    }
}
#endif
