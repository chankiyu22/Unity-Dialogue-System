using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Characters
{

[CustomPropertyDrawer(typeof(CharacterDataKeyEvent))]
class CharacterDataKeyEventPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty dataKeyProp = property.FindPropertyRelative("m_dataKey");
        CharacterDataKey dataKey = (CharacterDataKey) dataKeyProp.objectReferenceValue;
        float dataKeyPropHeight = EditorGUI.GetPropertyHeight(dataKeyProp);
        Rect dataKeyRect = new Rect(position.x, position.y, position.width, dataKeyPropHeight);
        EditorGUI.PropertyField(dataKeyRect, dataKeyProp, GUIContent.none);

        if (dataKey != null)
        {
            switch (dataKey.GetKeyType())
            {
                case DataKeyType.INTEGER:
                {
                    SerializedProperty intEventProp = property.FindPropertyRelative("m_OnIntEvents");
                    float intEventPropHeight = EditorGUI.GetPropertyHeight(intEventProp);
                    Rect eventRect = new Rect(position.x, position.y + dataKeyPropHeight + 2, position.width, intEventPropHeight);
                    EditorGUI.PropertyField(eventRect, intEventProp);
                    break;
                }
                case DataKeyType.FLOAT:
                {
                    SerializedProperty floatEventProp = property.FindPropertyRelative("m_OnFloatEvents");
                    float floatEventPropHeight = EditorGUI.GetPropertyHeight(floatEventProp);
                    Rect eventRect = new Rect(position.x, position.y + dataKeyPropHeight + 2, position.width, floatEventPropHeight);
                    EditorGUI.PropertyField(eventRect, floatEventProp);
                    break;
                }
                case DataKeyType.BOOLEAN:
                {
                    SerializedProperty boolEventProp = property.FindPropertyRelative("m_OnBoolEvents");
                    float boolEventPropHeight = EditorGUI.GetPropertyHeight(boolEventProp);
                    Rect eventRect = new Rect(position.x, position.y + dataKeyPropHeight + 2, position.width, boolEventPropHeight);
                    EditorGUI.PropertyField(eventRect, boolEventProp);
                    break;
                }
                case DataKeyType.STRING:
                {
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
