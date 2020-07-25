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
    private DialogueText m_beginText = null;
    public DialogueText beginText
    {
        get
        {
            return m_beginText;
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

        if (beginText != null)
        {
            dialogueTextsFromBeginText.Add(beginText);
        }

        foreach (DialogueNode dialogueNode in dialogueNodes)
        {
            if (dialogueNode.dialogueText != null)
            {
                dialogueTextsFromNode.Add(dialogueNode.dialogueText);
            }

            foreach (DialogueOption dialogueOption in dialogueNode.options)
            {
                if (dialogueOption.next != null)
                {
                    dialogueTextsFromNextText.Add(dialogueOption.next);
                }
            }

            foreach (DialogueNodeNext dialogueNodeNext in dialogueNode.nexts)
            {
                if (dialogueNodeNext.next != null)
                {
                    dialogueTextsFromNextText.Add(dialogueNodeNext.next);
                }
            }
            if (dialogueNode.next != null)
            {
                dialogueTextsFromNextText.Add(dialogueNode.next);
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
