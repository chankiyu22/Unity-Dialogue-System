using UnityEngine;

namespace Chankiyu22.DialogueSystem.Characters
{

[CreateAssetMenu(menuName="Dialogue System/Characters/Data Key For String")]
public class CharacterDataKeyString : CharacterDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.STRING;
    }
}

}
