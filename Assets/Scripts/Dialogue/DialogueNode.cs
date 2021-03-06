using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

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

    [SerializeField]
    private DialogueText m_finalNext = null;

    public DialogueText finalNext
    {
        get
        {
            return m_finalNext;
        }
    }

    public DialogueText GetNextDialogueText(Dictionary<Variable, VariableValue> variableValues)
    {
        foreach (DialogueNodeNext dialogueNodeNext in m_nexts)
        {
            if (dialogueNodeNext.EvaluateCondition(variableValues))
            {
                return dialogueNodeNext.next;
            }
        }
        return m_finalNext;
    }
}

}
