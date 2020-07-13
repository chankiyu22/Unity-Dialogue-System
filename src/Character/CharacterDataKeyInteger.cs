using UnityEngine;

namespace Chankiyu22.DialogueSystem.Characters
{

[CreateAssetMenu(menuName="Dialogue System/Characters/Data Key For Integer")]
public class CharacterDataKeyInteger : CharacterDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.INTEGER;
    }
}

}
