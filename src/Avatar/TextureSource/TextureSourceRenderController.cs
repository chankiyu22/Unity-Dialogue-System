using UnityEngine;

namespace Chankiyu22.DialogueSystem.Avatar
{

[ExecuteInEditMode]
public class TextureSourceRenderController : MonoBehaviour
{
    [SerializeField]
    private TextureSourceController m_textureSourceController = null;
    private TextureSourceController m_prevTextureSourceController = null;

    private TextureSourceController m_textureSourceControllerInstance = null;

    public void ApplyTextureSourceController(TextureSourceController textureSourceController)
    {
        m_textureSourceController = textureSourceController;
    }

    public void ResetTextureSourceContrller()
    {
        m_textureSourceController = null;
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
        if (m_prevTextureSourceController != m_textureSourceController)
        {
            if (m_textureSourceControllerInstance != null)
            {
                DestroyImmediate(m_textureSourceControllerInstance.gameObject);
                m_textureSourceControllerInstance = null;
            }

            if (m_textureSourceController)
            {
                m_textureSourceControllerInstance = Instantiate(m_textureSourceController);
                m_textureSourceControllerInstance.transform.SetParent(transform, false);
            }
        }

        m_prevTextureSourceController = m_textureSourceController;
    }
}

}
