using UnityEngine;
using Unity;
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

    public override Texture GetPreviewTexture()
    {
        Image image = GetComponent<Image>();
        if (image != null && image.sprite != null)
        {
            Sprite sprite = image.sprite;
            Rect textureRect;
            try
            {
                textureRect = sprite.textureRect;
            }
            catch
            {
                return sprite.texture;
            }
            if (sprite.texture.isReadable)
            {
                Texture2D croppedTexture = new Texture2D((int) textureRect.width, (int) textureRect.height);
                Color[] pixels = sprite.texture.GetPixels((int) textureRect.x, (int) textureRect.y, (int) textureRect.width, (int) textureRect.height);
                croppedTexture.SetPixels(pixels);
                croppedTexture.Apply();
                return croppedTexture;
            }
            else
            {
                return sprite.texture;
            }
        }
        return null;
    }
}

}
