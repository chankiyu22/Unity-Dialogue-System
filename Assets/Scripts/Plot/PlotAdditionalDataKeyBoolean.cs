using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

[CreateAssetMenu(menuName="Dialogue System/Plots/Data Key For Boolean")]
public class PlotAdditionalDataKeyBoolean : PlotAdditionalDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.BOOLEAN;
    }
}

}
