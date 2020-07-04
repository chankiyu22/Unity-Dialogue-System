using UnityEngine;
using UnityEditor;


namespace Chankiyu22.DialogueSystem
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
        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Editing Dialogue", m_dialogue, typeof(Dialogue), false);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        m_dialogueEditor.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.EndScrollView();
    }
}

}
