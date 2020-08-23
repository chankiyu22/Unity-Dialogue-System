using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class CurrentDialogueController : MonoBehaviour
{
    private DialogueController currentDialogueController
    {
        get
        {
            return DialogueController.currentDialogueController;
        }
    }

    public void ProceedNext()
    {
        if (currentDialogueController == null)
        {
            Debug.LogWarning("No dialogue in progress", this);
            return;
        }
        currentDialogueController.Next();
    }

    public void SelectOption(DialogueOption dialogueOption)
    {
        if (currentDialogueController == null)
        {
            Debug.LogWarning("No dialogue in progress", this);
            return;
        }
        currentDialogueController.SelectOption(dialogueOption);
    }

    public void End()
    {
        if (currentDialogueController == null)
        {
            Debug.LogWarning("No dialogue in progress", this);
            return;
        }
        currentDialogueController.End();
    }
}

}
