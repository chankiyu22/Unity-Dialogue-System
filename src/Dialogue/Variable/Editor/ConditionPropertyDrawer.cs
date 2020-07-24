using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CustomPropertyDrawer(typeof(Condition))]
public class ConditionPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect rowRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect variableRect = new Rect(rowRect.x, rowRect.y, rowRect.width / 2 - 5, rowRect.height);
        Rect operationRect = new Rect((rowRect.x + rowRect.xMax) / 2, rowRect.y, 25, rowRect.height);
        Rect valueRect = new Rect((rowRect.x + rowRect.xMax) / 2 + 25, rowRect.y, rowRect.width / 2 - 25, rowRect.height);

        SerializedProperty variableProp = property.FindPropertyRelative("m_variable");
        Variable variable = (Variable) variableProp.objectReferenceValue;

        EditorGUI.PropertyField(variableRect, variableProp, GUIContent.none);

        SerializedProperty operationProp = property.FindPropertyRelative("m_operation");
        using (new EditorGUI.DisabledScope(variable == null))
        {
            if (EditorGUI.DropdownButton(operationRect, new GUIContent(GetOperationText(operationProp)), FocusType.Passive, EditorStyles.miniButtonLeft))
            {
                GenericMenu operationMenu = new GenericMenu();
                ConditionOperation[] availableOperations = variable.GetAvailableOperations();
                foreach (ConditionOperation operation in availableOperations)
                {
                    operationMenu.AddItem(new GUIContent(Condition.GetOperationText(operation)), false, () => {
                        operationProp.intValue = (int) operation;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                operationMenu.DropDown(operationRect);
            }
        }

        if (variable == null)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.TextField(valueRect, "");
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
                    EditorGUI.PropertyField(valueRect, intValueProp, GUIContent.none);
                    break;
                }
                case VariableType.FLOAT:
                {
                    SerializedProperty floatValueProp = property.FindPropertyRelative("m_floatValue");
                    EditorGUI.PropertyField(valueRect, floatValueProp, GUIContent.none);
                    break;
                }
                case VariableType.BOOLEAN:
                {
                    SerializedProperty boolValueProp = property.FindPropertyRelative("m_boolValue");
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUI.LabelField(valueRect, "", EditorStyles.miniButtonRight);
                    }
                    EditorGUI.PropertyField(new Rect(valueRect.x + 4, valueRect.y, valueRect.width - 4, valueRect.height), boolValueProp, GUIContent.none);
                    break;
                }
                case VariableType.STRING:
                {
                    SerializedProperty stringValueProp = property.FindPropertyRelative("m_stringValue");
                    EditorGUI.PropertyField(valueRect, stringValueProp, GUIContent.none);
                    break;
                }
            }
        }

        EditorGUI.EndProperty();
    }

    string GetOperationText(SerializedProperty operationProp)
    {
        ConditionOperation operation = (ConditionOperation) operationProp.intValue;
        return Condition.GetOperationText(operation);
    }
}

}
