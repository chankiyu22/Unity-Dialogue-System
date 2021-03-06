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

    public DialogueNode GetBeginNode(Dictionary<Variable, VariableValue> variableValueMap)
    {
        Dictionary<DialogueText, DialogueNode> dialogueTextNodeMap = new Dictionary<DialogueText, DialogueNode>();
        foreach (DialogueNode dialogueNode in m_dialogueNodes)
        {
            dialogueTextNodeMap.Add(dialogueNode.dialogueText, dialogueNode);
        }

        foreach (DialogueNodeNext dialogueNodeNext in m_beginTexts)
        {
            if (dialogueNodeNext.EvaluateCondition(variableValueMap))
            {
                if (dialogueNodeNext.next != null && dialogueTextNodeMap.ContainsKey(dialogueNodeNext.next))
                {
                    return dialogueTextNodeMap[dialogueNodeNext.next];
                }
                return null;
            }
        }

        if (m_finalBeginText != null && dialogueTextNodeMap.ContainsKey(m_finalBeginText))
        {
            return dialogueTextNodeMap[m_finalBeginText];
        }
        return null;
    }

    public DialogueNode GetNextDialogueNode(DialogueNode currentDialogueNode, Dictionary<Variable, VariableValue> variableValueMap)
    {
        Dictionary<DialogueText, DialogueNode> dialogueTextNodeMap = new Dictionary<DialogueText, DialogueNode>();
        foreach (DialogueNode dialogueNode in m_dialogueNodes)
        {
            dialogueTextNodeMap.Add(dialogueNode.dialogueText, dialogueNode);
        }

        foreach (DialogueNodeNext dialogueNodeNext in currentDialogueNode.nexts)
        {
            if (dialogueNodeNext.EvaluateCondition(variableValueMap))
            {
                if (dialogueNodeNext.next != null && dialogueTextNodeMap.ContainsKey(dialogueNodeNext.next))
                {
                    return dialogueTextNodeMap[dialogueNodeNext.next];
                }
                return null;
            }
        }

        if (currentDialogueNode.finalNext != null && dialogueTextNodeMap.ContainsKey(currentDialogueNode.finalNext))
        {
            return dialogueTextNodeMap[currentDialogueNode.finalNext];
        }
        return null;
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
