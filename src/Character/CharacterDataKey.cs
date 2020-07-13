using UnityEngine;

namespace Chankiyu22.DialogueSystem.Characters
{

public enum DataKeyType
{
    INTEGER,
    FLOAT,
    BOOLEAN,
    STRING,
}

public abstract class CharacterDataKey : ScriptableObject
{
    public abstract DataKeyType GetKeyType();

    public string GetKeyTypeName()
    {
        switch (GetKeyType())
        {
            case DataKeyType.INTEGER:
            {
                return "Integer";
            }
            case DataKeyType.FLOAT:
            {
                return "Float";
            }
            case DataKeyType.BOOLEAN:
            {
                return "Boolean";
            }
            case DataKeyType.STRING:
            {
                return "String";
            }
            default:
            {
                return "Undefined";
            }
        }
    }

    [SerializeField]
    protected string m_description = null;

    public string description
    {
        get
        {
            return m_description;
        }
    }
}

}
