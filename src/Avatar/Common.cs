using System;
using UnityEngine.Events;

namespace Chankiyu22.DialogueSystem.Avatars
{

[Serializable]
public class AvatarTextureSourceUnityEvent : UnityEvent<AvatarTextureSource> {}

public class AvatarTextureSourceEventArgs : EventArgs
{
    public AvatarTextureSource avatarTextureSource;
}

}
