using System;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class VariableValue
{
    private Variable m_variable;

    private int m_intValue;
    private float m_floatValue;
    private bool m_boolValue;
    private string m_stringValue;

    public VariableValue(IntVariable variable, int intValue)
    {
        m_variable = variable;
        m_intValue = intValue;
    }

    public VariableValue(FloatVariable variable, float floatValue)
    {
        m_variable = variable;
        m_floatValue = floatValue;
    }

    public VariableValue(BoolVariable variable, bool boolValue)
    {
        m_variable = variable;
        m_boolValue = boolValue;
    }

    public VariableValue(StringVariable variable, string stringValue)
    {
        m_variable = variable;
        m_stringValue = stringValue;
    }

    public void SetValue(int intValue)
    {
        if (m_variable.GetVariableType() != VariableType.INTEGER)
        {
            throw new Exception("Invalid Int Value");
        }
        m_intValue = intValue;
    }

    public void SetValue(float floatValue)
    {
        if (m_variable.GetVariableType() != VariableType.FLOAT)
        {
            throw new Exception("Invalid Float Value");
        }
        m_floatValue = floatValue;
    }

    public void SetValue(bool boolValue)
    {
        if (m_variable.GetVariableType() != VariableType.BOOLEAN)
        {
            throw new Exception("Invalid Boolean Value");
        }
        m_boolValue = boolValue;
    }

    public void SetValue(string stringValue)
    {
        if (m_variable.GetVariableType() != VariableType.STRING)
        {
            throw new Exception("Invalid String Value");
        }
        m_stringValue = stringValue;
    }
}

}
