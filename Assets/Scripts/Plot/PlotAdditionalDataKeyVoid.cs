using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

[CreateAssetMenu(menuName="Dialogue System/Plots/A Data Key")]
public class PlotAdditionalDataKeyVoid : PlotAdditionalDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.VOID;
    }
}

}

