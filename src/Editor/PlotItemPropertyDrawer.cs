using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem
{

[CustomPropertyDrawer(typeof(PlotItem))]
class PlotItemPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int prevIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty characterProp = property.FindPropertyRelative("m_character");
        SerializedProperty dialogueTextProp = property.FindPropertyRelative("m_dialogueText");
        SerializedProperty avatarTextureSourceProp = property.FindPropertyRelative("m_avatarTextureSource");

        Rect avatarTexturePreviewRect = new Rect(position.x, position.y, 70, position.height - EditorGUIUtility.singleLineHeight * 2 - 8);
        Rect avatarTextureSourcePropRect = new Rect(position.x, position.yMax - EditorGUIUtility.singleLineHeight * 2 - 6, 70, EditorGUIUtility.singleLineHeight);
        Rect characterPropRect = new Rect(position.x, position.yMax - EditorGUIUtility.singleLineHeight - 4, 88, EditorGUIUtility.singleLineHeight);
        Rect dialogueTextPropRect = new Rect(position.x + 72, position.y, position.width - 72, EditorGUIUtility.singleLineHeight);
        Rect dialogueTextPreviewRect = new Rect(position.x + 72, position.y  + EditorGUIUtility.singleLineHeight + 2, position.width - 72, position.height - EditorGUIUtility.singleLineHeight * 2 - 8);

        EditorGUI.DrawRect(avatarTexturePreviewRect, Color.blue);
        EditorGUI.PropertyField(avatarTextureSourcePropRect, avatarTextureSourceProp, GUIContent.none);
        EditorGUI.PropertyField(characterPropRect, characterProp, GUIContent.none);
        EditorGUI.PropertyField(dialogueTextPropRect, dialogueTextProp, GUIContent.none);

        DialogueText dialogueText = (DialogueText) dialogueTextProp.objectReferenceValue;
        using (new EditorGUI.DisabledScope(true))
        {
            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            EditorGUI.TextArea(dialogueTextPreviewRect, dialogueText == null ? "" : dialogueText.text, style);
        }

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = prevIndent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 7 + 2;
    }
}

}
