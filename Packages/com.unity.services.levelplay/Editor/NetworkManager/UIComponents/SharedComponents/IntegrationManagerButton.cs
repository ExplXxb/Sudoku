using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerButton : IDrawable
    {
        readonly Func<Task> m_OnClick;
        readonly GUIContent m_GuiContent;
        readonly RectOffset m_Offset;

        internal IntegrationManagerButton(
            string buttonText,
            string tooltip,
            Func<Task> onClick,
            RectOffset offset = null)
        {
            m_OnClick = onClick;
            m_GuiContent = new GUIContent
            {
                text = buttonText,
                tooltip = tooltip
            };
            m_Offset = offset;
        }

        public void Draw()
        {
            GUILayout.BeginVertical();

            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                margin = m_Offset ?? new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 20
            };


            GUILayout.FlexibleSpace();
            var btn = GUILayout.Button(m_GuiContent, buttonStyle,
                GUILayout.ExpandWidth(true), GUILayout.MinWidth(72), GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            if (btn)
            {
                m_OnClick.Invoke();
            }
        }
    }
}
