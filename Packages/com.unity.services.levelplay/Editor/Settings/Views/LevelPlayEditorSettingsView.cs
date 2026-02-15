#if UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
using System.Collections.Generic;
using Unity.Services.Core.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace Unity.Services.LevelPlay.Editor
{
    class LevelPlayEditorSettingsView : EditorGameServiceSettingsProvider
    {
        public LevelPlayEditorSettingsView(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(
            path, scopes, keywords)
        {
        }

        protected override VisualElement GenerateServiceDetailUI()
        {
            return new LevelPlayVisualElement();
        }

        protected override VisualElement GenerateUnsupportedDetailUI()
        {
            return GenerateServiceDetailUI();
        }

        protected override IEditorGameService EditorGameService =>
            EditorGameServiceRegistry.Instance.GetEditorGameService<LevelPlayIdentifier>();

        protected override string Title => "Ads Mediation (LevelPlay)";
        protected override string Description => "Monetize your game with Unity LevelPlay.";

        LevelPlayEditorSettingsView(SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(GenerateProjectSettingsPath("Ads Mediation (LevelPlay)"), scopes, keywords)
        {
#if UNITY_2022_2_OR_NEWER
            if (CloudProjectSettings.projectBound)
#else
            if (!string.IsNullOrEmpty(CloudProjectSettings.projectId))
#endif
            {
                EditorServices.Instance.EditorAnalyticsService.SendProjectBound();
            }
            else
            {
                EditorServices.Instance.EditorAnalyticsService.SendProjectNotBound();
            }
        }

        /// <summary>
        /// Method which adds your settings provider to ProjectSettings
        /// </summary>
        /// <returns>A <see cref="LevelPlayEditorSettingsView"/>.</returns>
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new LevelPlayEditorSettingsView(SettingsScope.Project);
        }

        public override void OnDeactivate()
        {
            EditorServices.Instance.EditorAnalyticsService.SendCloseSettings();
        }
    }

    class LevelPlayVisualElement : VisualElement
    {
        public LevelPlayVisualElement()
        {
            Clear();

            var appsManagementSettingsProvider = new AppsManagementSettingsView();
            Add(appsManagementSettingsProvider);

            var appsManagementSeparator = new VisualElement();
            appsManagementSeparator.AddToClassList("separator");
            Add(appsManagementSeparator);

            var packageManagementSettingsProvider = new PackageManagementSettingsView();
            Add(packageManagementSettingsProvider);

            var packageManagementSeparator = new VisualElement();
            packageManagementSeparator.AddToClassList("separator");
            Add(packageManagementSeparator);
        }
    }
}
#endif
