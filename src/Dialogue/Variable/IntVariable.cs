using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CreateAssetMenu(menuName="Dialogue System/Dialogue/Int Variable")]
public class IntVariable : Variable
{
    public override VariableType GetVariableType()
    {
        return VariableType.INTEGER;
    }

    public override string GetVariableTypeName()
    {
        return "Integer";
    }

    public override ConditionOperation[] GetAvailableOperations()
    {
        return new ConditionOperation[]
        {
            ConditionOperation.EQUAL,
            ConditionOperation.NOT_EQUAL,
            ConditionOperation.GREATER_THAN,
            ConditionOperation.GREATER_THAN_OR_EQUAL,
            ConditionOperation.LESS_THEN,
            ConditionOperation.LESS_THAN_OR_EQUAL
        };
    }
}

}
