using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Characters
{

[CustomEditor(typeof(CharacterDataKey), true)]
public class CharacterDataKeyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CharacterDataKey dataKey = (CharacterDataKey) target;

        Rect defRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
        Rect typeRect = new Rect(defRect.x, defRect.y, 75, defRect.height);
        EditorGUI.LabelField(typeRect, dataKey.GetKeyTypeName(), EditorStyles.boldLabel);
        Rect nameRect = new Rect(defRect.x + 75, defRect.y, defRect.width - 75, defRect.height);
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUI.ObjectField(nameRect, dataKey, typeof(CharacterDataKey), false);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Description");

        SerializedProperty descriptionProp = serializedObject.FindProperty("m_description");
        Rect descriptionRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 3);
        GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
        textAreaStyle.wordWrap = true;
        descriptionProp.stringValue = EditorGUI.TextArea(descriptionRect, descriptionProp.stringValue, textAreaStyle);

        serializedObject.ApplyModifiedProperties();
    }

}

}
