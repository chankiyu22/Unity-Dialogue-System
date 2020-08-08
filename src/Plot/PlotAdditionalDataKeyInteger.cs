using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

[CreateAssetMenu(menuName="Dialogue System/Plots/Data Key For Integer")]
public class PlotAdditionalDataKeyInteger : PlotAdditionalDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.INTEGER;
    }
}

}
