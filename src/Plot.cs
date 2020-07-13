using System;
using System.Collections.Generic;
using UnityEngine;

using Chankiyu22.DialogueSystem.Characters;
using Chankiyu22.DialogueSystem.Avatars;

namespace Chankiyu22.DialogueSystem
{

[Serializable]
public class PlotItem
{
    [SerializeField]
    private DialogueText m_dialogueText = null;

    public DialogueText dialogueText
    {
        get
        {
            return m_dialogueText;
        }

        set
        {
            m_dialogueText = value;
        }
    }

    [SerializeField]
    private Character m_character = null;

    public Character character
    {
        get
        {
            return m_character;
        }

        set
        {
            m_character = value;
        }
    }

    [SerializeField]
    private AvatarTextureSource m_avatarTextureSource = null;

    public AvatarTextureSource avatarTextureSource
    {
        get
        {
            return m_avatarTextureSource;
        }

        set
        {
            m_avatarTextureSource = value;
        }
    }
}

[CreateAssetMenu(menuName="Dialogue System/Plot")]
public class Plot : ScriptableObject
{
    [SerializeField]
    private Dialogue m_dialogue = null;

    [SerializeField]
    private List<PlotItem> m_plotItems = new List<PlotItem>();

    public List<PlotItem> plotItems
    {
        get
        {
            return m_plotItems;
        }
    }
}

}
