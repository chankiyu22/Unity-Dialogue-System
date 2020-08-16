using UnityEngine;

namespace Chankiyu22.DialogueSystem.Avatars
{

public class AvatarTextureController : MonoBehaviour
{

    [SerializeField]
    private AvatarTextureRenderController m_renderContoller = null;

    public void OnHandleAvatarTextureSource(AvatarTextureSource avatarTextureSource)
    {
        if (m_renderContoller != null)
        {
            m_renderContoller.ApplyTextureSource(avatarTextureSource);
        }
    }

}

}
