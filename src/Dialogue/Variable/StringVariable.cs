using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CreateAssetMenu(menuName="Dialogue System/Dialogue/String Variable")]
public class StringVariable : Variable
{
    public override VariableType GetVariableType()
    {
        return VariableType.STRING;
    }

    public override string GetVariableTypeName()
    {
        return "String";
    }
}

}
