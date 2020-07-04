using UnityEditor;

namespace Chankiyu22.DialogueSystem
{

[CustomEditor(typeof(DialogueController))]
public class DialogueControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        SerializedProperty dialogueProp = serializedObject.FindProperty("m_dialogue");
        EditorGUILayout.PropertyField(dialogueProp);

        EditorGUILayout.Space();

        SerializedProperty onDialogueBeginProp = serializedObject.FindProperty("m_OnDialogueBegin");
        EditorGUILayout.PropertyField(onDialogueBeginProp);
        SerializedProperty OnDialogueEndProp = serializedObject.FindProperty("m_OnDialogueEnd");
        EditorGUILayout.PropertyField(OnDialogueEndProp);

        EditorGUILayout.Space();

        SerializedProperty OnDialogueTextBeginProp = serializedObject.FindProperty("m_OnDialogueTextBegin");
        EditorGUILayout.PropertyField(OnDialogueTextBeginProp);
        SerializedProperty OnDialogueTextEndProp = serializedObject.FindProperty("m_OnDialogueTextEnd");
        EditorGUILayout.PropertyField(OnDialogueTextEndProp);

        EditorGUILayout.Space();

        SerializedProperty OnDialogueOptionsBeginProp = serializedObject.FindProperty("m_OnDialogueOptionsBegin");
        EditorGUILayout.PropertyField(OnDialogueOptionsBeginProp);
        SerializedProperty OnDialogueOptionsEndProp = serializedObject.FindProperty("m_OnDialogueOptionsEnd");
        EditorGUILayout.PropertyField(OnDialogueOptionsEndProp);

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

}
