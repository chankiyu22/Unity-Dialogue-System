using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

[CreateAssetMenu(menuName="Dialogue System/Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{

    [SerializeField]
    private List<DialogueNodeNext> m_beginTexts = new List<DialogueNodeNext>();

    public List<DialogueNodeNext> beginTexts
    {
        get
        {
            return m_beginTexts;
        }
    }

    [SerializeField]
    private DialogueText m_finalBeginText = null;
    public DialogueText finalBeginText
    {
        get
        {
            return m_finalBeginText;
        }
    }

    [SerializeField]
    private List<DialogueNode> m_dialogueNodes = new List<DialogueNode>();

    public List<DialogueNode> dialogueNodes
    {
        get
        {
            return m_dialogueNodes;
        }
    }

    [SerializeField]
    private List<VariableAssignment> m_dialogueVariables = new List<VariableAssignment>();

    public List<VariableAssignment> dialogueVariables
    {
        get
        {
            return m_dialogueVariables;
        }
    }

    private Dictionary<Variable, VariableValue> m_variableValues = new Dictionary<Variable, VariableValue>();

    public Dictionary<Variable, VariableValue> variableValues
    {
        get
        {
            return m_variableValues;
        }
    }

    public void InitializeVariableValues()
    {
        m_variableValues.Clear();
        ApplyVariableValues(m_dialogueVariables);
    }

    public DialogueText GetBeginText()
    {
        foreach (DialogueNodeNext dialogueNodeNext in m_beginTexts)
        {
            if (dialogueNodeNext.EvaluateCondition(variableValues))
            {
                return dialogueNodeNext.next;
            }
        }
        return m_finalBeginText;
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

    public (List<DialogueText> undefinedDialogueTexts, List<DialogueText> unusedDialoguedTexts) GetUnreferencedDialoguedTextAndUnusedNodes()
    {
        List<DialogueText> dialogueTextsFromNode = new List<DialogueText>();
        List<DialogueText> dialogueTextsFromBeginText = new List<DialogueText>();
        List<DialogueText> dialogueTextsFromNextText = new List<DialogueText>();
        List<DialogueText> dialogueTextsFromOptionNextText =new List<DialogueText>();

        foreach (DialogueNodeNext beginTextNext in m_beginTexts)
        {
            if (beginTextNext.next != null)
            {
                dialogueTextsFromBeginText.Add(beginTextNext.next);
            }
        }

        if (m_finalBeginText != null)
        {
            dialogueTextsFromBeginText.Add(m_finalBeginText);
        }

        foreach (DialogueNode dialogueNode in dialogueNodes)
        {
            if (dialogueNode.dialogueText != null)
            {
                dialogueTextsFromNode.Add(dialogueNode.dialogueText);
            }

            foreach (DialogueNodeNext dialogueNodeNext in dialogueNode.nexts)
            {
                if (dialogueNodeNext.next != null)
                {
                    dialogueTextsFromNextText.Add(dialogueNodeNext.next);
                }
            }

            if (dialogueNode.finalNext != null)
            {
                dialogueTextsFromNextText.Add(dialogueNode.finalNext);
            }
        }

        IEnumerable<DialogueText> declaredDialogueTexts = dialogueTextsFromBeginText
            .Concat(dialogueTextsFromNextText)
            .Concat(dialogueTextsFromOptionNextText);

        List<DialogueText> undefinedDialogueTexts = declaredDialogueTexts.Except(dialogueTextsFromNode).ToList();

        IEnumerable<DialogueNode> unusedDialoguedNodes = dialogueNodes
            .Where((DialogueNode d) => declaredDialogueTexts.Where((DialogueText t) => t == d.dialogueText).Count() == 0);

        List<DialogueText> unusedDialoguedTexts = new List<DialogueText>();

        foreach (DialogueNode dialogueNode in unusedDialoguedNodes)
        {
            if (dialogueNode.dialogueText != null)
            {
                unusedDialoguedTexts.Add(dialogueNode.dialogueText);
            }
        }

        return (undefinedDialogueTexts, unusedDialoguedTexts);
    }

    public List<DialogueText> GetDialogueTexts()
    {
        List<DialogueText> results = new List<DialogueText>();
        foreach (DialogueNode dialogueNode in m_dialogueNodes)
        {
            if (dialogueNode.dialogueText != null && !results.Contains(dialogueNode.dialogueText))
            {
                results.Add(dialogueNode.dialogueText);
            }
        }
        return results;
    }
}

}
