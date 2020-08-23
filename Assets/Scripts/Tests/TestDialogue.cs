using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class TestDialogue
{
    private DialogueController m_dialogueController = null;

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
    }

    [TearDown]
    public void TearDown()
    {
        m_dialogueController.End();
        m_currentDialogue = null;
        m_currentDialogueText = null;
    }

    [Test]
    public void TestLinearDialogue()
    {
        /* Setup */
        Dialogue linearDialogue = ScriptableObject.CreateInstance<Dialogue>();

        DialogueText beginText = ScriptableObject.CreateInstance<DialogueText>();
        Utils.ExecuteSetter(beginText, "m_text", (textProp) => {
            textProp.stringValue = "Begin Text";
        });

        Utils.ExecuteSetter(linearDialogue, "m_finalBeginText", (finalBeginTextProp) => {
            finalBeginTextProp.objectReferenceValue = beginText;
        });
        Utils.ExecuteSetter(linearDialogue, "m_dialogueNodes", (dialogueNodesProp) => {
            DialogueText text1 = ScriptableObject.CreateInstance<DialogueText>();
            Utils.ExecuteSetter(text1, "m_text", (textProp) => textProp.stringValue = "Text 1");

            DialogueText text2 = ScriptableObject.CreateInstance<DialogueText>();
            Utils.ExecuteSetter(text2, "m_text", (textProp) => textProp.stringValue = "Text 2");

            Utils.ExecuteSetterArrayAutoIncrement(dialogueNodesProp, null, (dialogueNodeProp) => {
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
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = text2;
                });
            });
            Utils.ExecuteSetterArrayAutoIncrement(dialogueNodesProp, null, (dialogueNodeProp) => {
                Utils.ExecuteSetter(dialogueNodeProp, "m_dialogueText", (dialogueTextProp) => {
                    dialogueTextProp.objectReferenceValue = text2;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = null;
                });
            });
        });
        /** End setup **/

        Utils.ExecuteSetter(m_dialogueController, "m_dialogue", (dialogueProp) => {
            dialogueProp.objectReferenceValue = linearDialogue;
        });

        Assert.AreEqual(m_currentDialogue, null);

        // begin dialogue
        m_dialogueController.StartDialogue();
        Assert.AreEqual(m_currentDialogue, linearDialogue);

        // begin text
        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Begin Text");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Text 1");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Text 2");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, null);

        // current dialogue has ended
        Assert.AreEqual(m_currentDialogue, null);
    }

    [Test]
    public void TestBranchedDialogue()
    {
        /* Setup */
        Dialogue branchedDialogue = ScriptableObject.CreateInstance<Dialogue>();
        BoolVariable boolVariable1 = ScriptableObject.CreateInstance<BoolVariable>();
        DialogueText beginText = ScriptableObject.CreateInstance<DialogueText>();
        Utils.ExecuteSetter(beginText, "m_text", (textProp) => {
            textProp.stringValue = "Begin Text";
        });
        Utils.ExecuteSetter(branchedDialogue, "m_finalBeginText", (finalBeginTextProp) => {
            finalBeginTextProp.objectReferenceValue = beginText;
        });
        Utils.ExecuteSetter(branchedDialogue, "m_dialogueVariables", (dialogueVariablesProp) => {
            Utils.ExecuteSetterArrayAutoIncrementReset(dialogueVariablesProp, null, (dialogueVariableProp) => {
                Utils.ExecuteSetter(dialogueVariableProp, "m_variable", (variableProp) => {
                    variableProp.objectReferenceValue = boolVariable1;
                });
                Utils.ExecuteSetter(dialogueVariableProp, "m_boolValue", (boolValueProp) => {
                    boolValueProp.boolValue = false;
                });
            });
        });
        Utils.ExecuteSetter(branchedDialogue, "m_dialogueNodes", (dialogueNodesProp) => {
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
                            nextDialogueTextProp.objectReferenceValue = text2_1;
                        });
                    });
                    Utils.ExecuteSetterArrayAutoIncrement(nextsProp, null, (nextProp) => {
                        Utils.ExecuteSetter(nextProp, "m_conditions", (conditionsProp) => {
                            Utils.ExecuteSetterArrayAutoIncrementReset(conditionsProp, null, (conditionProp) => {
                                Utils.ExecuteSetter(conditionProp, "m_variable", (variableProp) => {
                                    variableProp.objectReferenceValue = boolVariable1;
                                });
                                Utils.ExecuteSetter(conditionProp, "m_boolValue", (boolValueProp) => {
                                    boolValueProp.boolValue = false;
                                });
                            });
                        });
                        Utils.ExecuteSetter(nextProp, "m_next", (nextDialogueTextProp) => {
                            nextDialogueTextProp.objectReferenceValue = text2_2;
                        });
                    });
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = null;
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
                Utils.ExecuteSetter(dialogueNodeProp, "m_assignments", (assignmentsProp) => {
                    Utils.ExecuteSetterArrayAutoIncrementReset(assignmentsProp, null, (assignmentProp) => {
                        Utils.ExecuteSetter(assignmentProp, "m_variable", (variableProp) => {
                            variableProp.objectReferenceValue = boolVariable1;
                        });
                        Utils.ExecuteSetter(assignmentProp, "m_boolValue", (boolValueProp) => {
                            boolValueProp.boolValue = true;
                        });
                    });
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_nexts", (nextsProp) => {
                    nextsProp.arraySize = 0;
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_finalNext", (finalNextProp) => {
                    finalNextProp.objectReferenceValue = beginText;
                });
            });
        });
        /** End setup **/

        Utils.ExecuteSetter(m_dialogueController, "m_dialogue", (dialogueProp) => {
            dialogueProp.objectReferenceValue = branchedDialogue;
        });

        Assert.AreEqual(m_currentDialogue, null);

        // begin dialogue
        m_dialogueController.StartDialogue();
        Assert.AreEqual(m_currentDialogue, branchedDialogue);

        // begin text
        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Begin Text");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Text 1");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Text 2.2");

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

}
