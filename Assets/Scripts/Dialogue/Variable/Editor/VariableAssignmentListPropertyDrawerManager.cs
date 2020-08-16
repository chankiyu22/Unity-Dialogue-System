using System.Collections.Generic;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class VariableAssignmentListPropertyDrawerManager
{
    private Dictionary<string, VariableAssignmentReorderableList> m_map = new Dictionary<string, VariableAssignmentReorderableList>();

    public VariableAssignmentReorderableList GetReorderableList(SerializedProperty property, string header)
    {
        string propertyPath = property.propertyPath;
        if (!m_map.ContainsKey(propertyPath))
        {
            m_map.Add(propertyPath, CreateListReorderableList(property, header));
        }
        return m_map[propertyPath];
    }

    private VariableAssignmentReorderableList CreateListReorderableList(SerializedProperty property, string header)
    {
        return new VariableAssignmentReorderableList(property.serializedObject, property, header);
    }
}

}
