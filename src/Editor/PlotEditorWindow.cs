using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem
{

public class PlotEditorWindow : EditorWindow
{
    private Plot m_plot = null;
    private PlotEditor m_plotEditor = null;
    private Vector2 m_scrollPos;

    public static EditorWindow InitWindow(Plot plot)
    {
        PlotEditorWindow window = GetWindow<PlotEditorWindow>(true, "Plot Editor");
        window.m_plot = plot;
        window.m_plotEditor = (PlotEditor) Editor.CreateEditor(plot);
        window.m_plotEditor.isEditorWindow = true;
        return window;
    }

    void OnGUI()
    {
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField(m_plot, typeof(Plot), false);
        }

        float scrollTopOffset = EditorGUIUtility.singleLineHeight + 2;
        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - scrollTopOffset));
        m_plotEditor.viewportRect = new Rect(m_scrollPos.x, m_scrollPos.y, position.width, position.height - scrollTopOffset);
        m_plotEditor.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.EndScrollView();
    }
}

}
