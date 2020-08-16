using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Characters
{

[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
    SerializedProperty dataListProp;
    ReorderableList dataReorderableList;

    void OnEnable()
    {
        dataListProp = serializedObject.FindProperty("m_dataList");
        dataReorderableList = new ReorderableList(serializedObject, dataListProp, true, true, true, true);
        dataReorderableList.drawElementCallback = DrawDataListElement;
        dataReorderableList.onAddCallback = AddElement;
        dataReorderableList.drawHeaderCallback = DrawDataListHeader;
        dataReorderableList.onRemoveCallback = RemoveElement;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Description");
        Rect descriptionRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 3);
        SerializedProperty descriptionProp = serializedObject.FindProperty("m_description");
        descriptionProp.stringValue = EditorGUI.TextArea(descriptionRect, descriptionProp.stringValue);

        EditorGUILayout.Space();

        dataReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawDataListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Data");
    }

    void DrawDataListElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        SerializedProperty characterDataProp = dataListProp.GetArrayElementAtIndex(index);

        rect.y += 2;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), characterDataProp);
    }

    void AddElement(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dataKey").objectReferenceValue = null;
        element.FindPropertyRelative("m_intValue").intValue = 0;
        element.FindPropertyRelative("m_floatValue").floatValue = 0;
        element.FindPropertyRelative("m_boolValue").boolValue = false;
        element.FindPropertyRelative("m_stringValue").stringValue = null;
    }

    void RemoveElement(ReorderableList l)
    {
        if (EditorUtility.DisplayDialog("Delete Data Field", "Are you sure you want to delete?", "Yes", "No"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        }
    }
}

}
