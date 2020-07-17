using System;
using System.Collections.Generic;
using UnityEngine;

using Chankiyu22.DialogueSystem.Characters;

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
    }

    [SerializeField]
    private Character m_character = null;

    public Character character
    {
        get
        {
            return m_character;
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
}

}
