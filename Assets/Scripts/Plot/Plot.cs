using System;
using System.Collections.Generic;
using UnityEngine;

using Chankiyu22.DialogueSystem.Dialogues;
using Chankiyu22.DialogueSystem.Characters;
using Chankiyu22.DialogueSystem.Avatars;

namespace Chankiyu22.DialogueSystem.Plots
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

    [SerializeField]
    private List<PlotAdditionalData> m_additionalDataList = new List<PlotAdditionalData>();

    public List<PlotAdditionalData> additionalDataList
    {
        get
        {
            return m_additionalDataList;
        }
    }
}

[CreateAssetMenu(menuName="Dialogue System/Plots/Plot")]
public class Plot : ScriptableObject
{
    [SerializeField]
    private Dialogue m_dialogue = null;

    public Dialogue dialogue
    {
        get
        {
            return m_dialogue;
        }
    }

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
