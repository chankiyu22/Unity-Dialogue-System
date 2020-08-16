using UnityEngine;
using UnityEditor;

using Chankiyu22.DialogueSystem.Avatars;
using Chankiyu22.DialogueSystem.Dialogues;

namespace Chankiyu22.DialogueSystem.Plots
{

[CustomPropertyDrawer(typeof(PlotItem))]
class PlotItemPropertyDrawer : PropertyDrawer
{
    PlotAdditionalDataReorderableListManager plotAdditionalDataReorderableListManager = new PlotAdditionalDataReorderableListManager();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int prevIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty characterProp = property.FindPropertyRelative("m_character");
        SerializedProperty dialogueTextProp = property.FindPropertyRelative("m_dialogueText");
        SerializedProperty avatarTextureSourceProp = property.FindPropertyRelative("m_avatarTextureSource");

        Rect plotDialogueRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight * 7);

        Rect avatarTexturePreviewRect = new Rect(plotDialogueRect.x, plotDialogueRect.y, 70, plotDialogueRect.height - EditorGUIUtility.singleLineHeight * 2 - 8);
        Rect avatarTextureSourcePropRect = new Rect(plotDialogueRect.x, plotDialogueRect.yMax - EditorGUIUtility.singleLineHeight * 2 - 6, 70, EditorGUIUtility.singleLineHeight);
        Rect characterPropRect = new Rect(plotDialogueRect.x, plotDialogueRect.yMax - EditorGUIUtility.singleLineHeight - 4, 88, EditorGUIUtility.singleLineHeight);
        Rect dialogueTextPropRect = new Rect(plotDialogueRect.x + 72, plotDialogueRect.y, plotDialogueRect.width - 72, EditorGUIUtility.singleLineHeight);
        Rect dialogueTextPreviewRect = new Rect(plotDialogueRect.x + 72, plotDialogueRect.y  + EditorGUIUtility.singleLineHeight + 2, plotDialogueRect.width - 72, plotDialogueRect.height - EditorGUIUtility.singleLineHeight * 2 - 8);

        EditorGUI.DrawRect(avatarTexturePreviewRect, Color.grey);
        AvatarTextureSource avatarTextureSource = (AvatarTextureSource) avatarTextureSourceProp.objectReferenceValue;
        if (avatarTextureSource == null || avatarTextureSource.GetPreviewTexture() == null)
        {
        }
        else
        {
            EditorGUI.DrawPreviewTexture(avatarTexturePreviewRect, avatarTextureSource.GetPreviewTexture(), null, ScaleMode.ScaleToFit);
        }
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

        position.y += EditorGUIUtility.singleLineHeight * 7;

        SerializedProperty additionalDataListProp = property.FindPropertyRelative("m_additionalDataList");
        PlotAdditionalDataReorderableList plotAdditionalDataReorderableList = plotAdditionalDataReorderableListManager.GetReorderableList(additionalDataListProp);

        Rect additionalDataListRect = new Rect(position.x, position.y, position.width, plotAdditionalDataReorderableList.GetHeight());
        plotAdditionalDataReorderableList.DoList(additionalDataListRect);

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = prevIndent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty additionalDataListProp = property.FindPropertyRelative("m_additionalDataList");
        PlotAdditionalDataReorderableList plotAdditionalDataReorderableList = plotAdditionalDataReorderableListManager.GetReorderableList(additionalDataListProp);
        return EditorGUIUtility.singleLineHeight * 7 + 2 + plotAdditionalDataReorderableList.GetHeight();
    }
}

}
