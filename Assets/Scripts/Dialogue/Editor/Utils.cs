using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class Utils
{
    public static string GetDirectoryForSelectedProjectObject()
    {
        Object selectedObject = Selection.activeObject;
        if (selectedObject != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedObject.GetInstanceID());
            if (path.Length == 0)
            {
                return "";
            }
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                return Path.GetDirectoryName(path);
            }
        }

        return "";
    }

    public static string GetCurrentSceneDirectory()
    {
        Scene activeScene = EditorSceneManager.GetActiveScene();
        return Path.GetDirectoryName(activeScene.path);
    }

    public static string GetActiveDirectory()
    {
        string directory = GetDirectoryForSelectedProjectObject();
        if (directory.Length != 0)
        {
            return directory;
        }
        return GetCurrentSceneDirectory();
    }

    public static (List<DialogueText> undefinedDialogueTexts, List<DialogueText> unusedDialoguedTexts) GetUnreferencedDialogueTextAndUnusedNodes(Dialogue dialogue)
    {
        List<DialogueText> dialogueTextsFromNode = new List<DialogueText>();
        List<DialogueText> dialogueTextsFromBeginText = new List<DialogueText>();
        List<DialogueText> dialogueTextsFromNextText = new List<DialogueText>();
        List<DialogueText> dialogueTextsFromOptionNextText =new List<DialogueText>();

        List<DialogueNodeNext> beginTexts = dialogue.beginTexts;
        foreach (DialogueNodeNext beginTextNext in beginTexts)
        {
            if (beginTextNext.next != null)
            {
                dialogueTextsFromBeginText.Add(beginTextNext.next);
            }
        }

        DialogueText finalBeginText = dialogue.finalBeginText;

        if (finalBeginText != null)
        {
            dialogueTextsFromBeginText.Add(finalBeginText);
        }

        List<DialogueNode> dialogueNodes = dialogue.dialogueNodes;

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
}

}

