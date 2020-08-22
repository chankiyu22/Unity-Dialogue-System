using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Characters
{

[CustomPropertyDrawer(typeof(DefaultCharacterDataKeyEvent))]
class DefaultCharacterDataKeyEventPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty dataKeyProp = property.FindPropertyRelative("m_dataKey");
        CharacterDataKey dataKey = (CharacterDataKey) dataKeyProp.objectReferenceValue;
        float dataKeyPropHeight = EditorGUI.GetPropertyHeight(dataKeyProp);
        Rect dataKeyRect = new Rect(position.x, position.y, position.width / 2 - 20, dataKeyPropHeight);
        EditorGUI.PropertyField(dataKeyRect, dataKeyProp, GUIContent.none);

        Rect equalSignRect = new Rect(position.xMax / 2, position.y, 20, dataKeyPropHeight);
        EditorGUI.LabelField(equalSignRect, "=");

        if (dataKey != null)
        {
            switch (dataKey.GetKeyType())
            {
                case DataKeyType.INTEGER:
                {
                    Rect defaultValueRect = new Rect(position.xMax / 2 + 20, position.y, position.xMax / 2 - 20, dataKeyPropHeight);
                    SerializedProperty defaultIntValueProp = property.FindPropertyRelative("m_defaultIntValue");
                    EditorGUI.PropertyField(defaultValueRect, defaultIntValueProp, GUIContent.none);
                    SerializedProperty intEventProp = property.FindPropertyRelative("m_OnIntEvents");
                    float intEventPropHeight = EditorGUI.GetPropertyHeight(intEventProp);
                    Rect eventRect = new Rect(position.x, position.y + dataKeyPropHeight + 2, position.width, intEventPropHeight);
                    EditorGUI.PropertyField(eventRect, intEventProp);
                    break;
                }
                case DataKeyType.FLOAT:
                {
                    Rect defaultValueRect = new Rect(position.xMax / 2 + 20, position.y, position.xMax / 2 - 20, dataKeyPropHeight);
                    SerializedProperty defaultFloatValueProp = property.FindPropertyRelative("m_defaultFloatValue");
                    EditorGUI.PropertyField(defaultValueRect, defaultFloatValueProp, GUIContent.none);
                    SerializedProperty floatEventProp = property.FindPropertyRelative("m_OnFloatEvents");
                    float floatEventPropHeight = EditorGUI.GetPropertyHeight(floatEventProp);
                    Rect eventRect = new Rect(position.x, position.y + dataKeyPropHeight + 2, position.width, floatEventPropHeight);
                    EditorGUI.PropertyField(eventRect, floatEventProp);
                    break;
                }
                case DataKeyType.BOOLEAN:
                {
                    Rect defaultValueRect = new Rect(position.xMax / 2 + 20, position.y, dataKeyPropHeight, dataKeyPropHeight);
                    SerializedProperty defaultBoolValueProp = property.FindPropertyRelative("m_defaultBoolValue");
                    EditorGUI.PropertyField(defaultValueRect, defaultBoolValueProp, GUIContent.none);
                    SerializedProperty boolEventProp = property.FindPropertyRelative("m_OnBoolEvents");
                    float boolEventPropHeight = EditorGUI.GetPropertyHeight(boolEventProp);
                    Rect eventRect = new Rect(position.x, position.y + dataKeyPropHeight + 2, position.width, boolEventPropHeight);
                    EditorGUI.PropertyField(eventRect, boolEventProp);
                    break;
                }
                case DataKeyType.STRING:
                {
                    Rect defaultValueRect = new Rect(position.xMax / 2 + 20, position.y, position.xMax / 2 - 20, dataKeyPropHeight);
                    SerializedProperty defaultStringValueProp = property.FindPropertyRelative("m_defaultStringValue");
                    EditorGUI.PropertyField(defaultValueRect, defaultStringValueProp, GUIContent.none);
                    SerializedProperty stringEventProp = property.FindPropertyRelative("m_OnStringEvents");
                    float stringEventPropHeight = EditorGUI.GetPropertyHeight(stringEventProp);
                    Rect eventRect = new Rect(position.x, position.y + dataKeyPropHeight + 2, position.width, stringEventPropHeight);
                    EditorGUI.PropertyField(eventRect, stringEventProp);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        else
        {
            using (new EditorGUI.DisabledScope(true))
            {
                Rect defaultValueRect = new Rect(position.xMax / 2 + 20, position.y, position.xMax / 2 - 20, dataKeyPropHeight);
                EditorGUI.TextField(defaultValueRect, "");
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty dataKeyProp = property.FindPropertyRelative("m_dataKey");
        CharacterDataKey dataKey = (CharacterDataKey) dataKeyProp.objectReferenceValue;

        float height = EditorGUI.GetPropertyHeight(dataKeyProp);

        if (dataKey != null)
        {
            switch (dataKey.GetKeyType())
            {
                case DataKeyType.INTEGER:
                {
                    SerializedProperty intEventProp = property.FindPropertyRelative("m_OnIntEvents");
                    height += EditorGUI.GetPropertyHeight(intEventProp);
                    break;
                }
                case DataKeyType.FLOAT:
                {
                    SerializedProperty floatEventProp = property.FindPropertyRelative("m_OnFloatEvents");
                    height += EditorGUI.GetPropertyHeight(floatEventProp);
                    break;
                }
                case DataKeyType.BOOLEAN:
                {
                    SerializedProperty boolEventProp = property.FindPropertyRelative("m_OnBoolEvents");
                    height += EditorGUI.GetPropertyHeight(boolEventProp);
                    break;
                }
                case DataKeyType.STRING:
                {
                    SerializedProperty stringEventProp = property.FindPropertyRelative("m_OnStringEvents");
                    height += EditorGUI.GetPropertyHeight(stringEventProp);
                    break;
                }
                default:
                {
                    break;
                }

            }
        }

        return height;
    }
}

}
