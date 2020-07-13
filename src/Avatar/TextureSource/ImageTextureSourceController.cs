using UnityEngine;
using UnityEngine.UI;

namespace Chankiyu22.DialogueSystem.Avatar
{

[RequireComponent(typeof(Image))]
public class ImageTextureSourceController : TextureSourceController
{
    private Image m_image = null;

    void Awake()
    {
        m_image = GetComponent<Image>();
    }
}

}
