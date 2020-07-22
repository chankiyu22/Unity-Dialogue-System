using System;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[Serializable]
public class DialogueNodeNext
{
    // Condition
    [SerializeField]
    private Condition m_condition = null;

    public Condition condition
    {
        get
        {
            return m_condition;
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
