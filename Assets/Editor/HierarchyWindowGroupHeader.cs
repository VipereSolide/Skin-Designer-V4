using UnityEngine;
using UnityEditor;

/// <summary>
/// Hierarchy Window Group Header
/// http://diegogiacomelli.com.br/unitytips-hierarchy-window-group-header
/// </summary>
[InitializeOnLoad]
public static class HierarchyWindowGroupHeader
{
    static GUIStyle style = new GUIStyle();

    static Color32 headerH1Background = new Color32(0, 0, 0, 192);
    static Color32 headerH2Background = new Color32(0, 0, 0, 146);

    static Color32 textColor = new Color32(240, 240, 240, 255);

    static string headerH1Prefix = "[h1]";
    static string headerH2Prefix = "[h2]";

    static HierarchyWindowGroupHeader()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null && gameObject.name.ToLowerInvariant().StartsWith(headerH1Prefix, System.StringComparison.Ordinal))
        {
            style.fontSize = Mathf.CeilToInt(EditorGUIUtility.singleLineHeight / 1.5f);
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = textColor;
            style.alignment = TextAnchor.MiddleCenter;
            style.richText = true;

            EditorGUI.DrawRect(selectionRect, new Color32(56,56,56,255));
            EditorGUI.DrawRect(selectionRect, headerH1Background);
            EditorGUI.LabelField(selectionRect, gameObject.name.Split(']')[1], style);
        }

        if (gameObject != null && gameObject.name.ToLowerInvariant().StartsWith(headerH2Prefix, System.StringComparison.Ordinal))
        {
            style.fontSize = Mathf.CeilToInt(EditorGUIUtility.singleLineHeight / 1.75f);
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = textColor;
            style.alignment = TextAnchor.MiddleCenter;
            style.richText = true;

            EditorGUI.DrawRect(selectionRect, new Color32(56, 56, 56, 255));
            EditorGUI.DrawRect(selectionRect, headerH2Background);
            EditorGUI.LabelField(selectionRect, gameObject.name.Split(']')[1], style);
        }

        if (gameObject != null && gameObject.name.ToLowerInvariant().StartsWith("[space]", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(56, 56, 56, 255));
        }
    }
}