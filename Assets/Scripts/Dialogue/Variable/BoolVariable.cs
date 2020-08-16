using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CreateAssetMenu(menuName="Dialogue System/Dialogue/Bool Variable")]
public class BoolVariable : Variable
{
    public override VariableType GetVariableType()
    {
        return VariableType.BOOLEAN;
    }

    public override string GetVariableTypeName()
    {
        return "Boolean";
    }

    public override ConditionOperation[] GetAvailableOperations()
    {
        return new ConditionOperation[]
        {
            ConditionOperation.EQUAL,
            ConditionOperation.NOT_EQUAL,
        };
    }
}

}
