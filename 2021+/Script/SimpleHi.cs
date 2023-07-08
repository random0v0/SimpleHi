#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleHi
{
    [InitializeOnLoad]
    class SimpleHi
    {
        static SimpleHi()
        {
            EditorApplication.hierarchyWindowItemOnGUI += Settings;
        }

        private static void Settings(int _objects, Rect _rect)
        {
            GameObject select = (GameObject)EditorUtility.InstanceIDToObject(_objects);

            if (select == null) return;

            if (select.name.StartsWith("_"))
            {
                EditorGUI.DrawRect(_rect, SimpleHiUtility.GetSavedColor()); //Change Color

                string displayName = select.name[1..];

                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.black;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;

                EditorGUI.LabelField(_rect, displayName, style);

                if (Event.current.type == EventType.MouseDown && _rect.Contains(Event.current.mousePosition))
                {
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    }

    static class SimpleHiProjectSettings
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/SimpleHiProjectSettings", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "SimpleHi",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    Color color = SimpleHiUtility.GetSavedColor();

                    EditorGUI.BeginChangeCheck();

                    Color changedColor = EditorGUILayout.ColorField("Color Field", color);

                    if (EditorGUI.EndChangeCheck())
                    {
                        SimpleHiUtility.SetSavedColor(changedColor);
                        EditorApplication.RepaintHierarchyWindow();
                    }
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "SimpleHi", "hierarchy" })
            };

            return provider;
        }
    }

    static class SimpleHiUtility
    {
        public static readonly Color DEFAULT_COLOR = Color.yellow;
        public static readonly string DEFAULT_COLOR_HEX = "FFFF00";

        public static Color GetSavedColor()
        {
            string savedColorAsHex = EditorPrefs.GetString("MyColor", DEFAULT_COLOR_HEX);
            return Hex2Color(savedColorAsHex);
        }

        public static void SetSavedColor(Color color)
        {
            EditorPrefs.SetString("MyColor", ColorUtility.ToHtmlStringRGB(color));
        }

        private static Color Hex2Color(string hex)
        {
            return ColorUtility.TryParseHtmlString('#' + hex, out Color color) ? color : DEFAULT_COLOR;
        }
    }
}
#endif