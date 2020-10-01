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

    private Dictionary<Variable, VariableValue> m_variableValues = new Dictionary<Variable, VariableValue>();

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

    [SerializeField]
    private List<DialogueHandler> m_dialogueHandlers = new List<DialogueHandler>();

    public void StartDialogue()
    {
        if (m_currentDialogueController != null)
        {
            Debug.LogWarning("There is a dialogue in progress", m_currentDialogueController);
            return;
        }
        m_currentDialogueController = this;
        m_variableValues.Clear();
        ApplyVariableValues(m_dialogue.dialogueVariables);
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
        m_variableValues.Clear();
        ApplyVariableValues(m_dialogue.dialogueVariables);
        ApplyVariableValues(variableAssignments);
        EmitDialogueBegin(m_dialogue);
    }

    public void Next()
    {
        if (m_currentDialogueNode == null)
        {
            m_currentDialogueNode = m_dialogue.GetBeginNode(m_variableValues);
            if (m_currentDialogueNode == null)
            {
                Debug.LogWarning("No begin dialogue node found", this);
                EndDialogue();
                return;
            }
            EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
        }
        else
        {
            EmitDialogueTextEnd(m_currentDialogueNode.dialogueText);
            ApplyVariableValues(m_currentDialogueNode.assignments);
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
                m_currentDialogueNode = m_dialogue.GetNextDialogueNode(m_currentDialogueNode, m_variableValues);
                if (m_currentDialogueNode == null)
                {
                    Debug.LogWarning("No next dialogue node found", this);
                    EndDialogue();
                }
                else
                {
                    EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
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
        ApplyVariableValues(selectedOption.assignments);

        m_currentDialogueNode = m_dialogue.GetNextDialogueNode(m_currentDialogueNode, m_variableValues);
        if (m_currentDialogueNode == null)
        {
            Debug.LogWarning("No dialogue node found", this);
            EndDialogue();
            return;
        }

        EmitDialogueTextBegin(m_currentDialogueNode.dialogueText);
    }

    void EndDialogue()
    {
        EmitDialogueEnd(m_dialogue);
        m_currentDialogueController = null;
    }

    public void ApplyVariableValues(List<VariableAssignment> assignments)
    {
        foreach (VariableAssignment assignment in assignments)
        {
            Variable variable = assignment.variable;
            if (!m_variableValues.ContainsKey(variable))
            {
                AddVariableValue(assignment);
                continue;
            }
            VariableValue variableValue = m_variableValues[variable];
            switch (variable.GetVariableType())
            {
                case VariableType.INTEGER:
                {
                    variableValue.SetValue(assignment.intValue);
                    break;
                }
                case VariableType.FLOAT:
                {
                    variableValue.SetValue(assignment.floatValue);
                    break;
                }
                case VariableType.BOOLEAN:
                {
                    variableValue.SetValue(assignment.boolValue);
                    break;
                }
                case VariableType.STRING:
                {
                    variableValue.SetValue(assignment.stringValue);
                    break;
                }
            }
        }
    }

    void AddVariableValue(VariableAssignment variableAssignment)
    {
        Variable variable = variableAssignment.variable;
        switch (variable.GetVariableType())
        {
            case VariableType.INTEGER:
            {
                m_variableValues.Add(variable, new VariableValue((IntVariable) variable, variableAssignment.intValue));
                break;
            }
            case VariableType.FLOAT:
            {
                m_variableValues.Add(variable, new VariableValue((FloatVariable) variable, variableAssignment.floatValue));
                break;
            }
            case VariableType.BOOLEAN:
            {
                m_variableValues.Add(variable, new VariableValue((BoolVariable) variable, variableAssignment.boolValue));
                break;
            }
            case VariableType.STRING:
            {
                m_variableValues.Add(variable, new VariableValue((StringVariable) variable, variableAssignment.stringValue));
                break;
            }
        }
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

        foreach (DialogueHandler dialogueHandler in m_dialogueHandlers)
        {
            dialogueHandler.HandleDialogueBegin(dialogue);
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

        foreach (DialogueHandler dialogueHandler in m_dialogueHandlers)
        {
            dialogueHandler.HandleDialogueEnd(dialogue);
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

        foreach (DialogueHandler dialogueHandler in m_dialogueHandlers)
        {
            dialogueHandler.HandleDialogueTextBegin(dialogueText);
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

        foreach (DialogueHandler dialogueHandler in m_dialogueHandlers)
        {
            dialogueHandler.HandleDialogueTextEnd(dialogueText);
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

        foreach (DialogueHandler dialogueHandler in m_dialogueHandlers)
        {
            dialogueHandler.HandleDialogueOptionsBegin(dialogueOptions);
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

        foreach (DialogueHandler dialogueHandler in m_dialogueHandlers)
        {
            dialogueHandler.HandleDialogueOptionsEnd(dialogueOptions);
        }
    }
}

}
