using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

public enum DataKeyType
{
    INTEGER,
    FLOAT,
    BOOLEAN,
    STRING,
    VOID,
}

public abstract class PlotAdditionalDataKey : ScriptableObject
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
            case DataKeyType.VOID:
            {
                return "Void";
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
