using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Plots
{

public class PlotEditorInInspector
{
    Plot m_plot;

    public PlotEditorInInspector(Plot plot)
    {
        m_plot = plot;
    }

    public void OnInspectorGUI()
    {
        Rect targetRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUI.ObjectField(targetRect, m_plot, typeof(Plot), false);
        }
        Rect buttonRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 12);
        if (GUI.Button(buttonRect, "Open Editor"))
        {
            PlotEditorWindow.InitWindow(m_plot);
        }
    }

}

}
