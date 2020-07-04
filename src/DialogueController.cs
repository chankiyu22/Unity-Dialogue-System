using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem
{

public class DialogueController : MonoBehaviour
{
    private static DialogueController m_currentDialogueController = null;

    public static DialogueController currentDialogueController
    {
        get
        {
            return m_currentDialogueController;
        }
    }

    [SerializeField]
    private Dialogue m_dialogue = null;

    public Dialogue dialogue
    {
        get
        {
            return m_dialogue;
        }

        set
        {
            m_dialogue = value;
        }
    }

    [SerializeField]
    private DialogueUnityEvent m_OnDialogueBegin = null;

    public event EventHandler<DialogueEventArgs> OnDialogueBegin;

    [SerializeField]
    private DialogueUnityEvent m_OnDialogueEnd = null;

    public event EventHandler<DialogueEventArgs> OnDialogueEnd;

    [SerializeField]
    private DialogueTextUnityEvent m_OnDialogueTextBegin = null;

    public event EventHandler<DialogueTextEventArgs> OnDialogueTextBegin;

    [SerializeField]
    private DialogueTextUnityEvent m_OnDialogueTextEnd = null;

    public event EventHandler<DialogueTextEventArgs> OnDialogueTextEnd;

    [SerializeField]
    private DialogueOptionsUnityEvent m_OnDialogueOptionsBegin = null;

    public event EventHandler<DialogueOptionsEventArgs> OnDialogueOptionsBegin;

    [SerializeField]
    private DialogueOptionsUnityEvent m_OnDialogueOptionsEnd = null;

    public event EventHandler<DialogueOptionsEventArgs> OnDialogueOptionsEnd;

    private DialogueNode m_currentDialogueNode = null;

    public void StartDialogue()
    {
        if (m_currentDialogueController != null)
        {
            Debug.LogWarning("There is a dialogue in progress", m_currentDialogueController);
            return;
        }
        m_currentDialogueController = this;
        EmitDialogueBegin(m_dialogue);
    }

    // TODO: Initialize a DialogueNode field with dialogue.beginText.
    // TODO: Handle onDialogueTextBegin and onDialogueTextEnd correctly.
    //       Add method StartText and EndText to emit text begin and end events.
    public void StartText()
    {
        m_currentDialogueNode = m_dialogue.dialogueNodes.Find((DialogueNode d) => d.dialogueText == m_dialogue.beginText);
        if (m_currentDialogueNode != null)
        {
            EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
        }
        else
        {
            Debug.LogWarning("No begin dialogue text found", this);
            EndDialogue();
        }
    }

    public void Next()
    {
        if (m_currentDialogueNode != null)
        {
            EmitDialogueTextEnd(m_currentDialogueNode.dialogueText);
            switch (m_currentDialogueNode.nextOption) {
                case DialogueNextOption.END:
                {
                    m_currentDialogueNode = null;
                    EndDialogue();
                    break;
                }
                case DialogueNextOption.DIALOGUE_TEXT:
                {
                    m_currentDialogueNode = m_dialogue.dialogueNodes.Find((DialogueNode d) => d.dialogueText == m_currentDialogueNode.next);
                    if (m_currentDialogueNode == null)
                    {
                        Debug.LogWarning("Next is dialogue text but no dialogue text found", this);
                        EndDialogue();
                    }
                    else
                    {
                        EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
                    }
                    break;
                }
                case DialogueNextOption.DIALOGUE_OPTIONS:
                {
                    List<DialogueOption> dialogueOptions = m_currentDialogueNode.options;
                    if (dialogueOptions.Count == 0)
                    {
                        Debug.LogWarning("Next is options but no options found", this);
                        EndDialogue();
                    }
                    else
                    {
                        EmitDialogueOptionsBegin(dialogueOptions);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }

    public void SelectOption(DialogueOption dialogueOption)
    {
        if (m_currentDialogueNode == null)
        {
            Debug.LogWarning("No dialogue in progress", this);
            return;
        }

        if (m_currentDialogueNode.nextOption != DialogueNextOption.DIALOGUE_OPTIONS)
        {
            Debug.LogWarning("The current dialogue is not proceeding to options", this);
            return;
        }

        List<DialogueOption> dialogueOptions = m_currentDialogueNode.options;

        DialogueOption selectedOption = dialogueOptions.Find((DialogueOption d) => d == dialogueOption);

        if (selectedOption == null)
        {
            Debug.LogWarning("Desire dialogue option is not in options of current dialogue node", this);
            return;
        }

        DialogueNode dialogueNode = m_dialogue.dialogueNodes.Find((DialogueNode d) => d.dialogueText == selectedOption.next);

        if (dialogueNode == null)
        {
            Debug.LogWarning("Next option is not found", this);
            return;
        }

        m_currentDialogueNode = dialogueNode;
        EmitDialogueOptionsEnd(dialogueOptions);
        EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
    }

    void EndDialogue()
    {
        EmitDialogueEnd(m_dialogue);
        m_currentDialogueController = null;
    }

    void EmitDialogueBegin(Dialogue dialogue)
    {
        m_OnDialogueBegin.Invoke(dialogue);

        if (OnDialogueBegin != null)
        {
            OnDialogueBegin.Invoke(this, new DialogueEventArgs() {
                dialogue = dialogue
            });
        }
    }

    void EmitDialogueEnd(Dialogue dialogue)
    {
        m_OnDialogueEnd.Invoke(dialogue);

        if (OnDialogueEnd != null)
        {
            OnDialogueEnd.Invoke(this, new DialogueEventArgs() {
                dialogue = dialogue
            });
        }
    }

    void EmitDialogueTextBegin(DialogueText dialogueText)
    {
        m_OnDialogueTextBegin.Invoke(dialogueText);

        if (OnDialogueTextBegin != null)
        {
            OnDialogueTextBegin.Invoke(this, new DialogueTextEventArgs() {
                dialogueText = dialogueText
            });
        }
    }

    void EmitDialogueTextEnd(DialogueText dialogueText)
    {
        m_OnDialogueTextEnd.Invoke(dialogueText);

        if (OnDialogueTextEnd != null)
        {
            OnDialogueTextEnd.Invoke(this, new DialogueTextEventArgs() {
                dialogueText = dialogueText
            });
        }
    }

    void EmitDialogueOptionsBegin(List<DialogueOption> dialogueOptions)
    {
        m_OnDialogueOptionsBegin.Invoke(dialogueOptions);

        if (OnDialogueOptionsBegin != null)
        {
            OnDialogueOptionsBegin.Invoke(this, new DialogueOptionsEventArgs() {
                dialogueOptions = dialogueOptions
            });
        }
    }

    void EmitDialogueOptionsEnd(List<DialogueOption> dialogueOptions)
    {
        m_OnDialogueOptionsEnd.Invoke(dialogueOptions);

        if (OnDialogueOptionsEnd != null)
        {
            OnDialogueOptionsEnd.Invoke(this, new DialogueOptionsEventArgs() {
                dialogueOptions = dialogueOptions
            });
        }
    }
}

}
