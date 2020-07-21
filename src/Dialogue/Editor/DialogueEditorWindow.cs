using UnityEngine;
using UnityEditor;


namespace Chankiyu22.DialogueSystem.Dialogues
{

public class DialogueEditorWindow : EditorWindow
{
    private Dialogue m_dialogue = null;
    private Editor m_dialogueEditor = null;
    private Vector2 m_scrollPos;

    public static void InitWindow(Dialogue dialogue)
    {
        DialogueEditorWindow window = GetWindow<DialogueEditorWindow>(true, "Dialogue Editor");
        window.m_dialogue = dialogue;
        window.m_dialogueEditor = Editor.CreateEditor(dialogue);
        window.Show();
    }

    void OnGUI()
    {
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField(m_dialogue, typeof(Dialogue), false);
        }

        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - EditorGUIUtility.singleLineHeight - 2));
        m_dialogueEditor.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.EndScrollView();
    }
}

}