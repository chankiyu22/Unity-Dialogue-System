using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Chankiyu22.DialogueSystem.Dialogues;

namespace Chankiyu22.DialogueSystem.Plots
{

[CustomEditor(typeof(Plot))]
public class PlotEditor : Editor
{

    public bool isEditorWindow = false;
    public Rect viewportRect = Rect.zero;

    PlotEditorInEditorWindow m_plotEditorInEditorWindow = null;
    PlotEditorInInspector m_plotEditorInInspector = null;

    void OnEnable()
    {
        m_plotEditorInEditorWindow = new PlotEditorInEditorWindow(serializedObject, target);
        m_plotEditorInInspector = new PlotEditorInInspector((Plot) target);

    }

    public override void OnInspectorGUI()
    {
        if (isEditorWindow)
        {
            m_plotEditorInEditorWindow.OnInspectorGUI(viewportRect);
        }
        else
        {
            m_plotEditorInInspector.OnInspectorGUI();
        }

    }
}

}
