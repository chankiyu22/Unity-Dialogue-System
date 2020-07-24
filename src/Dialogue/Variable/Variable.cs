using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public enum VariableType
{
    INTEGER,
    FLOAT,
    BOOLEAN,
    STRING,
}

public abstract class Variable : ScriptableObject
{
    public abstract VariableType GetVariableType();

    public abstract string GetVariableTypeName();

    [SerializeField]
    private string m_description = null;

    public string description
    {
        get
        {
            return m_description;
        }
    }

    public abstract ConditionOperation[] GetAvailableOperations();
}

}
