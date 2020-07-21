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
}

}
