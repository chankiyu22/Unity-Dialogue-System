using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Plots
{

[CustomEditor(typeof(PlotAdditionalDataDispatcher))]
public class PlotAdditionalDataDispatcherEditor : Editor
{
    SerializedProperty m_dataKeyEventsProp = null; 

    ReorderableList m_dataKeyEventReorderableList = null;

    void OnEnable()
    {
        m_dataKeyEventsProp = serializedObject.FindProperty("m_dataKeyEvents");
        m_dataKeyEventReorderableList = new ReorderableList(serializedObject, m_dataKeyEventsProp);
        m_dataKeyEventReorderableList.drawElementCallback = DrawDataKeyEventElement;
        m_dataKeyEventReorderableList.elementHeightCallback = GetDataKeyEventElementHeight;
        m_dataKeyEventReorderableList.onAddCallback = AddElement;
        m_dataKeyEventReorderableList.drawHeaderCallback = DrawHeader;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        m_dataKeyEventReorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Data Key Events");
    }

    void DrawDataKeyEventElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        rect.y += 2;
        SerializedProperty dataKeyEventProp = m_dataKeyEventsProp.GetArrayElementAtIndex(index);
        Rect dataKeyEventRect = new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(dataKeyEventProp));
        EditorGUI.PropertyField(dataKeyEventRect, dataKeyEventProp, GUIContent.none);
    }

    float GetDataKeyEventElementHeight(int index)
    {
        SerializedProperty dataKeyProp = m_dataKeyEventsProp.GetArrayElementAtIndex(index);
        return 2 + EditorGUI.GetPropertyHeight(dataKeyProp) + 2;
    }

    void AddElement(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        // Highlight new item
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dataKey").objectReferenceValue = null;
        element.FindPropertyRelative("m_intEvent.m_PersistentCalls.m_Calls").arraySize = 0;
        element.FindPropertyRelative("m_floatEvent.m_PersistentCalls.m_Calls").arraySize = 0;
        element.FindPropertyRelative("m_boolEvent.m_PersistentCalls.m_Calls").arraySize = 0;
        element.FindPropertyRelative("m_stringEvent.m_PersistentCalls.m_Calls").arraySize = 0;
    }
}

}