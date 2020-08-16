using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Characters
{

[CustomEditor(typeof(CharacterDataDispatcher))]
public class CharacterDataDispatcherEditor : Editor
{
    SerializedProperty characterDataKeyEventListProp;
    ReorderableList characterDataKeyEventReorderableList;

    void OnEnable()
    {
        characterDataKeyEventListProp = serializedObject.FindProperty("m_characterDataKeyEventList");
        characterDataKeyEventReorderableList = new ReorderableList(serializedObject, characterDataKeyEventListProp, true, true, true, true);
        characterDataKeyEventReorderableList.drawHeaderCallback = DrawHeader;
        characterDataKeyEventReorderableList.drawElementCallback = DrawElement;
        characterDataKeyEventReorderableList.elementHeightCallback = ElementHeight;
        characterDataKeyEventReorderableList.onAddCallback = AddElement;
        characterDataKeyEventReorderableList.onRemoveCallback = RemoveElement;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        characterDataKeyEventReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Data Key and Events");
    }

    void DrawElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        SerializedProperty characterDataKeyEventProp = characterDataKeyEventListProp.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField(rect, characterDataKeyEventProp);
    }

    float ElementHeight(int index)
    {
        SerializedProperty characterDataKeyEventProp = characterDataKeyEventListProp.GetArrayElementAtIndex(index);
        return EditorGUI.GetPropertyHeight(characterDataKeyEventProp) + 6;
    }

    void AddElement(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dataKey").objectReferenceValue = null;
        element.FindPropertyRelative("m_OnIntEvents.m_PersistentCalls.m_Calls").arraySize = 0;
        element.FindPropertyRelative("m_OnFloatEvents.m_PersistentCalls.m_Calls").arraySize = 0;
        element.FindPropertyRelative("m_OnBoolEvents.m_PersistentCalls.m_Calls").arraySize = 0;
        element.FindPropertyRelative("m_OnStringEvents.m_PersistentCalls.m_Calls").arraySize = 0;

    }

    void RemoveElement(ReorderableList l)
    {
        if (EditorUtility.DisplayDialog("Delete Data Key Event", "Are you sure you want to delete?", "Yes", "No"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        }
    }
}

}
