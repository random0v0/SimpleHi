using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class SimpleHi
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
            EditorGUI.DrawRect(_rect, Color.yellow); //Change Color

            string displayName = select.name;

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