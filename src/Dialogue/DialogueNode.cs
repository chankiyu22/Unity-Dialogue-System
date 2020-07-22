using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public enum DialogueNextOption
{
    END = 0,
    DIALOGUE_TEXT = 1,
    DIALOGUE_OPTIONS = 2,
}

[Serializable]
public class DialogueNode
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
    private List<VariableAssignment> m_assignments = new List<VariableAssignment>();

    public List<VariableAssignment> assignments
    {
        get
        {
            return m_assignments;
        }
    }

    [SerializeField]
    private DialogueNextOption m_nextOption = DialogueNextOption.END;

    public DialogueNextOption nextOption
    {
        get
        {
            return m_nextOption;
        }
    }

    // DIALOGUE TEXT
    [SerializeField]
    private DialogueText m_next = null;

    public DialogueText next
    {
        get
        {
            return m_next;
        }
    }


    // DIALOGUE_OPTIONS
    [SerializeField]
    private List<DialogueOption> m_options = null;

    public List<DialogueOption> options
    {
        get
        {
            return m_options;
        }
    }

    [SerializeField]
    private List<DialogueNodeNext> m_nexts = new List<DialogueNodeNext>();

    public List<DialogueNodeNext> nexts
    {
        get
        {
            return m_nexts;
        }
    }

    private DialogueText m_finalNext = null;

    public DialogueText finalNext
    {
        get
        {
            return m_finalNext;
        }
    }
}

}
