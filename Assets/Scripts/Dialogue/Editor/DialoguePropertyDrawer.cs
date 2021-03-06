using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CustomPropertyDrawer(typeof(Dialogue))]
class DialogueRefPopertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Dialogue dialogue = (Dialogue) property.objectReferenceValue;
        if (dialogue == null)
        {
            EditorGUI.PropertyField(position, property, label);
        }
        else
        {
            Rect propertyRect = new Rect(position.x, position.y, position.width - 105, position.height);
            Rect buttonRect = new Rect(position.xMax - 100, position.y, 100, position.height);
            EditorGUI.PropertyField(propertyRect, property, label);
            if (GUI.Button(buttonRect, "Open Editor", EditorStyles.miniButton))
            {
                DialogueEditorWindow.InitWindow(dialogue);
            }
        }
        EditorGUI.EndProperty();
    }
}

}
