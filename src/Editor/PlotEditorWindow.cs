using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem
{

public class PlotEditorWindow : EditorWindow
{
    private Plot m_plot = null;
    private Editor m_plotEditor = null;
    private Vector2 m_scrollPos;

    public static EditorWindow InitWindow(Plot plot)
    {
        PlotEditorWindow window = GetWindow<PlotEditorWindow>(true, "Plot Editor");
        window.m_plot = plot;
        window.m_plotEditor = Editor.CreateEditor(plot);
        return window;
    }

    void OnGUI()
    {
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField(m_plot, typeof(Plot), false);
        }

        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - EditorGUIUtility.singleLineHeight - 2));
        m_plotEditor.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.EndScrollView();
    }
}

}
