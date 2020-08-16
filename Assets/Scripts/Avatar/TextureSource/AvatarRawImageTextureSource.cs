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

    public override Texture GetPreviewTexture()
    {
        RawImage rawImage = GetComponent<RawImage>();
        if (rawImage != null)
        {
            return rawImage.mainTexture;
        }
        return null;
    }
}

}
