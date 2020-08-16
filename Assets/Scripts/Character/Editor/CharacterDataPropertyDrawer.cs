using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Characters
{

[CustomPropertyDrawer(typeof(CharacterData))]
class CharacterDataPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int prevIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        Rect typeRect = new Rect(position.x, position.y, 75, position.height);
        Rect dataKeyRect = new Rect(position.x + 75, position.y, 110, position.height);
        Rect valueRect = new Rect(position.x + 190, position.y, position.width - 190, position.height);

        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty dataKeyProp = property.FindPropertyRelative("m_dataKey");

        EditorGUI.PropertyField(dataKeyRect, dataKeyProp, GUIContent.none);

        CharacterDataKey dataKey = (CharacterDataKey) dataKeyProp.objectReferenceValue;

        if (dataKey == null)
        {
            if (GUI.Button(typeRect, "New", EditorStyles.miniButtonLeft))
            {
                GenericMenu newKeyMenu = new GenericMenu();
                newKeyMenu.AddItem(new GUIContent("String"), false, () => {
                    CharacterDataKeyString newStringKey = PromptToCreate<CharacterDataKeyString>("New String Key", "Name");
                    dataKeyProp.objectReferenceValue = newStringKey;
                    property.serializedObject.ApplyModifiedProperties();
                });
                newKeyMenu.AddItem(new GUIContent("Integer"), false, () => {
                    CharacterDataKeyInteger newIntegerKey = PromptToCreate<CharacterDataKeyInteger>("New Integer Key", "Name");
                    dataKeyProp.objectReferenceValue = newIntegerKey;
                    property.serializedObject.ApplyModifiedProperties();
                });
                newKeyMenu.AddItem(new GUIContent("Float"), false, () => {
                    CharacterDataKeyFloat newFloatKey = PromptToCreate<CharacterDataKeyFloat>("New Float Key", "Name");
                    dataKeyProp.objectReferenceValue = newFloatKey;
                    property.serializedObject.ApplyModifiedProperties();
                });
                newKeyMenu.AddItem(new GUIContent("Boolean"), false, () => {
                    CharacterDataKeyBoolean newBooleanKey = PromptToCreate<CharacterDataKeyBoolean>("New Boolean Key", "Name");
                    dataKeyProp.objectReferenceValue = newBooleanKey;
                    property.serializedObject.ApplyModifiedProperties();
                });
                newKeyMenu.DropDown(new Rect(typeRect.x, typeRect.y, 0, typeRect.height));
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.TextField(valueRect, "");
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            switch (dataKey.GetKeyType())
            {
                case DataKeyType.INTEGER:
                {
                    EditorGUI.LabelField(typeRect, "Integer", EditorStyles.boldLabel);
                    SerializedProperty intValueProp = property.FindPropertyRelative("m_intValue");
                    EditorGUI.PropertyField(valueRect, intValueProp, GUIContent.none);
                    break;
                }
                case DataKeyType.FLOAT:
                {
                    EditorGUI.LabelField(typeRect, "Float", EditorStyles.boldLabel);
                    SerializedProperty floatValueProp = property.FindPropertyRelative("m_floatValue");
                    EditorGUI.PropertyField(valueRect, floatValueProp, GUIContent.none);
                    break;
                }
                case DataKeyType.BOOLEAN:
                {
                    EditorGUI.LabelField(typeRect, "Boolean", EditorStyles.boldLabel);
                    SerializedProperty boolValueProp = property.FindPropertyRelative("m_boolValue");
                    EditorGUI.PropertyField(valueRect, boolValueProp, GUIContent.none);
                    break;
                }
                case DataKeyType.STRING:
                {
                    EditorGUI.LabelField(typeRect, "String", EditorStyles.boldLabel);
                    SerializedProperty stringValueProp = property.FindPropertyRelative("m_stringValue");
                    EditorGUI.PropertyField(valueRect, stringValueProp, GUIContent.none);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = prevIndentLevel;
    }

    T PromptToCreate<T>(string title, string defaultName) where T : CharacterDataKey
    {
        string directoryPath = Utils.GetActiveDirectory();
        string createdAssetPath = EditorUtility.SaveFilePanel(title, directoryPath, defaultName, "asset");
        if (createdAssetPath.Length != 0)
        {
            createdAssetPath = createdAssetPath.Replace(Application.dataPath, "Assets");
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, createdAssetPath);
            AssetDatabase.SaveAssets();
            return asset;
        }
        return null;
    }
}

}
