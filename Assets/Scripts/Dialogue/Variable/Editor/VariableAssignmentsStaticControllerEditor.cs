using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CustomEditor(typeof(VariableAssignmentsStaticController))]
public class VariableAssignmentsStaticControllerEditor : Editor
{
    VariableAssignmentReorderableList m_reorderableList;
    SerializedProperty m_variableAssignmentsProp;

    void OnEnable()
    {
        m_variableAssignmentsProp = serializedObject.FindProperty("m_variableAssignments");
        m_reorderableList = new VariableAssignmentReorderableList(serializedObject, m_variableAssignmentsProp, "Static Variable Assignments");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        Rect reorderableListRect = EditorGUILayout.GetControlRect(false, m_reorderableList.GetHeight());
        m_reorderableList.DoList(reorderableListRect);

        serializedObject.ApplyModifiedProperties();
    }
}

}
