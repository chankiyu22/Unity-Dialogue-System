using System;
using UnityEngine;
using UnityEngine.Events;

namespace Chankiyu22.DialogueSystem
{

public class DialogueTextController : MonoBehaviour
{
    [SerializeField]
    private StringUnityEvent m_OnHandleDialogueText = null;

    public event EventHandler<StringEventArgs> OnHandleDialogueText;

    [SerializeField]
    private UnityEvent m_OnResetText = null;

    public event EventHandler OnResetText;

    public void HandleDialogueText(DialogueText dialogueText)
    {
        EmitHandleDialogueText(dialogueText.text);
    }

    void EmitHandleDialogueText(string text)
    {
        m_OnHandleDialogueText.Invoke(text);

        if (OnHandleDialogueText != null)
        {
            OnHandleDialogueText.Invoke(this, new StringEventArgs() {
                value = text
            });
        }
    }

    public void ResetText()
    {
        m_OnResetText.Invoke();

        if (OnResetText != null)
        {
            OnResetText.Invoke(this, EventArgs.Empty);
        }
    }
}

}
