using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class DialogueVariableReorderableList : ReorderableList
{

    public DialogueVariableReorderableList(SerializedObject serializedObject, SerializedProperty elements)
        : base(serializedObject, elements)
    {
        this.serializedProperty = elements;

        this.drawElementCallback = DrawElement;
        this.onAddCallback = Add;
        this.drawHeaderCallback = DrawHeader;
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
        Rect variableRect = new Rect(rect.x + 14, rect.y, rect.width * 0.5f + 20 - 14, rect.height);
        Rect valueRect = new Rect(rect.x + rect.width * 0.5f + 20 + 5, rect.y, rect.width - (rect.width * 0.5f + 20 + 5), rect.height);

        EditorGUI.LabelField(variableRect, "Variable");
        EditorGUI.LabelField(valueRect, "Initial Value");
    }
}

}
