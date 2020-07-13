using UnityEngine;

namespace Chankiyu22.DialogueSystem.Characters
{

[CreateAssetMenu(menuName="Dialogue System/Characters/Data Key For Float")]
public class CharacterDataKeyFloat : CharacterDataKey
{
    public override DataKeyType GetKeyType()
    {
        return DataKeyType.FLOAT;
    }
}

}
