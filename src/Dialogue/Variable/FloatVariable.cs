using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CreateAssetMenu(menuName="Dialogue System/Dialogue/Float Variable")]
public class FloatVariable : Variable
{
    public override VariableType GetVariableType()
    {
        return VariableType.FLOAT;
    }

    public override string GetVariableTypeName()
    {
        return "Float";
    }
}

}
