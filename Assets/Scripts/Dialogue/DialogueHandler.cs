using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public abstract class DialogueHandler : MonoBehaviour
{
    public virtual void HandleDialogueBegin(Dialogue dialogue) {}
    public virtual void HandleDialogueEnd(Dialogue dialogue) {}

    public virtual void HandleDialogueTextBegin(DialogueText dialogueText) {}
    public virtual void HandleDialogueTextEnd(DialogueText dialogueText) {}

    public virtual void HandleDialogueOptionsBegin(List<DialogueOption> dialogueOptions) {}
    public virtual void HandleDialogueOptionsEnd(List<DialogueOption> dialogueOptions) {}
}

}
