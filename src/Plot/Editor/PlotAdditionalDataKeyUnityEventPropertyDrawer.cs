using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Plots
{


[CustomPropertyDrawer(typeof(PlotAdditionalDataKeyUnityEvent))]
public class PlotAdditionalDataKeyUnityEventPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int prevIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty dataKeyProp = property.FindPropertyRelative("m_dataKey");
        SerializedProperty intEventProp = property.FindPropertyRelative("m_intEvent");
        SerializedProperty floatEventProp = property.FindPropertyRelative("m_floatEvent");
        SerializedProperty boolEventProp = property.FindPropertyRelative("m_boolEvent");
        SerializedProperty stringEventProp = property.FindPropertyRelative("m_stringEvent");
        SerializedProperty voidEventProp = property.FindPropertyRelative("m_voidEvent");

        Rect dataKeyRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(dataKeyProp));
        EditorGUI.PropertyField(dataKeyRect, dataKeyProp, GUIContent.none);

        PlotAdditionalDataKey dataKey = (PlotAdditionalDataKey) dataKeyProp.objectReferenceValue;

        if (dataKey != null)
        {
            position.y += dataKeyRect.height + 2;

            switch (dataKey.GetKeyType())
            {
                case DataKeyType.INTEGER:
                    Rect intEventRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(intEventProp));
                    EditorGUI.PropertyField(intEventRect, intEventProp);
                    break;
                case DataKeyType.FLOAT:
                    Rect floatEventRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(floatEventProp));
                    EditorGUI.PropertyField(floatEventRect, floatEventProp);
                    break;
                case DataKeyType.BOOLEAN:
                    Rect boolEventRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(boolEventProp));
                    EditorGUI.PropertyField(boolEventRect, boolEventProp);
                    break;
                case DataKeyType.STRING:
                    Rect stringEventRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(stringEventProp));
                    EditorGUI.PropertyField(stringEventRect, stringEventProp);
                    break;
                case DataKeyType.VOID:
                    Rect voidEventRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(voidEventProp));
                    EditorGUI.PropertyField(voidEventRect, voidEventProp);
                    break;
            }
        }

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = prevIndentLevel;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty dataKeyProp = property.FindPropertyRelative("m_dataKey");
        SerializedProperty intEventProp = property.FindPropertyRelative("m_intEvent");
        SerializedProperty floatEventProp = property.FindPropertyRelative("m_floatEvent");
        SerializedProperty boolEventProp = property.FindPropertyRelative("m_boolEvent");
        SerializedProperty stringEventProp = property.FindPropertyRelative("m_stringEvent");
        SerializedProperty voidEventProp = property.FindPropertyRelative("m_voidEvent");

        float result = EditorGUI.GetPropertyHeight(dataKeyProp);

        PlotAdditionalDataKey dataKey = (PlotAdditionalDataKey) dataKeyProp.objectReferenceValue;
        if (dataKey == null)
        {
            return result;
        }

        result += 2;

        switch (dataKey.GetKeyType())
        {
            case DataKeyType.INTEGER:
                result += EditorGUI.GetPropertyHeight(intEventProp);
                break;
            case DataKeyType.FLOAT:
                result += EditorGUI.GetPropertyHeight(floatEventProp);
                break;
            case DataKeyType.BOOLEAN:
                result += EditorGUI.GetPropertyHeight(boolEventProp);
                break;
            case DataKeyType.STRING:
                result += EditorGUI.GetPropertyHeight(stringEventProp);
                break;
            case DataKeyType.VOID:
                result += EditorGUI.GetPropertyHeight(voidEventProp);
                break;
        }

        return result;
    }
}

}