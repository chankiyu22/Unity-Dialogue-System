using System;
using UnityEngine.Events;

namespace Chankiyu22.DialogueSystem.Avatar
{

[Serializable]
public class TextureSourceUnityEvent : UnityEvent<TextureSourceController> {}

public class TextureSourceEventArgs : EventArgs
{
    public TextureSourceController textureSourceController;
}

}
