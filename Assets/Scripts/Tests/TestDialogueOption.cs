using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class TestDialogueOption
{
    private DialogueController m_dialogueController = null;
    private Dialogue m_dialogue = null;

    /** Result **/
    private Dialogue m_currentDialogue = null;
    private string m_currentDialogueText = null;
    private List<DialogueOption> m_currentDialogueOptions = null;

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
            m_currentDialogueOptions = null;
        };
        m_dialogueController.OnDialogueTextBegin += (sender, args) => {
            m_currentDialogueText = args.dialogueText.text;
        };
        m_dialogueController.OnDialogueTextEnd += (sender, args) => {
            m_currentDialogueText = null;
        };
        m_dialogueController.OnDialogueOptionsBegin += (sender, args) => {
            m_currentDialogueOptions = args.dialogueOptions;
        };
        m_dialogueController.OnDialogueOptionsEnd += (sender, args) => {
            m_currentDialogueOptions = null;
        };

        /* Setup */
        Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
        m_dialogue = dialogue;
        IntVariable intVariable1 = ScriptableObject.CreateInstance<IntVariable>();
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
                    variableProp.objectReferenceValue = intVariable1;
                });
                Utils.ExecuteSetter(dialogueVariableProp, "m_intValue", (intValueProp) => {
                    intValueProp.intValue = 0;
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
                Utils.ExecuteSetter(dialogueNodeProp, "m_options", (dialogueNodeOptionsProp) => {
                    // Option 1
                    Utils.ExecuteSetterArrayAutoIncrementReset(dialogueNodeOptionsProp, null, (dialogueNodeOptionProp) => {
                        Utils.ExecuteSetter(dialogueNodeOptionProp, "m_dialogueOptionText", (dialogueOptionTextProp) => {
                            dialogueOptionTextProp.objectReferenceValue = ScriptableObject.CreateInstance<DialogueOptionText>();
                            Utils.ExecuteSetter(dialogueOptionTextProp.objectReferenceValue, "m_text", (textProp) => {
                                textProp.stringValue = "Option 1";
                            });
                        });
                        Utils.ExecuteSetter(dialogueNodeOptionProp, "m_assignments", (assignmentsProp) => {
                            Utils.ExecuteSetterArrayAutoIncrementReset(assignmentsProp, null, (assignmentProp) => {
                                Utils.ExecuteSetter(assignmentProp, "m_variable", (variableProp) => {
                                    variableProp.objectReferenceValue = intVariable1;
                                });
                                Utils.ExecuteSetter(assignmentProp, "m_intValue", (intValueProp) => {
                                    intValueProp.intValue = 1;
                                });
                            });
                        });
                    });
                    // Option 2
                    Utils.ExecuteSetterArrayAutoIncrement(dialogueNodeOptionsProp, null, (dialogueNodeOptionProp) => {
                        Utils.ExecuteSetter(dialogueNodeOptionProp, "m_dialogueOptionText", (dialogueOptionTextProp) => {
                            dialogueOptionTextProp.objectReferenceValue = ScriptableObject.CreateInstance<DialogueOptionText>();
                            Utils.ExecuteSetter(dialogueOptionTextProp.objectReferenceValue, "m_text", (textProp) => {
                                textProp.stringValue = "Option 2";
                            });
                        });
                        Utils.ExecuteSetter(dialogueNodeOptionProp, "m_assignments", (assignmentsProp) => {
                            Utils.ExecuteSetterArrayAutoIncrementReset(assignmentsProp, null, (assignmentProp) => {
                                Utils.ExecuteSetter(assignmentProp, "m_variable", (variableProp) => {
                                    variableProp.objectReferenceValue = intVariable1;
                                });
                                Utils.ExecuteSetter(assignmentProp, "m_intValue", (intValueProp) => {
                                    intValueProp.intValue = 2;
                                });
                            });
                        });
                    });
                });
                Utils.ExecuteSetter(dialogueNodeProp, "m_nexts", (nextsProp) => {
                    Utils.ExecuteSetterArrayAutoIncrementReset(nextsProp, null, (nextProp) => {
                        Utils.ExecuteSetter(nextProp, "m_conditions", (conditionsProp) => {
                            Utils.ExecuteSetterArrayAutoIncrementReset(conditionsProp, null, (conditionProp) => {
                                Utils.ExecuteSetter(conditionProp, "m_variable", (variableProp) => {
                                    variableProp.objectReferenceValue = intVariable1;
                                });
                                Utils.ExecuteSetter(conditionProp, "m_intValue", (intValueProp) => {
                                    intValueProp.intValue = 1;
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
                                    variableProp.objectReferenceValue = intVariable1;
                                });
                                Utils.ExecuteSetter(conditionProp, "m_intValue", (intValueProp) => {
                                    intValueProp.intValue = 2;
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
                Utils.ExecuteSetter(dialogueNodeProp, "m_options", (dialogueNodeOptionsProp) => {
                    dialogueNodeOptionsProp.arraySize = 0;
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
                Utils.ExecuteSetter(dialogueNodeProp, "m_options", (dialogueNodeOptionsProp) => {
                    dialogueNodeOptionsProp.arraySize = 0;
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
    }

    [TearDown]
    public void TearDown()
    {
        m_dialogueController.End();
        m_currentDialogue = null;
        m_currentDialogueText = null;
        m_currentDialogueOptions = null;
    }

    [Test]
    public void TestOptions()
    {
        Assert.AreEqual(m_currentDialogue, null);

        /**
         * Test to select Option 1
         */

        // begin dialogue
        m_dialogueController.StartDialogue();
        Assert.AreEqual(m_currentDialogue, m_dialogue);

        // begin text
        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Begin Text");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Text 1");
        Assert.AreEqual(m_currentDialogueOptions, null);

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, null);
        Assert.AreNotEqual(m_currentDialogueOptions, null);

        // Dialogue options keep emitting
        m_dialogueController.Next();
        Assert.AreNotEqual(m_currentDialogueOptions, null);
        Assert.AreEqual(m_currentDialogueOptions.Count, 2);

        DialogueOption dialogueOption1 = m_currentDialogueOptions[0];
        m_dialogueController.SelectOption(dialogueOption1);
        Assert.AreEqual(m_currentDialogueText, "Text 2.1");
        Assert.AreEqual(m_currentDialogueOptions, null);

        // No effect
        m_dialogueController.SelectOption(dialogueOption1);
        Assert.AreEqual(m_currentDialogueText, "Text 2.1");
        Assert.AreEqual(m_currentDialogueOptions, null);

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, null);

        // current dialogue has ended
        Assert.AreEqual(m_currentDialogue, null);

        /**
         * Test to select Option 1
         */

        // begin dialogue
        m_dialogueController.StartDialogue();
        Assert.AreEqual(m_currentDialogue, m_dialogue);

        // begin text
        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Begin Text");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Text 1");
        Assert.AreEqual(m_currentDialogueOptions, null);

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, null);
        Assert.AreNotEqual(m_currentDialogueOptions, null);

        // Dialogue options keep emitting
        m_dialogueController.Next();
        Assert.AreNotEqual(m_currentDialogueOptions, null);
        Assert.AreEqual(m_currentDialogueOptions.Count, 2);

        DialogueOption dialogueOption2 = m_currentDialogueOptions[0];
        m_dialogueController.SelectOption(dialogueOption2);
        Assert.AreEqual(m_currentDialogueText, "Text 2.1");
        Assert.AreEqual(m_currentDialogueOptions, null);

        // No effect
        m_dialogueController.SelectOption(dialogueOption2);
        Assert.AreEqual(m_currentDialogueText, "Text 2.1");
        Assert.AreEqual(m_currentDialogueOptions, null);

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, null);

        /**
         * Test to select Option 2
         */

        // begin dialogue
        m_dialogueController.StartDialogue();
        Assert.AreEqual(m_currentDialogue, m_dialogue);

        // begin text
        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Begin Text");

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, "Text 1");
        Assert.AreEqual(m_currentDialogueOptions, null);

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, null);
        Assert.AreNotEqual(m_currentDialogueOptions, null);

        // Dialogue options keep emitting
        m_dialogueController.Next();
        Assert.AreNotEqual(m_currentDialogueOptions, null);
        Assert.AreEqual(m_currentDialogueOptions.Count, 2);

        DialogueOption dialogueOption3 = m_currentDialogueOptions[1];
        m_dialogueController.SelectOption(dialogueOption3);
        Assert.AreEqual(m_currentDialogueText, "Text 2.2");
        Assert.AreEqual(m_currentDialogueOptions, null);

        // No effect
        m_dialogueController.SelectOption(dialogueOption3);
        Assert.AreEqual(m_currentDialogueText, "Text 2.2");
        Assert.AreEqual(m_currentDialogueOptions, null);

        m_dialogueController.Next();
        Assert.AreEqual(m_currentDialogueText, null);
    }
}

}
