using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Characters
{

[CustomEditor(typeof(CharacterDataDispatcher))]
public class CharacterDataDispatcherEditor : Editor
{
    SerializedProperty characterDataKeyEventListProp;
    SerializedProperty defaultCharacterDataKeyEventListProp;

    CharacterDataKeyEventReorderableList characterDataKeyEventReorderableList;
    DefaultCharacterDataKeyEventReorderableList defaultCharacterDataKeyEventReorderableList;

    void OnEnable()
    {
        characterDataKeyEventListProp = serializedObject.FindProperty("m_characterDataKeyEventList");
        defaultCharacterDataKeyEventListProp = serializedObject.FindProperty("m_defaultCharacterDataKeyEventList");
        characterDataKeyEventReorderableList = new CharacterDataKeyEventReorderableList(serializedObject, characterDataKeyEventListProp);
        defaultCharacterDataKeyEventReorderableList = new DefaultCharacterDataKeyEventReorderableList(serializedObject, defaultCharacterDataKeyEventListProp);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        characterDataKeyEventReorderableList.DoLayoutList();

        EditorGUILayout.Space();
        defaultCharacterDataKeyEventReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}

}
