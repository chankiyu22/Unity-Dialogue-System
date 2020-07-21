using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class VariableAssignmentReorderableList
{
    ReorderableList reorderableList;
    SerializedObject serializedObject;
    SerializedProperty serializedProperty;

    public VariableAssignmentReorderableList(SerializedObject serializedObject, SerializedProperty elements)
    {
        this.reorderableList = new ReorderableList(serializedObject, elements);
        this.serializedObject = serializedObject;
        this.serializedProperty = elements;

        this.reorderableList.drawElementCallback = DrawElement;
        this.reorderableList.onAddCallback = Add;
        this.reorderableList.drawHeaderCallback = DrawHeader;
    }

    public void DoLayoutList()
    {
        reorderableList.DoLayoutList();
    }

    void DrawElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        SerializedProperty elementProp = serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField(rect, elementProp);
    }

    void Add(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        // Highlight new item
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_variable").objectReferenceValue = null;
        element.FindPropertyRelative("m_intValue").intValue = 0;
        element.FindPropertyRelative("m_floatValue").floatValue = 0;
        element.FindPropertyRelative("m_boolValue").boolValue = false;
        element.FindPropertyRelative("m_stringValue").stringValue = null;
    }

    void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Update Variables");
    }
}

}

