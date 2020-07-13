using UnityEngine;

namespace Chankiyu22.DialogueSystem.Characters
{

[CreateAssetMenu(menuName="Dialogue System/Characters/Data Key For Boolean")]
public class CharacterDataKeyBoolean : CharacterDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.BOOLEAN;
    }
}

}
