using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem
{

[Serializable]
class DialogueTextToUnityEvent
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
    private DialogueTextUnityEvent m_actions = null;

    public DialogueTextUnityEvent actions
    {
        get
        {
            return m_actions;
        }
    }
}

public class DialogueTextDispatcher : MonoBehaviour
{
    [SerializeField]
    private List<DialogueTextToUnityEvent> m_dialogueTexts = new List<DialogueTextToUnityEvent>();

    private Dictionary<DialogueText, DialogueTextToUnityEvent> m_map = new Dictionary<DialogueText, DialogueTextToUnityEvent>();

    void Awake()
    {
        foreach (DialogueTextToUnityEvent d in m_dialogueTexts)
        {
            m_map.Add(d.dialogueText, d);
        }
    }

    public void Dispatch(DialogueText dialogueText)
    {
        if (m_map.ContainsKey(dialogueText))
        {
            DialogueTextToUnityEvent d = m_map[dialogueText];
            d.actions.Invoke(dialogueText);
        }
    }
}

}
