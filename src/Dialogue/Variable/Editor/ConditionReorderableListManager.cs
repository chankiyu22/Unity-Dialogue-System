using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class ConditionReorderableListManager
{
    Dictionary<string, ConditionReorderableList> m_map = new Dictionary<string, ConditionReorderableList>();

    public ConditionReorderableList GetReorderableList(SerializedProperty property)
    {
        string propertyPath = property.propertyPath;
        if (!m_map.ContainsKey(propertyPath))
        {
            m_map.Add(propertyPath, CreateListReorderableList(property));
        }
        return m_map[propertyPath];
    }

    private ConditionReorderableList CreateListReorderableList(SerializedProperty property)
    {
        return new ConditionReorderableList(property.serializedObject, property);
    }
}

public class ConditionReorderableList : ReorderableList
{
    public ConditionReorderableList(SerializedObject serializedObject, SerializedProperty property)
        : base(serializedObject, property, true, true, true, true)
    {
        this.drawElementCallback = DrawElement;
        this.elementHeightCallback = GetElementHeight;
        this.drawHeaderCallback = DrawHeader;
    }

    void DrawElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        rect.y += 2;
        SerializedProperty conditionProp = serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, conditionProp);
    }

    public float GetElementHeight(int index)
    {
        SerializedProperty conditionProp = serializedProperty.GetArrayElementAtIndex(index);
        return EditorGUI.GetPropertyHeight(conditionProp) + 5;
    }

    public void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, new GUIContent("Conditions"));
    }
}

}
