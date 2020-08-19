using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
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
    private DialogueUnityEvent m_OnDialogueBegin = new DialogueUnityEvent();

    public event EventHandler<DialogueEventArgs> OnDialogueBegin;

    [SerializeField]
    private DialogueUnityEvent m_OnDialogueEnd = new DialogueUnityEvent();

    public event EventHandler<DialogueEventArgs> OnDialogueEnd;

    [SerializeField]
    private DialogueTextUnityEvent m_OnDialogueTextBegin = new DialogueTextUnityEvent();

    public event EventHandler<DialogueTextEventArgs> OnDialogueTextBegin;

    [SerializeField]
    private DialogueTextUnityEvent m_OnDialogueTextEnd = new DialogueTextUnityEvent();

    public event EventHandler<DialogueTextEventArgs> OnDialogueTextEnd;

    [SerializeField]
    private DialogueOptionsUnityEvent m_OnDialogueOptionsBegin = new DialogueOptionsUnityEvent();

    public event EventHandler<DialogueOptionsEventArgs> OnDialogueOptionsBegin;

    [SerializeField]
    private DialogueOptionsUnityEvent m_OnDialogueOptionsEnd = new DialogueOptionsUnityEvent();

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
        m_dialogue.InitializeVariableValues();
        EmitDialogueBegin(m_dialogue);
    }

    public void StartDialogueWithInitialVariables(VariableAssignmentsController variableAssignmentsController)
    {
        if (m_currentDialogueController != null)
        {
            Debug.LogWarning("There is a dialogue in progress", m_currentDialogueController);
            return;
        }
        List<VariableAssignment> variableAssignments = variableAssignmentsController.GetVariableAssignments();
        m_currentDialogueController = this;
        m_dialogue.InitializeVariableValues();
        m_dialogue.ApplyVariableValues(variableAssignments);
        EmitDialogueBegin(m_dialogue);
    }

    // TODO: Initialize a DialogueNode field with dialogue.beginText.
    // TODO: Handle onDialogueTextBegin and onDialogueTextEnd correctly.
    //       Add method StartText and EndText to emit text begin and end events.
    public void StartText()
    {
        DialogueText beginText = m_dialogue.GetBeginText();
        if (beginText == null)
        {
            Debug.LogWarning("No begin dialogue text found", this);
            EndDialogue();
            return;
        }
        m_currentDialogueNode = m_dialogue.dialogueNodes.Find((DialogueNode d) => d.dialogueText == beginText);
        if (m_currentDialogueNode == null)
        {
            Debug.LogWarning("No begin dialogue text found", this);
            EndDialogue();
            return;
        }
        EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
    }

    public void Next()
    {
        if (m_currentDialogueNode != null)
        {
            EmitDialogueTextEnd(m_currentDialogueNode.dialogueText);
            m_dialogue.ApplyVariableValues(m_currentDialogueNode.assignments);
            if (m_currentDialogueNode.options.Count > 0)
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
            }
            else
            {
                DialogueText nextDialogueText = m_currentDialogueNode.GetNextDialogueText(m_dialogue.variableValues);
                if (nextDialogueText == null)
                {
                    Debug.LogWarning("No next dialogue text evaluated valid", this);
                    EndDialogue();
                }
                else
                {
                    m_currentDialogueNode = m_dialogue.dialogueNodes.Find((DialogueNode d) => d.dialogueText == nextDialogueText);
                    if (m_currentDialogueNode == null)
                    {
                        Debug.LogWarning("Next is dialogue text but no dialogue text found", this);
                        EndDialogue();
                    }
                    else
                    {
                        EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
                    }
                }
            }
        }
    }

    public void End()
    {
        EndDialogue();
    }

    public void SelectOption(DialogueOption dialogueOption)
    {
        if (m_currentDialogueNode == null)
        {
            Debug.LogWarning("No dialogue in progress", this);
            return;
        }

        if (m_currentDialogueNode.options.Count == 0)
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

        EmitDialogueOptionsEnd(dialogueOptions);
        m_dialogue.ApplyVariableValues(selectedOption.assignments);

        DialogueText nextDialogueText = m_currentDialogueNode.GetNextDialogueText(m_dialogue.variableValues);
        if (nextDialogueText == null)
        {
            Debug.LogWarning("No next dialogue text evaluated valid", this);
            EndDialogue();
        }

        DialogueNode dialogueNode = m_dialogue.dialogueNodes.Find((DialogueNode d) => d.dialogueText == nextDialogueText);

        if (dialogueNode == null)
        {
            Debug.LogWarning("Next option is not found", this);
            return;
        }

        m_currentDialogueNode = dialogueNode;
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
