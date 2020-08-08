using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

[CreateAssetMenu(menuName="Dialogue System/Plots/Data Key For Float")]
public class PlotAdditionalDataKeyFloat : PlotAdditionalDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.FLOAT;
    }
}

}
