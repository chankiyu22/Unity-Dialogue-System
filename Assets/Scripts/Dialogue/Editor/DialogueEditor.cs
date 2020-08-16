using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    public Rect viewportRect = Rect.zero;
    public bool isEditorWindow = false;

    DialogueEditorInEditorWindow dialogueEditorInEditorWindow = null;
    DialogueEditorInInspector dialogueEditorInInspector = null;

    void OnEnable()
    {
        dialogueEditorInEditorWindow = new DialogueEditorInEditorWindow(serializedObject);
        dialogueEditorInInspector = new DialogueEditorInInspector((Dialogue) target);
    }

    public override void OnInspectorGUI()
    {
        if (isEditorWindow)
        {
            dialogueEditorInEditorWindow.OnInspectorGUI(viewportRect);
        }
        else
        {
            dialogueEditorInInspector.OnInspectorGUI();
        }
    }

}

}
