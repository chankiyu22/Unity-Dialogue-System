using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class DialogueOptionsController : MonoBehaviour
{
    private List<DialogueOption> m_dialogueOptions;

    [SerializeField]
    private DialogueOptionsUnityEvent m_OnDialogueOptionsUpdated = null;

    public event EventHandler<DialogueOptionsEventArgs> OnDialogueOptionsUpdated;

    public void SetDialogueOptions(List<DialogueOption> dialogueOptions)
    {
        m_dialogueOptions = dialogueOptions;
        EmitDialogueOptionsUpdated(dialogueOptions);
    }

    public void EmitDialogueOptionsUpdated(List<DialogueOption> dialogueOptions)
    {
        m_OnDialogueOptionsUpdated.Invoke(m_dialogueOptions);

        if (OnDialogueOptionsUpdated != null)
        {
            OnDialogueOptionsUpdated.Invoke(this, new DialogueOptionsEventArgs() {
                dialogueOptions = dialogueOptions
            });
        }
    }
}

}
