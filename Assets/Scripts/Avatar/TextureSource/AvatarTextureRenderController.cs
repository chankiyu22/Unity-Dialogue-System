using UnityEngine;

namespace Chankiyu22.DialogueSystem.Avatars
{

[ExecuteInEditMode]
public class AvatarTextureRenderController : MonoBehaviour
{
    [SerializeField]
    private AvatarTextureSource m_avatarTextureSource = null;
    private AvatarTextureSource m_prevAvatarTextureSource = null;

    private AvatarTextureSource m_avatarTextureSourceInstance = null;

    public void ApplyTextureSource(AvatarTextureSource textureSourceController)
    {
        m_avatarTextureSource = textureSourceController;
    }

    public void ResetTextureSourceContrller()
    {
        m_avatarTextureSource = null;
    }

    void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }

    void Update()
    {
        if (m_prevAvatarTextureSource != m_avatarTextureSource)
        {
            if (m_avatarTextureSourceInstance != null)
            {
                DestroyImmediate(m_avatarTextureSourceInstance.gameObject);
                m_avatarTextureSourceInstance = null;
            }

            if (m_avatarTextureSource)
            {
                m_avatarTextureSourceInstance = Instantiate(m_avatarTextureSource);
                m_avatarTextureSourceInstance.transform.SetParent(transform, false);
            }
        }

        m_prevAvatarTextureSource = m_avatarTextureSource;
    }
}

}
