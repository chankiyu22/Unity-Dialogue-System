using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class VariableAssignmentListPropertyDrawerManager
{
    private Dictionary<string, VariableAssignmentListReorderableList> m_map = new Dictionary<string, VariableAssignmentListReorderableList>();

    public VariableAssignmentListReorderableList GetReorderableList(SerializedProperty property)
    {
        string propertyPath = property.propertyPath;
        if (!m_map.ContainsKey(propertyPath))
        {
            m_map.Add(propertyPath, CreateListReorderableList(property));
        }
        return m_map[propertyPath];
    }

    private VariableAssignmentListReorderableList CreateListReorderableList(SerializedProperty property)
    {
        return new VariableAssignmentListReorderableList(property.serializedObject, property);
    }
}

public class VariableAssignmentListReorderableList : ReorderableList
{

    public VariableAssignmentListReorderableList(SerializedObject serializedObject, SerializedProperty property)
        : base(serializedObject, property, true, true, true, true)
    {
        this.drawElementCallback = DrawElement;
        this.elementHeightCallback = GetElementHeight;
        this.drawHeaderCallback = DrawHeader;
    }

    void DrawElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        rect.y += 2;
        SerializedProperty variableAssignmentProp = serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, variableAssignmentProp);
    }

    public float GetElementHeight(int index)
    {
        SerializedProperty variableAssignmentProp = serializedProperty.GetArrayElementAtIndex(index);
        return EditorGUI.GetPropertyHeight(variableAssignmentProp) + 5;
    }

    public void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, new GUIContent("Change Variables"));
    }
}

}
