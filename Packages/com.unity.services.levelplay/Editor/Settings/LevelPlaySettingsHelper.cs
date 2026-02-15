#if UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.LevelPlay.Editor
{
    static class LevelPlaySettingsHelper
    {
        internal static Label BuildLabel(string text)
        {
            var label = new Label(text)
            {
                style =
                {
                    marginBottom = 8,
                    marginTop = 8,
                    marginLeft = 8,
                    marginRight = 8,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    flexGrow = 1,
                    flexShrink = 0,
                    flexBasis = 0
                }
            };

            return label;
        }

        internal static Label BuildNoFlexLabel(string text)
        {
            var label = new Label(text)
            {
                style =
                {
                    marginBottom = 8,
                    marginTop = 8,
                    marginLeft = 8,
                    marginRight = 8,
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };

            return label;
        }

        internal static Label BuildTitleLabel(string text)
        {
            var label = new Label(text) {
                style =
                {
                    fontSize = 18,
                    marginBottom = 8,
                    marginTop = 8
                }
            };

            return label;
        }

        internal static VisualElement VisualElementsRow(params VisualElement[] elements)
        {
            var row = new VisualElement {style = {flexDirection = FlexDirection.Row}};
            foreach (var element in elements)
            {
                row.Add(element);
            }

            return row;
        }

        internal static VisualElement DivideVisualElementInARow(VisualElement leftVisualElement,
            VisualElement rightVisualElement)
        {
            var row = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems = Align.Center
                }
            };

            row.Add(leftVisualElement);
            row.Add(rightVisualElement);

            return row;
        }
    }
}
#endif
