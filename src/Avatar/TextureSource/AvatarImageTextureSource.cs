using UnityEngine;
using UnityEngine.UI;

namespace Chankiyu22.DialogueSystem.Avatars
{

[RequireComponent(typeof(Image))]
public class AvatarImageTextureSource : AvatarTextureSource
{
    private Image m_image = null;

    void Awake()
    {
        m_image = GetComponent<Image>();
    }
}

}
