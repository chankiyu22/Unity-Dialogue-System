using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class TestStartDialogue
{
    private DialogueController m_dialogueController = null;
    private Dialogue m_dialogue = null;

    private VariableAssignmentsController m_variableAssignmentsController = null;

    /** Result **/
    private Dialogue m_currentDialogue = null;
    private string m_currentDialogueText = null;

    [OneTimeSetUp]
    public void Setup()
    {
        GameObject gameObject = new GameObject("Dialogue Controller Object");
        m_dialogueController = gameObject.AddComponent<DialogueController>();
        m_dialogueController.OnDialogueBegin += (sender, args) => {
            m_currentDialogue = args.dialogue;
        };
        m_dialogueController.OnDialogueEnd += (sender, args) => {
            m_currentDialogueText = null;
            m_currentDialogue = null;
        };
        m_dialogueController.OnDialogueTextBegin += (sender, args) => {
            m_currentDialogueText = args.dialogueText.text;
        };
        m_dialogueController.OnDialogueTextEnd += (sender, args) => {
            m_currentDialogueText = null;
        };

        /* Setup */
        Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
        m_dialogue = dialogue;
        BoolVariable boolVariable1 = ScriptableObject.CreateInstance<BoolVariable>();
        DialogueText beginText = ScriptableObject.CreateInstance<DialogueText>();
        Utils.ExecuteSetter(beginText, "m_text", (textProp) => {
            textProp.stringValue = "Begin Text";
        });
        Utils.ExecuteSetter(dialogue, "m_finalBeginText", (finalBeginTextProp) => {
            finalBeginTextProp.objectReferenceValue = beginText;
        });
        Utils.ExecuteSetter(dialogue, "m_dialogueVariables", (dialogueVariablesProp) => {
            Utils.ExecuteSetterArrayAutoIncrementReset(dialogueVariablesProp, null, (dialogueVariableProp) => {
                Utils.ExecuteSetter(dialogueVariableProp, "m_variable", (variableProp) => {
                    variableProp.objectReferenceValue = boolVariable1;
                });
                Utils.ExecuteSetter(dialogueVariableProp, "m_boolValue", (boolValueProp) => {
                    boolValueProp.boolValue = false;
                });
            });
        });
        Utils.ExecuteSetter(dialogue, "m_dialogueNodes", (dialogueNodesProp) => {
            DialogueText text1 = ScriptableObject.CreateInstance<DialogueText>();
            Utils.ExecuteSetter(text1, "m_text", (textProp) => textProp.stringValue = "Text 1");
            DialogueText text2_1 = ScriptableObject.CreateInstance<DialogueText>();
            Utils.ExecuteSetter(text2_1, "m_text", (textProp) => textProp.stringValue = "Text 2.1");
            DialogueText text2_2 = ScriptableObject.CreateInstance<DialogueText>();
            Utils.ExecuteSetter(text2_2, "m_text", (textProp) => textProp.stringValue = "Text 2.2");

            Utils.ExecuteSetterArrayAutoIncrementReset(dialogueNodesProp, null, (dialogueNodeProp) => {
                Utils.ExecuteSetter(dialogueNodeProp, "m_dialogueText", (dialogueTextProp) => {
                    dialogueTextProp.objectReferenceValue = beginText;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = text1;
                });
            });
            Utils.ExecuteSetterArrayAutoIncrement(dialogueNodesProp, null, (dialogueNodeProp) => {
                Utils.ExecuteSetter(dialogueNodeProp, "m_dialogueText", (dialogueTextProp) => {
                    dialogueTextProp.objectReferenceValue = text1;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_nexts", (nextsProp) => {
                    Utils.ExecuteSetterArrayAutoIncrementReset(nextsProp, null, (nextProp) => {
                        Utils.ExecuteSetter(nextProp, "m_conditions", (conditionsProp) => {
                            Utils.ExecuteSetterArrayAutoIncrementReset(conditionsProp, null, (conditionProp) => {
                                Utils.ExecuteSetter(conditionProp, "m_variable", (variableProp) => {
                                    variableProp.objectReferenceValue = boolVariable1;
                                });
                                Utils.ExecuteSetter(conditionProp, "m_boolValue", (boolValueProp) => {
                                    boolValueProp.boolValue = true;
                                });
                            });
                        });
                        Utils.ExecuteSetter(nextProp, "m_next", (nextDialogueTextProp) => {
                            nextDialogueTextProp.objectReferenceValue = text2_2;
                        });
                    });
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = text2_1;
                });
            });
            Utils.ExecuteSetterArrayAutoIncrement(dialogueNodesProp, null, (dialogueNodeProp) => {
                Utils.ExecuteSetter(dialogueNodeProp, "m_dialogueText", (dialogueTextProp) => {
                    dialogueTextProp.objectReferenceValue = text2_1;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_nexts", (nextsProp) => {
                    nextsProp.arraySize = 0;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = null;
                });
            });
            Utils.ExecuteSetterArrayAutoIncrement(dialogueNodesProp, null, (dialogueNodeProp) => {
                Utils.ExecuteSetter(dialogueNodeProp, "m_dialogueText", (dialogueTextProp) => {
                    dialogueTextProp.objectReferenceValue = text2_2;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_nexts", (nextsProp) => {
                    nextsProp.arraySize = 0;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = null;
                });
            });
        });
        /** End setup **/

        Utils.ExecuteSetter(m_dialogueController, "m_dialogue", (dialogueProp) => {
            dialogueProp.objectReferenceValue = dialogue;
        });

        m_variableAssignmentsController = gameObject.AddComponent<VariableAssignmentsStaticController>();
        Utils.ExecuteSetter(m_variableAssignmentsController, "m_variableAssignments", (variableAssignmentsProp) => {
            Utils.ExecuteSetterArrayAutoIncrementReset(variableAssignmentsProp, null, (variableAssignmentProp) => {
                Utils.ExecuteSetter(variableAssignmentProp, "m_variable", (variableProp) => {
                    variableProp.objectReferenceValue = boolVariable1;
                });
                Utils.ExecuteSetter(variableAssignmentProp, "m_boolValue", (boolValueProp) => {
                    boolValueProp.boolValue = true;
                });
            });
        });
    }

    [TearDown]
    public void TearDown()
    {
        m_dialogueController.End();
        m_currentDialogue = null;
        m_currentDialogueText = null;
    }

    [Test]
    public void Test_DialogueController_StartDialogue()
    {
        Assert.AreEqual(m_currentDialogue, null);

        for (int i = 0; i < 3; i++)
        {
            m_dialogueController.StartDialogue();
            Assert.AreEqual(m_currentDialogue, m_dialogue);

            // begin text
            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, "Begin Text");

            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, "Text 1");

            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, "Text 2.1");

            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, null);

            // current dialogue has ended
            Assert.AreEqual(m_currentDialogue, null);
        }
    }

    [Test]
    public void Test_DialogueController_StartDialogueWithInitialVariables_VariableAssignmentsStaticController()
    {
        Assert.AreEqual(m_currentDialogue, null);

        for (int i = 0; i < 3; i++)
        {
            m_dialogueController.StartDialogueWithInitialVariables(m_variableAssignmentsController);
            Assert.AreEqual(m_currentDialogue, m_dialogue);

            // begin text
            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, "Begin Text");

            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, "Text 1");

            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, "Text 2.2");

            m_dialogueController.Next();
            Assert.AreEqual(m_currentDialogueText, null);

            // current dialogue has ended
            Assert.AreEqual(m_currentDialogue, null);
        }
    }
}

}
