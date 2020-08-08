using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Plots
{

public class PlotAdditionalDataReorderableListManager
{
    Dictionary<string, PlotAdditionalDataReorderableList> m_map = new Dictionary<string, PlotAdditionalDataReorderableList>();

    public PlotAdditionalDataReorderableList GetReorderableList(SerializedProperty property)
    {
        string propertyPath = property.propertyPath;
        if (!m_map.ContainsKey(propertyPath))
        {
            m_map.Add(propertyPath, CreateListReorderableList(property));
        }
        return m_map[propertyPath];
    }

    private PlotAdditionalDataReorderableList CreateListReorderableList(SerializedProperty property)
    {
        return new PlotAdditionalDataReorderableList(property.serializedObject, property);
    }
}

public class PlotAdditionalDataReorderableList : ReorderableList
{
    public PlotAdditionalDataReorderableList(SerializedObject serializedObject, SerializedProperty property)
        : base(serializedObject, property, true, true, true, true)
    {
        this.drawElementCallback = DrawElement;
        this.elementHeightCallback = GetElementHeight;
        this.drawHeaderCallback = DrawHeader;
        this.onAddCallback = AddElement;
    }

    void DrawElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        rect.y += 2;
        SerializedProperty plotAdditionalDataProp = serializedProperty.GetArrayElementAtIndex(index);
        Rect plotAdditionalDataRect = new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(plotAdditionalDataProp));
        EditorGUI.PropertyField(plotAdditionalDataRect, plotAdditionalDataProp);
    }

    public float GetElementHeight(int index)
    {
        SerializedProperty plotAdditionalDataProp = serializedProperty.GetArrayElementAtIndex(index);
        return EditorGUI.GetPropertyHeight(plotAdditionalDataProp) + 5;
    }

    public void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, new GUIContent("Additional Data"));
    }

    void AddElement(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        // Highlight new item
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dataKey").objectReferenceValue = null;
        element.FindPropertyRelative("m_intValue").intValue = 0;
        element.FindPropertyRelative("m_floatValue").floatValue = 0;
        element.FindPropertyRelative("m_boolValue").boolValue = false;
        element.FindPropertyRelative("m_stringValue").stringValue = null;
    }
}

}
