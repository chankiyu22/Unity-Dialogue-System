using UnityEngine;
using UnityEngine.UI;

namespace Chankiyu22.DialogueSystem.Avatar
{

[RequireComponent(typeof(RawImage))]
public class RawImageTextureSourceController : TextureSourceController
{
    private RawImage m_rawImage = null;

    void Awake()
    {
        m_rawImage = GetComponent<RawImage>();
    }
}

}
