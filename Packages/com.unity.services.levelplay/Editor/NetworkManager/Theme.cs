using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor
{
    struct Colors
    {
        internal Color BackgroundColor;
        internal Color BorderColor;
        internal Color RowColor1;
        internal Color RowColor2;
        internal Color ProjectSettingsUpdateLabelColor;
    }

    static class Theme
    {
        private static readonly Colors Dark = new Colors
        {
            BackgroundColor = new Color(56 / 255f, 56 / 255f, 56 / 255f, 1),
            BorderColor = new Color(35 / 255f, 35 / 255f, 35 / 255f, 1),
            RowColor1 = new Color(63 / 255f, 63 / 255f, 63 / 255f, 1),
            RowColor2 = new Color(56 / 255f, 56 / 255f, 56 / 255f, 1),
            ProjectSettingsUpdateLabelColor = ParseHexColor("#FFC107", Color.yellow)
        };

        private static readonly Colors Light = new Colors
        {
            BackgroundColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1),
            BorderColor = new Color(35 / 255f, 35 / 255f, 35 / 255f, 1),
            RowColor1 = new Color(240 / 255f, 240 / 255f, 240 / 255f, 1),
            RowColor2 = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1),
            ProjectSettingsUpdateLabelColor = ParseHexColor("#FFC107", Color.yellow)
        };

        internal static Colors GetColors()
        {
            return EditorGUIUtility.isProSkin ? Dark : Light;
        }

        internal static Color ParseHexColor(string hex, Color fallback) => ColorUtility.TryParseHtmlString(hex, out var color) ? color : fallback;
    }
}
