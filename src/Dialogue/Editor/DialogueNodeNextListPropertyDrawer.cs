using System;
using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class DialogueNodeNextListPropertyDrawer
{
    private static float margin = 2;

    private ConditionReorderableListManager m_conditionReorderableListManager;

    private DrawDialogueTextLinkageRowDelegate m_DrawDialogueTextLinkageRow;

    public DialogueNodeNextListPropertyDrawer(ConditionReorderableListManager conditionReorderableListManager, DrawDialogueTextLinkageRowDelegate DrawDialogueTextLinkageRow)
    {
        m_conditionReorderableListManager = conditionReorderableListManager;
        m_DrawDialogueTextLinkageRow = DrawDialogueTextLinkageRow;
    }

    public float GetHeight(SerializedProperty dialogueNodeNextsProp)
    {
        float headerHeight = EditorGUIUtility.singleLineHeight + margin;
        float ifsHeight = 0;

        for (int i = 0; i < dialogueNodeNextsProp.arraySize; i++)
        {
            SerializedProperty dialogueNodeNextProp = dialogueNodeNextsProp.GetArrayElementAtIndex(i);
            SerializedProperty conditionsProp = dialogueNodeNextProp.FindPropertyRelative("m_conditions");

            ConditionReorderableList r = m_conditionReorderableListManager.GetReorderableList(conditionsProp);

            ifsHeight += r.GetHeight() + margin;

            // Next Dialogue Text
            ifsHeight += EditorGUIUtility.singleLineHeight + margin;
        }

        // New Branch Button
        float newBranchHeight = EditorGUIUtility.singleLineHeight + margin;

        // Else Row
        float elseHeight = EditorGUIUtility.singleLineHeight;

        return headerHeight + ifsHeight + newBranchHeight + elseHeight;
    }

    public void DrawDialogueNodeNexts(Rect totalRect, SerializedProperty dialogueNodeNextsProp, SerializedProperty finalProp, string headerText)
    {
        Rect headerRect = new Rect(totalRect.x, totalRect.y, totalRect.width, EditorGUIUtility.singleLineHeight);

        GUIStyle headerStyle = new GUIStyle();
        headerStyle.normal.background = new Texture2D(1, 1);
        headerStyle.padding = EditorStyles.miniButtonLeft.padding;
        headerStyle.alignment = TextAnchor.MiddleLeft;
        headerStyle.normal.background.SetPixels(new Color[]{ new Color(0.3f, 0.3f, 0.3f) });
        headerStyle.normal.background.Apply();
        headerStyle.normal.textColor = Color.white;
        GUI.Label(headerRect, headerText, headerStyle);

        totalRect.y += headerRect.height + margin;

        for (int i = 0; i < dialogueNodeNextsProp.arraySize; i++)
        {
            SerializedProperty dialogueNodeNextProp = dialogueNodeNextsProp.GetArrayElementAtIndex(i);

            // Condition
            {
                SerializedProperty conditionsProp = dialogueNodeNextProp.FindPropertyRelative("m_conditions");
                ConditionReorderableList conditionReorderableList = m_conditionReorderableListManager.GetReorderableList(conditionsProp);
                float conditionReorderableListHeight = conditionReorderableList.GetHeight();

                Rect borderRect = new Rect(totalRect.x, totalRect.y - 2, EditorGUIUtility.singleLineHeight + 2, conditionReorderableListHeight + 2);
                Rect ifLabelRect = new Rect(totalRect.x + borderRect.width, totalRect.y, 40, EditorGUIUtility.singleLineHeight);
                Rect deleteButtonRect = new Rect(totalRect.xMax - 60, totalRect.y, 60, EditorGUIUtility.singleLineHeight);

                EditorGUI.DrawRect(borderRect, new Color(0.3f, 0.3f, 0.3f));

                GUIStyle ifLabelStyle = new GUIStyle();
                ifLabelStyle.normal.background = new Texture2D(1, 1);
                ifLabelStyle.padding = EditorStyles.miniButtonLeft.padding;
                ifLabelStyle.alignment = TextAnchor.MiddleLeft;
                ifLabelStyle.normal.background.SetPixels(new Color[]{ new Color(0.3f, 0.3f, 0.3f) });
                ifLabelStyle.normal.background.Apply();
                ifLabelStyle.normal.textColor = Color.white;
                GUI.Label(ifLabelRect, "If", ifLabelStyle);

                Rect conditionReorderableListRect = new Rect(totalRect.x + borderRect.width + ifLabelRect.width, totalRect.y, totalRect.width - borderRect.width - ifLabelRect.width - deleteButtonRect.width, conditionReorderableListHeight);
                conditionReorderableList.DoList(conditionReorderableListRect);

                if (GUI.Button(deleteButtonRect, "De|ete", EditorStyles.miniButton))
                {
                    DeleteBranch(dialogueNodeNextsProp, i);
                    return;
                }

                totalRect.y += conditionReorderableListHeight + margin;
            }

            // Next Dialogue Text
            {
                Rect borderRect = new Rect(totalRect.x, totalRect.y - 2, EditorGUIUtility.singleLineHeight + 2, EditorGUIUtility.singleLineHeight + 2);
                float indent = 20;
                Rect propRect = new Rect(totalRect.x + borderRect.width + indent, totalRect.y, totalRect.width - borderRect.width - indent, EditorGUIUtility.singleLineHeight);

                SerializedProperty nextDialogueTextProp = dialogueNodeNextProp.FindPropertyRelative("m_next");

                EditorGUI.DrawRect(borderRect, new Color(0.3f, 0.3f, 0.3f));
                m_DrawDialogueTextLinkageRow(propRect, nextDialogueTextProp, 20, new GUIContent("\u2192"));

                totalRect.y += propRect.height + margin;
            }

        }

        {
            Rect borderRect = new Rect(totalRect.x, totalRect.y - 2, EditorGUIUtility.singleLineHeight + 2, EditorGUIUtility.singleLineHeight + 2);
            Rect labelRect = new Rect(totalRect.x + borderRect.width, totalRect.y, 40, EditorGUIUtility.singleLineHeight);
            Rect buttonRect = new Rect(totalRect.x + borderRect.width + labelRect.width, totalRect.y, totalRect.width - borderRect.width - labelRect.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.DrawRect(borderRect, new Color(0.3f, 0.3f, 0.3f));

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.background = new Texture2D(1, 1);
            labelStyle.padding = EditorStyles.miniButtonLeft.padding;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.normal.background.SetPixels(new Color[]{ new Color(0.5f, 0.5f, 0.5f) });
            labelStyle.normal.background.Apply();
            labelStyle.normal.textColor = Color.white;
            GUI.Label(labelRect, "If", labelStyle);

            if (GUI.Button(buttonRect, "New Branch", EditorStyles.miniButtonRight))
            {
                AddBranch(dialogueNodeNextsProp);
                return;
            }

            totalRect.y += EditorGUIUtility.singleLineHeight + margin;
        }

        // Final Next
        {
            Rect borderRect = new Rect(totalRect.x, totalRect.y - 2, EditorGUIUtility.singleLineHeight + 2, EditorGUIUtility.singleLineHeight + 2);
            Rect labelRect = new Rect(totalRect.x + borderRect.width, totalRect.y, 40, EditorGUIUtility.singleLineHeight);
            Rect propRect = new Rect(totalRect.x + borderRect.width + labelRect.width, totalRect.y, totalRect.width - borderRect.width - labelRect.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.DrawRect(borderRect, new Color(0.3f, 0.3f, 0.3f));

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.background = new Texture2D(1, 1);
            labelStyle.padding = EditorStyles.miniButtonLeft.padding;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.normal.background.SetPixels(new Color[]{ Color.red });
            labelStyle.normal.background.Apply();
            labelStyle.normal.textColor = Color.white;
            GUI.Label(labelRect, "Else", labelStyle);

            m_DrawDialogueTextLinkageRow(propRect, finalProp, 0, GUIContent.none);
        }
    }

    void AddBranch(SerializedProperty dialogueNodeNextsProp)
    {
        int index = dialogueNodeNextsProp.arraySize;
        dialogueNodeNextsProp.InsertArrayElementAtIndex(index);
        dialogueNodeNextsProp.serializedObject.ApplyModifiedProperties();

        SerializedProperty element = dialogueNodeNextsProp.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_conditions").arraySize = 0;
        element.FindPropertyRelative("m_next").objectReferenceValue = null;
    }

    void DeleteBranch(SerializedProperty dialogueNodeNextsProp, int index)
    {
        dialogueNodeNextsProp.DeleteArrayElementAtIndex(index);
    }
}

public delegate void DrawDialogueTextLinkageRowDelegate(Rect rect, SerializedProperty property, float labelWidth, GUIContent label = null);

}
