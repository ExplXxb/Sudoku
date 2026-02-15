#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class LineItemComponent : IDrawable
    {
        internal struct ViewModel
        {
            internal string NetworkName;
            internal string CurrentVersion;
            internal string LatestVersion;
            internal bool IsCurrentVersionCompatible;
            internal string TooltipText;
            internal bool IsRecommended;
            internal string RecommendedIconPath;
            internal bool IsNew;
            internal string NewIconPath;
            internal string DeleteIconPath;
            internal Func<Task> OnNetworkButtonClick;
            internal Action OnRemoveNetworkButtonClick;
            internal IntegrationManagerVersion.ViewModel CurrentVersionViewModel;
            internal IntegrationManagerVersion.ViewModel LatestVersionViewModel;
            internal bool CanBeRemoved;
        }

        readonly IntegrationManagerVersion m_CurrentVersion;
        readonly IntegrationManagerVersion m_LatestVersion;
        readonly IntegrationManagerButton m_Button;
        readonly IntegrationManagerRemoveButton m_RemoveButton;
        readonly IntegrationManagerRecommendedBox m_Recommended;
        readonly IntegrationManagerRecommendedBox m_New;
        readonly LineItemComponent.ViewModel m_ViewModel;
        const int k_NetworkNameFontSize = 12;
        private bool m_ButtonClickInProgress = false;
        const int k_LineItemHeight = 24;

        private AdapterState m_State = AdapterState.NotInstalled;

        internal LineItemComponent(ViewModel viewModel)
        {
            this.m_ViewModel = viewModel;
            m_CurrentVersion = new IntegrationManagerVersion(this.m_ViewModel.CurrentVersionViewModel, k_LineItemHeight);
            m_LatestVersion = new IntegrationManagerVersion(this.m_ViewModel.LatestVersionViewModel, k_LineItemHeight);

            SetAdapterState();

            m_Button = new IntegrationManagerButton(
                this.GetButtonText(),
                this.m_ViewModel.TooltipText,
                async () =>
                {
                    m_ButtonClickInProgress = true;
                    await this.m_ViewModel.OnNetworkButtonClick();
                    m_ButtonClickInProgress = false;
                });
            m_RemoveButton = new IntegrationManagerRemoveButton(GetDeleteTexture(), "Uninstall",
                m_ViewModel.OnRemoveNetworkButtonClick);

            m_Recommended = new IntegrationManagerRecommendedBox(GetRecommendedTexture(), 80, 16, k_LineItemHeight);
            m_New = new IntegrationManagerRecommendedBox(GetNewTexture(), 30, 16, k_LineItemHeight);
        }

        public void Draw()
        {
            this.Draw(0);
        }

        internal void Draw(int index)
        {
            var backgroundColor = index % 2 == 0 ? Theme.GetColors().RowColor1 : Theme.GetColors().RowColor2;
            GUIStyle horizontalStyle = new GUIStyle()
            {
                normal = { background = IntegrationManagerUIUtils.CreateTexture(backgroundColor) },
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(4, 4, 0, 0),
                fixedHeight = k_LineItemHeight
            };
            GUIStyle horizontalLabelStyle = new GUIStyle()
            {
                margin = new RectOffset(0, 0, 0, 0), padding = new RectOffset(0, 0, 0, 0), fixedWidth = 313
            };
            GUIStyle horizontalCurrentStyle = new GUIStyle()
            {
                margin = new RectOffset(0, 0, 0, 0), padding = new RectOffset(0, 0, 0, 0), fixedWidth = 125
            };
            GUIStyle horizontalLatestStyle = new GUIStyle()
            {
                margin = new RectOffset(0, 0, 0, 0), padding = new RectOffset(0, 0, 0, 0), fixedWidth = 200
            };
            var normalLabelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Normal,
                fontSize = k_NetworkNameFontSize,
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
            };

            GUILayout.BeginHorizontal(horizontalStyle);

            GUILayout.BeginHorizontal(horizontalLabelStyle);
            GUILayout.BeginVertical(new GUIStyle() { fixedHeight = k_LineItemHeight });
            GUILayout.FlexibleSpace();
            GUI.enabled = this.IsButtonEnabled();
            GUILayout.Label(this.m_ViewModel.NetworkName, normalLabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            if (this.m_ViewModel.IsRecommended)
            {
                m_Recommended.Draw();
            }
            if (this.m_ViewModel.IsNew)
            {
                m_New.Draw();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(horizontalCurrentStyle);
            m_CurrentVersion.Draw();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(horizontalLatestStyle);
            m_LatestVersion.Draw();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(new GUIStyle() { fixedHeight = k_LineItemHeight });
            GUILayout.FlexibleSpace();
            m_Button.Draw();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            if (m_ViewModel.CanBeRemoved)
            {
                m_RemoveButton.ActivateButton(ShowDeleteButton());
                m_RemoveButton.Draw();
            }

            GUILayout.EndHorizontal();
        }

        internal string GetAdapterName()
        {
            return m_ViewModel.NetworkName;
        }

        internal AdapterState GetAdapterState()
        {
            return m_State;
        }

        private void SetAdapterState()
        {

            if (string.IsNullOrEmpty(m_ViewModel.CurrentVersion))
            {
                m_State = AdapterState.NotInstalled;
            }
            else if (m_ViewModel.CurrentVersion != m_ViewModel.LatestVersion)
            {
                m_State = !m_ViewModel.IsCurrentVersionCompatible
                    ? AdapterState.UpdateIsNecessary
                    : AdapterState.UpdateAvailable;
            }
            else
            {
                m_State = AdapterState.Updated;
            }
        }

        private string GetButtonText()
        {
            if (string.IsNullOrEmpty(m_ViewModel.CurrentVersion))
            {
                return "Install";
            }

            return m_ViewModel.CurrentVersion != m_ViewModel.LatestVersion ? "Update" : "Updated";
        }

        private bool ShowDeleteButton()
        {
            return !string.IsNullOrEmpty(m_ViewModel.CurrentVersion);
        }

        private bool IsButtonEnabled()
        {
            if (m_ButtonClickInProgress)
            {
                return false;
            }

            return m_ViewModel.CurrentVersion != m_ViewModel.LatestVersion;
        }

        private Texture2D GetRecommendedTexture()
        {
            var rawData = System.IO.File.ReadAllBytes(this.m_ViewModel.RecommendedIconPath);
            var recommendedTexture = new Texture2D(2, 2);
            recommendedTexture.LoadImage(rawData);
            return recommendedTexture;
        }

        private Texture2D GetNewTexture()
        {
            var rawData = System.IO.File.ReadAllBytes(this.m_ViewModel.NewIconPath);
            var newTexture = new Texture2D(2, 2);
            newTexture.LoadImage(rawData);
            return newTexture;
        }

        private Texture2D GetDeleteTexture()
        {
            var rawData = System.IO.File.ReadAllBytes(this.m_ViewModel.DeleteIconPath);
            var deleteTexture = new Texture2D(2, 2);
            deleteTexture.LoadImage(rawData);
            return deleteTexture;
        }

        internal enum AdapterState
        {
            UpdateIsNecessary,
            UpdateAvailable,
            Updated,
            NotInstalled
        }
    }
}
#endif
