using UnityEngine;
using UnityEngine.UI;

namespace Chankiyu22.DialogueSystem.Avatars
{

[RequireComponent(typeof(RawImage))]
public class AvatarRawImageTextureSource : AvatarTextureSource
{
    private RawImage m_rawImage = null;

    void Awake()
    {
        m_rawImage = GetComponent<RawImage>();
    }
}

}
