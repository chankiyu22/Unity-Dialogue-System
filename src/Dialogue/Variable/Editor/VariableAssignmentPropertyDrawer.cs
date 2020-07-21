using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CustomPropertyDrawer(typeof(VariableAssignment))]
class VariableAssignmentPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty variableProp = property.FindPropertyRelative("m_variable");
        Variable variable = (Variable) variableProp.objectReferenceValue;

        Rect typeLabelRect = new Rect(position.x, position.y, 60, EditorGUIUtility.singleLineHeight);
        Rect variablePropRect = new Rect(position.x + typeLabelRect.width, position.y, position.width * 0.5f - typeLabelRect.width, EditorGUIUtility.singleLineHeight);
        Rect equalLabelRect = new Rect(position.x + typeLabelRect.width + variablePropRect.width, position.y, 20, EditorGUIUtility.singleLineHeight);
        Rect valuePropRect = new Rect(position.x + typeLabelRect.width + variablePropRect.width + equalLabelRect.width, position.y, position.width - typeLabelRect.width - variablePropRect.width - equalLabelRect.width, EditorGUIUtility.singleLineHeight);

        if (variable == null)
        {
            EditorGUI.LabelField(typeLabelRect, "");
        }
        else
        {
            EditorGUI.LabelField(typeLabelRect, variable.GetVariableTypeName(), EditorStyles.boldLabel);
        }
        EditorGUI.PropertyField(variablePropRect, variableProp, GUIContent.none);
        EditorGUI.LabelField(equalLabelRect, "=", EditorStyles.boldLabel);

        if (variable == null)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.TextField(valuePropRect, "");
            }
        }
        else
        {
            VariableType variableType = variable.GetVariableType();
            switch (variableType)
            {
                case VariableType.INTEGER:
                {
                    SerializedProperty intValueProp = property.FindPropertyRelative("m_intValue");
                    EditorGUI.PropertyField(valuePropRect, intValueProp, GUIContent.none);
                    break;
                }
                case VariableType.FLOAT:
                {
                    SerializedProperty floatValueProp = property.FindPropertyRelative("m_floatValue");
                    EditorGUI.PropertyField(valuePropRect, floatValueProp, GUIContent.none);
                    break;
                }
                case VariableType.BOOLEAN:
                {
                    SerializedProperty boolValueProp = property.FindPropertyRelative("m_boolValue");
                    EditorGUI.PropertyField(valuePropRect, boolValueProp, GUIContent.none);
                    break;
                }
                case VariableType.STRING:
                {
                    SerializedProperty stringValueProp = property.FindPropertyRelative("m_stringValue");
                    EditorGUI.PropertyField(valuePropRect, stringValueProp, GUIContent.none);
                    break;
                }
            }
        }

        EditorGUI.EndProperty();
    }
}

}
