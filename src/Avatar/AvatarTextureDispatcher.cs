using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Avatar
{

[Serializable]
class DialogueTextAvatarTextureSource
{
    [SerializeField]
    private DialogueText m_dialogueText = null;

    public DialogueText dialogueText
    {
        get
        {
            return m_dialogueText;
        }
    }

    [SerializeField]
    private TextureSourceController m_textureSourceController = null;

    public TextureSourceController textureSourceController
    {
        get
        {
            return m_textureSourceController;
        }
    }

    [SerializeField]
    private TextureSourceRenderController m_textureSourceRenderController = null;

    public TextureSourceRenderController textureSourceRenderController
    {
        get
        {
            return m_textureSourceRenderController;
        }
    }
}

public class AvatarTextureDispatcher : MonoBehaviour
{
    [SerializeField]
    private List<DialogueTextAvatarTextureSource> m_dialogueTextAvatars = new List<DialogueTextAvatarTextureSource>();

    private Dictionary<DialogueText, DialogueTextAvatarTextureSource> m_mapping = new Dictionary<DialogueText, DialogueTextAvatarTextureSource>();

    [SerializeField]
    private TextureSourceUnityEvent m_OnTextureSource = null;

    public event EventHandler<TextureSourceEventArgs> OnTextureSource;

    void Awake()
    {
        foreach (DialogueTextAvatarTextureSource d in m_dialogueTextAvatars)
        {
            m_mapping.Add(d.dialogueText, d);
        }
    }

    public void Dispatch(DialogueText dialogueText)
    {
        if (m_mapping.ContainsKey(dialogueText))
        {
            DialogueTextAvatarTextureSource d = m_mapping[dialogueText];
            TextureSourceController textureSourceController = d.textureSourceController;
            TextureSourceRenderController textureSourceRenderController = d.textureSourceRenderController;
            textureSourceRenderController.ApplyTextureSourceController(textureSourceController);
            EmitTextureSource(textureSourceController);
        }
    }

    void EmitTextureSource(TextureSourceController textureSourceController)
    {
        m_OnTextureSource.Invoke(textureSourceController);

        if (OnTextureSource != null)
        {
            OnTextureSource.Invoke(this, new TextureSourceEventArgs() {
                textureSourceController = textureSourceController
            });
        }
    }
}

}
