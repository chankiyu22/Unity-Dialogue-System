using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class TestSuite
{
    private CurrentDialogueController m_currentDialogueController = null;
    private DialogueController m_dialogueController = null;
    private Dialogue m_dialogue = null;

    private SerializedObject m_dialogueObject = null;

    private DialogueText m_currentDialogueText = null;

    [OneTimeSetUp]
    public void Setup()
    {
        GameObject gameObject = new GameObject("Dialogue Controller Object");
        m_currentDialogueController = gameObject.AddComponent<CurrentDialogueController>();
        m_dialogueController = gameObject.AddComponent<DialogueController>();
        m_dialogueController.OnDialogueBegin += OnDialogueBegin;
        m_dialogueController.OnDialogueTextBegin += OnDialogueTextBegin;

        m_dialogue = ScriptableObject.CreateInstance<Dialogue>();
        m_dialogueController.dialogue = m_dialogue;
        m_dialogueObject = new SerializedObject(m_dialogue);

        DialogueText beginText = ScriptableObject.CreateInstance<DialogueText>();
        beginText.text = "Begin Text";
        m_dialogueObject.FindProperty("m_finalBeginText").objectReferenceValue = beginText;
        SerializedProperty dialogueNodesProp = m_dialogueObject.FindProperty("m_dialogueNodes");
        dialogueNodesProp.arraySize = 1;
        dialogueNodesProp.FindPropertyRelative("Array.data[0].m_dialogueText").objectReferenceValue = beginText;

        m_dialogueObject.ApplyModifiedProperties();
    }

    void OnDialogueBegin(object sender, DialogueEventArgs args)
    {
    }

    void OnDialogueTextBegin(object sender, DialogueTextEventArgs args)
    {
        m_currentDialogueText = args.dialogueText;
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestSuiteSimplePasses()
    {
        Assert.AreEqual(DialogueController.currentDialogueController, null);

        // begin dialogue
        m_dialogueController.StartDialogue();
        Assert.AreEqual(DialogueController.currentDialogueController, m_dialogueController);

        // begin text
        m_dialogueController.StartText();
        Assert.AreEqual(m_currentDialogueText, m_dialogue.finalBeginText);

        m_currentDialogueController.ProceedNext();

        // current dialogue has ended
        Assert.AreEqual(DialogueController.currentDialogueController, null);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestSuiteWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

}
