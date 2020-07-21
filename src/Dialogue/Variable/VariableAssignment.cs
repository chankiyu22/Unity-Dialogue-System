using System;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[Serializable]
public class VariableAssignment
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
}

}
