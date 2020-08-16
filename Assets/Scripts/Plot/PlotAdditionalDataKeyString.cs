using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

[CreateAssetMenu(menuName="Dialogue System/Plots/Data Key For String")]
public class PlotAdditionalDataKeyString : PlotAdditionalDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.STRING;
    }
}

}
