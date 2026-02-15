using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerRemoveButton : IDrawable
    {
        readonly Action m_OnClick;
        readonly GUIContent m_GuiContent;
        readonly Texture2D m_Image;
        bool m_IsActivated;

        internal IntegrationManagerRemoveButton([CanBeNull] Texture2D deleteIcon,
                                                string tooltip, Action onClick)
        {
            m_OnClick = onClick;
            m_GuiContent = new GUIContent { tooltip = tooltip, image = deleteIcon };
        }

        public void ActivateButton(bool isActivated)
        {
            m_IsActivated = isActivated;
        }

        public void Draw()
        {
            GUILayout.BeginVertical(new GUIStyle { fixedHeight = 24, fixedWidth = 24 });
            var buttonStyle = GUI.skin.label;
            buttonStyle.margin = new RectOffset(5, 0, 0, 2);
            GUILayout.FlexibleSpace();

            if (m_IsActivated)
            {
                var btn = GUILayout.Button(m_GuiContent, buttonStyle, GUILayout.ExpandWidth(true),
                    GUILayout.MinWidth(16), GUILayout.Height(16));

                if (btn)
                {
                    m_OnClick.Invoke();
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}
