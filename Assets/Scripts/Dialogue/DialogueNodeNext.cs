using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[Serializable]
public class DialogueNodeNext
{
    // Condition
    [SerializeField]
    private List<Condition> m_conditions = new List<Condition>();

    public List<Condition> conditions
    {
        get
        {
            return m_conditions;
        }
    }

    [SerializeField]
    private DialogueText m_next = null;

    public DialogueText next
    {
        get
        {
            return m_next;
        }
    }

    public bool EvaluateCondition(Dictionary<Variable, VariableValue> variableValues)
    {
        foreach (Condition condition in m_conditions)
        {
            Variable variable = condition.variable;
            if (variable == null)
            {
                continue;
            }
            if (!variableValues.ContainsKey(variable))
            {
                return false;
            }
            VariableValue variableValue = variableValues[variable];
            bool conditionEvaluationResult = false;
            switch (variable.GetVariableType())
            {
                case VariableType.INTEGER:
                    conditionEvaluationResult = condition.Evaluate(variableValue.intValue);
                    break;
                case VariableType.FLOAT:
                    conditionEvaluationResult = condition.Evaluate(variableValue.floatValue);
                    break;
                case VariableType.BOOLEAN:
                    conditionEvaluationResult = condition.Evaluate(variableValue.boolValue);
                    break;
                case VariableType.STRING:
                    conditionEvaluationResult = condition.Evaluate(variableValue.stringValue);
                    break;
            }
            if (conditionEvaluationResult == false)
            {
                return false;
            }
        }
        return true;
    }
}

}
