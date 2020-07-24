using System;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public enum ConditionOperation
{
    EQUAL,
    NOT_EQUAL,
    GREATER_THAN,
    GREATER_THAN_OR_EQUAL,
    LESS_THEN,
    LESS_THAN_OR_EQUAL,
}

[Serializable]
public class Condition
{
    [SerializeField]
    private Variable m_variable = null;

    public Variable variable
    {
        get
        {
            return m_variable;
        }
    }

    [SerializeField]
    private ConditionOperation m_operation = ConditionOperation.EQUAL;

    public ConditionOperation operation
    {
        get
        {
            return m_operation;
        }
    }

    [SerializeField]
    private int m_intValue = 0;

    public int intValue
    {
        get
        {
            return m_intValue;
        }
    }

    [SerializeField]
    private float m_floatValue = 0;

    public float floatValue
    {
        get
        {
            return m_floatValue;
        }
    }

    [SerializeField]
    private bool m_boolValue = false;

    public bool boolValue
    {
        get
        {
            return m_boolValue;
        }
    }

    [SerializeField]
    private string m_stringValue = null;

    public string stringValue
    {
        get
        {
            return m_stringValue;
        }
    }

    public static string GetOperationText(ConditionOperation operation)
    {
        switch (operation)
        {
            case ConditionOperation.EQUAL:
                return "=";
            case ConditionOperation.NOT_EQUAL:
                return "≠";
            case ConditionOperation.GREATER_THAN:
                return ">";
            case ConditionOperation.GREATER_THAN_OR_EQUAL:
                return "\u2265";
            case ConditionOperation.LESS_THEN:
                return "<";
            case ConditionOperation.LESS_THAN_OR_EQUAL:
                return "\u2264";
        }
        return "";
    }
}

}
