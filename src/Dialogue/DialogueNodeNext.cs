using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[Serializable]
public class DialogueNodeNext
{
    // Condition
    [SerializeField]
    private List<Condition> m_conditions = new List<Condition>();

    public List<Condition> conditions
    {
        get
        {
            return m_conditions;
        }
    }

    [SerializeField]
    private DialogueText m_next = null;

    public DialogueText next
    {
        get
        {
            return m_next;
        }
    }
}

}
