using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class DialogueEditorInInspector
{
    Dialogue m_dialogue;

    public DialogueEditorInInspector(Dialogue dialogue)
    {
        m_dialogue = dialogue;
    }

    public void OnInspectorGUI()
    {
        Rect targetRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUI.ObjectField(targetRect, m_dialogue, typeof(Dialogue), false);
        }
        Rect buttonRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 12);
        if (GUI.Button(buttonRect, "Open Editor"))
        {
            DialogueEditorWindow.InitWindow(m_dialogue);
        }
    }
}

}
