using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[Serializable]
public class DialogueOption
{
    [SerializeField]
    private DialogueOptionText m_dialogueOptionText = null;

    public DialogueOptionText dialogueOptionText
    {
        get
        {
            return m_dialogueOptionText;
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
}

}
