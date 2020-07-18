using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem
{

public enum DialogueNextOption
{
    END = 0,
    DIALOGUE_TEXT = 1,
    DIALOGUE_OPTIONS = 2,
}

[Serializable]
public class DialogueOption
{
    [SerializeField]
    private DialogueOptionText m_dialogueOptionText = null;

    public DialogueOptionText dialogueOptionText
    {
        get
        {
            return m_dialogueOptionText;
        }
    }

    [SerializeField]
    private DialogueText m_next = null;

    public DialogueText next
    {
        get
        {
            return m_next;
        }
    }
}

[Serializable]
public class DialogueNode
{
    [SerializeField]
    private DialogueText m_dialogueText = null;

    public DialogueText dialogueText
    {
        get
        {
            return m_dialogueText;
        }
    }

    [SerializeField]
    private DialogueNextOption m_nextOption = DialogueNextOption.END;

    public DialogueNextOption nextOption
    {
        get
        {
            return m_nextOption;
        }
    }

    // DIALOGUE TEXT
    [SerializeField]
    private DialogueText m_next = null;

    public DialogueText next
    {
        get
        {
            return m_next;
        }
    }


    // DIALOGUE_OPTIONS
    [SerializeField]
    private List<DialogueOption> m_options = null;

    public List<DialogueOption> options
    {
        get
        {
            return m_options;
        }
    }
}

[CreateAssetMenu(menuName="Dialogue System/Dialogue")]
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

            if (dialogueNode.nextOption == DialogueNextOption.DIALOGUE_TEXT)
            {
                if (dialogueNode.next != null)
                {
                    dialogueTextsFromNextText.Add(dialogueNode.next);
                }
            }
            else if (dialogueNode.nextOption == DialogueNextOption.DIALOGUE_OPTIONS)
            {
                foreach (DialogueOption dialogueOption in dialogueNode.options)
                {
                    if (dialogueOption.next != null)
                    {
                        dialogueTextsFromNextText.Add(dialogueOption.next);
                    }
                }
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
