using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem
{

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    SerializedProperty dialogueNodesProp;
    ReorderableList dialogueNodeReorderableList;

    List<DialogueText> m_undefinedDialogueTexts = new List<DialogueText>();
    List<DialogueText> m_unusedDialogueTexts = new List<DialogueText>();

    float DialogueTextRefFieldHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    float DialogueTextAreaFieldHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight * 3;
        }
    }

    float NextOptionFieldHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    float ButtonRowHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    void OnEnable()
    {
        dialogueNodesProp = serializedObject.FindProperty("m_dialogueNodes");
        dialogueNodeReorderableList = new ReorderableList(serializedObject, dialogueNodesProp, true, true, true, true);
        dialogueNodeReorderableList.drawElementCallback = DrawDialogueNodeElement;
        dialogueNodeReorderableList.elementHeightCallback = GetDialogueNodeElementHeight;
        dialogueNodeReorderableList.onAddDropdownCallback = AddDropdown;
        dialogueNodeReorderableList.onRemoveCallback = RemoveDialogueNodeElement;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Dialogue dialogue = (Dialogue) serializedObject.targetObject;
        (List<DialogueText> undefinedDialogueTexts, List<DialogueText> unusedDialogueTexts) = dialogue.GetUnreferencedDialoguedTextAndUnusedNodes();
        m_undefinedDialogueTexts = undefinedDialogueTexts;
        m_unusedDialogueTexts = unusedDialogueTexts;
        DrawBeginTextField();
        dialogueNodeReorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawBeginTextField()
    {
        SerializedProperty beginTextProp = serializedObject.FindProperty("m_beginText");
        DialogueText beginText = (DialogueText) beginTextProp.objectReferenceValue;
        Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        EditorGUI.BeginProperty(rect, new GUIContent("Begin Text"), beginTextProp);
        DrawDialogueTextLinkageRow(new Rect(rect.x, rect.y, rect.width, rect.height), beginTextProp, EditorGUIUtility.labelWidth, new GUIContent("Begin Text"));
        EditorGUI.EndProperty();
    }

    void DrawDialogueNodeElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        // Handle delete intermediate elements by code
        if (index >= dialogueNodeReorderableList.serializedProperty.arraySize)
        {
            return;
        }

        float prevLabelWidth = EditorGUIUtility.labelWidth;

        SerializedProperty beginTextProp = serializedObject.FindProperty("m_beginText");
        DialogueText beginText = (DialogueText) beginTextProp.objectReferenceValue;
        SerializedProperty dialogueNodeProp = dialogueNodeReorderableList.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty dialogueTextProp = dialogueNodeProp.FindPropertyRelative("m_dialogueText");
        DialogueText dialogueText = (DialogueText) dialogueTextProp.objectReferenceValue;
        SerializedProperty nextOptionProp = dialogueNodeProp.FindPropertyRelative("m_nextOption");

        rect.y += 2;

        Rect dialogueTextRowColumn1Rect = new Rect(rect.x, rect.y, rect.width - 160, DialogueTextRefFieldHeight);
        Rect dialogueTextRowColumn2Rect = new Rect(rect.xMax - 60 - 90, rect.y, 90, DialogueTextRefFieldHeight);
        Rect dialogueTextRowColumn3Rect = new Rect(rect.xMax - 60, rect.y, 60, DialogueTextRefFieldHeight);

        bool isUnused = m_unusedDialogueTexts.Contains(dialogueText);

        if (isUnused)
        {
            Color originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 210, DialogueTextRefFieldHeight), dialogueTextProp, GUIContent.none);
            GUI.backgroundColor = originalBackgroundColor;
            EditorGUI.BeginDisabledGroup(true);
            GUI.Button(new Rect(rect.xMax - 60 - 90 - 50, rect.y, 50, DialogueTextRefFieldHeight), "Unused", EditorStyles.miniButtonLeft);
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            EditorGUI.PropertyField(dialogueTextRowColumn1Rect, dialogueTextProp, GUIContent.none);
        }
        if (dialogueText != null) {
            bool isBeginText = dialogueText == beginText;
            EditorGUI.BeginDisabledGroup(isBeginText);
            if (GUI.Button(dialogueTextRowColumn2Rect, isBeginText ? "Begin Text" : "As Begin Text", isUnused ? EditorStyles.miniButtonMid : EditorStyles.miniButtonLeft))
            {
                beginTextProp.objectReferenceValue = dialogueText;
            }
            EditorGUI.EndDisabledGroup();
            if (GUI.Button(dialogueTextRowColumn3Rect, "De|ete", EditorStyles.miniButtonRight))
            {
                if (EditorUtility.DisplayDialog("Delete Dialogue Node", "Are you sure you want to delete?", "Yes", "No"))
                {
                    DeleteDialogueNode(index);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
            }
        }
        else
        {
            if (GUI.Button(dialogueTextRowColumn2Rect, "New Text", EditorStyles.miniButtonLeft))
            {
                DialogueText newDialogueText = PromptToCreate<DialogueText>("New Dialogue Text", "Dialogue Text");
                dialogueTextProp.objectReferenceValue = newDialogueText;
            }
            if (GUI.Button(dialogueTextRowColumn3Rect, "De|ete", EditorStyles.miniButtonRight))
            {
                if (EditorUtility.DisplayDialog("Delete Dialogue Node", "Are you sure you want to delete?", "Yes", "No"))
                {
                    DeleteDialogueNode(index);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
            }
        }

        rect.y += DialogueTextRefFieldHeight;

        if (dialogueText != null)
        {
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
            dialogueText.text = EditorGUI.TextArea(new Rect(rect.x, rect.y, rect.width, DialogueTextAreaFieldHeight), dialogueText.text, textAreaStyle);
            EditorUtility.SetDirty(dialogueText);

            rect.y += DialogueTextAreaFieldHeight;
        }

        rect.y += 2;

        DialogueNextOption nextOption = (DialogueNextOption) nextOptionProp.intValue;

        DrawDialogueNextOptionRow(new Rect(rect.x, rect.y, rect.width, NextOptionFieldHeight), nextOptionProp);

        rect.y += NextOptionFieldHeight;

        rect.y += 2;

        if (nextOption == DialogueNextOption.DIALOGUE_TEXT)
        {
            SerializedProperty nextDialogueTextProp = dialogueNodeProp.FindPropertyRelative("m_next");
            DialogueText nextDialogueText = (DialogueText) nextDialogueTextProp.objectReferenceValue;

            EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, DialogueTextRefFieldHeight + 2, DialogueTextRefFieldHeight + 2), new Color(0.3f, 0.3f, 0.3f));
            DrawDialogueTextLinkageRow(new Rect(rect.x + 20, rect.y, rect.width - 20, DialogueTextRefFieldHeight), nextDialogueTextProp, 20.0f, new GUIContent("\u2198"));

            rect.y += DialogueTextRefFieldHeight;
        }
        else if (nextOption == DialogueNextOption.DIALOGUE_OPTIONS)
        {
            SerializedProperty nextDialogueOptionsProp = dialogueNodeProp.FindPropertyRelative("m_options");

            for (int i = 0; i < nextDialogueOptionsProp.arraySize; i++)
            {
                SerializedProperty nextDialogueOptionProp = nextDialogueOptionsProp.GetArrayElementAtIndex(i);
                SerializedProperty optionTextProp = nextDialogueOptionProp.FindPropertyRelative("m_dialogueOptionText");
                DialogueOptionText optionText = (DialogueOptionText) optionTextProp.objectReferenceValue;
                SerializedProperty optionNextProp = nextDialogueOptionProp.FindPropertyRelative("m_next");
                DialogueText optionNextText = (DialogueText) optionNextProp.objectReferenceValue;

                EditorGUIUtility.labelWidth = 20.0f;
                EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, DialogueTextRefFieldHeight + 2, DialogueTextRefFieldHeight + 2), new Color(0.3f, 0.3f, 0.3f));
                EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y, 100, DialogueTextRefFieldHeight), optionTextProp, new GUIContent("\u2192"));
                if (optionText)
                {
                    optionText.text = EditorGUI.TextField(new Rect(rect.x + 120, rect.y, rect.width - 180, DialogueTextRefFieldHeight), optionText.text);
                    EditorUtility.SetDirty(optionText);
                } else {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.TextField(new Rect(rect.x + 120, rect.y, rect.width - 280, DialogueTextRefFieldHeight), "");
                    EditorGUI.EndDisabledGroup();
                    if (GUI.Button(new Rect(rect.xMax - 60 - 100, rect.y, 100, DialogueTextRefFieldHeight), "New Option Text", EditorStyles.miniButtonMid))
                    {
                        DialogueOptionText newOptionText = PromptToCreate<DialogueOptionText>("New Dialogue Option Text", "Dialogue Option Text");
                        if (newOptionText != null)
                        {
                            optionTextProp.objectReferenceValue = newOptionText;
                        }
                    }
                }
                if (GUI.Button(new Rect(rect.xMax - 60, rect.y, 60, DialogueTextRefFieldHeight), "De|ete", EditorStyles.miniButtonRight))
                {
                    if (EditorUtility.DisplayDialog("Delete Option", "Are you sure you want to delete?", "Yes", "No"))
                    {
                        DeleteOption(nextDialogueOptionsProp, i);
                        return;
                    }
                }

                rect.y += DialogueTextRefFieldHeight;

                rect.y += 2;

                EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, DialogueTextRefFieldHeight + 2, DialogueTextRefFieldHeight + 2), new Color(0.3f, 0.3f, 0.3f));
                DrawDialogueTextLinkageRow(new Rect(rect.x + 40, rect.y, rect.width - 40, DialogueTextRefFieldHeight), optionNextProp, 20.0f, new GUIContent("\u2198"));

                rect.y += DialogueTextRefFieldHeight;

                rect.y += 2;
            }

            EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, DialogueTextRefFieldHeight + 2, DialogueTextRefFieldHeight + 2), new Color(0.3f, 0.3f, 0.3f));
            EditorGUI.LabelField(new Rect(rect.x + 20, rect.y, 20, ButtonRowHeight), "\u2192");
            if (GUI.Button(new Rect(rect.x + 40, rect.y, rect.width - 40, ButtonRowHeight), "New Option", EditorStyles.miniButton))
            {
                AddOption(nextDialogueOptionsProp);
            }

            rect.y += ButtonRowHeight;
        }

        rect.y += 10;

        EditorGUIUtility.labelWidth = prevLabelWidth;
    }

    void DrawDialogueNextOptionRow(Rect rect, SerializedProperty nextOptionProp)
    {
        // string[] nextOptionToolbarTexts = { "End", "Text", "Options" };
        // nextOptionProp.intValue = GUI.Toolbar(new Rect(rect.x, rect.y, rect.width, NextOptionFieldHeight), nextOptionProp.intValue, nextOptionToolbarTexts);

        DialogueNextOption nextOption = (DialogueNextOption) nextOptionProp.intValue;

        GUIStyle selectedStyle = new GUIStyle();
        selectedStyle.normal.background = new Texture2D(1, 1);
        selectedStyle.padding = EditorStyles.miniButtonLeft.padding;
        selectedStyle.alignment = TextAnchor.MiddleLeft;

        string selectedText = "";
        string[] texts = new string[]{};
        DialogueNextOption[] selectValues = new DialogueNextOption[]{};

        switch (nextOption)
        {
            case DialogueNextOption.END:
            {
                selectedText = "End";
                texts = new string[]{ "Text", "Options" };
                selectValues = new DialogueNextOption[]{ DialogueNextOption.DIALOGUE_TEXT, DialogueNextOption.DIALOGUE_OPTIONS };
                selectedStyle.normal.background.SetPixels(new Color[]{ Color.red });
                selectedStyle.normal.background.Apply();
                selectedStyle.normal.textColor = Color.white;
                break;
            }
            case DialogueNextOption.DIALOGUE_TEXT:
            {
                selectedText = "\u2198 Text";
                texts = new string[]{ "Options", "End" };
                selectValues = new DialogueNextOption[]{ DialogueNextOption.DIALOGUE_OPTIONS, DialogueNextOption.END };
                selectedStyle.normal.background.SetPixels(new Color[]{ new Color(0.3f, 0.3f, 0.3f) });
                selectedStyle.normal.background.Apply();
                selectedStyle.normal.textColor = Color.white;
                break;
            }
            case DialogueNextOption.DIALOGUE_OPTIONS:
            {
                selectedText = "\u2198 Options";
                texts = new string[]{ "Text", "End" };
                selectValues = new DialogueNextOption[]{ DialogueNextOption.DIALOGUE_TEXT, DialogueNextOption.END };
                selectedStyle.normal.background.SetPixels(new Color[]{ new Color(0.3f, 0.3f, 0.3f) });
                selectedStyle.normal.background.Apply();
                selectedStyle.normal.textColor = Color.white;
                break;
            }
        }

        GUI.Label(new Rect(rect.x, rect.y, rect.width - 120, rect.height), selectedText, selectedStyle);
        if (GUI.Button(new Rect(rect.xMax - 120, rect.y, 60, NextOptionFieldHeight), texts[0], EditorStyles.miniButtonMid))
        {
            nextOptionProp.intValue = (int) selectValues[0];
        }
        if (GUI.Button(new Rect(rect.xMax - 60, rect.y, 60, NextOptionFieldHeight), texts[1], EditorStyles.miniButtonRight))
        {
            nextOptionProp.intValue = (int) selectValues[1];
        }
    }

    void DrawDialogueTextLinkageRow(Rect rect, SerializedProperty property, float labelWidth, GUIContent label = null)
    {
        float prevLabelWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = labelWidth;

        DialogueText dialogueText = (DialogueText) property.objectReferenceValue;

        bool isUndefined = m_undefinedDialogueTexts.Contains(dialogueText);

        if (isUndefined)
        {
            Color prevBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 160, rect.height), property, label);
            GUI.backgroundColor = prevBackgroundColor;
            EditorGUI.BeginDisabledGroup(true);
            GUI.Button(new Rect(rect.xMax - 60 - 90, rect.y, 90, rect.height), "Undefined", EditorStyles.miniButtonLeft);
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 70, rect.height), property, label);
        }

        GUIStyle rightButtonStyle = isUndefined ? EditorStyles.miniButtonRight : EditorStyles.miniButton;

        if (dialogueText == null)
        {
            if (GUI.Button(new Rect(rect.xMax - 60, rect.y, 60, rect.height), "New", rightButtonStyle))
            {
                DialogueText newDialogueText = PromptToCreate<DialogueText>("New Dialogue Text", "Dialogue Text");
                property.objectReferenceValue = newDialogueText;
            }
        }
        else
        {
            if (GUI.Button(new Rect(rect.xMax - 60, rect.y, 60, DialogueTextRefFieldHeight), "+|->", rightButtonStyle))
            {
                int dialogueNodeIndex = FindDialogueNodeIndexByDialogueText(dialogueText);
                if (dialogueNodeIndex > -1)
                {
                    dialogueNodeReorderableList.index = dialogueNodeIndex;
                }
                else
                {
                    Add(dialogueNodeReorderableList, dialogueText);
                }
            }
        }

        EditorGUIUtility.labelWidth = prevLabelWidth;
    }

    float GetDialogueNodeElementHeight(int index)
    {
        float height = 2 + DialogueTextRefFieldHeight;

        // Handle delete intermediate elements by code
        if (index >= dialogueNodeReorderableList.serializedProperty.arraySize)
        {
            return 0;
        }

        SerializedProperty element = dialogueNodeReorderableList.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty dialogueTextProp = element.FindPropertyRelative("m_dialogueText");
        DialogueText dialogueText = (DialogueText) dialogueTextProp.objectReferenceValue;

        if (dialogueText != null)
        {
            height += 2 + DialogueTextAreaFieldHeight;
        }

        height += 2 + NextOptionFieldHeight;

        SerializedProperty nextOptionProp = element.FindPropertyRelative("m_nextOption");

        DialogueNextOption nextOption = (DialogueNextOption) nextOptionProp.enumValueIndex;

        if (nextOption == DialogueNextOption.DIALOGUE_TEXT)
        {
            height += 2 + DialogueTextRefFieldHeight;
        }
        else if (nextOption == DialogueNextOption.DIALOGUE_OPTIONS)
        {
            SerializedProperty optionsProp = element.FindPropertyRelative("m_options");
            int optionSize = optionsProp.arraySize;

            height += (2 + DialogueTextRefFieldHeight + 2 + DialogueTextRefFieldHeight) * optionSize + 2 + ButtonRowHeight;
        }

        height += 4;

        return height;
    }

    int FindDialogueNodeIndexByDialogueText(DialogueText dialogueText)
    {
        int size = dialogueNodesProp.arraySize;
        for (int i = 0; i < size; i++)
        {
            SerializedProperty dialogueTextProp = dialogueNodesProp.GetArrayElementAtIndex(i).FindPropertyRelative("m_dialogueText");
            DialogueText t = (DialogueText) dialogueTextProp.objectReferenceValue;
            if (t == dialogueText)
            {
                return i;
            }
        }
        return -1;
    }

    void Add(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        // Highlight new item
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dialogueText").objectReferenceValue = null;
        element.FindPropertyRelative("m_nextOption").enumValueIndex = 0;
        element.FindPropertyRelative("m_next").objectReferenceValue = null;
        element.FindPropertyRelative("m_options").arraySize = 0;
    }

    void Add(ReorderableList l, DialogueText dialogueText)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        // Highlight new item
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dialogueText").objectReferenceValue = dialogueText;
        element.FindPropertyRelative("m_nextOption").enumValueIndex = 0;
        element.FindPropertyRelative("m_next").objectReferenceValue = null;
        element.FindPropertyRelative("m_options").arraySize = 0;
    }

    void AddDropdown(Rect buttonRect, ReorderableList l)
    {
        GenericMenu menu = new GenericMenu();
        foreach (DialogueText undefinedDialogueText in m_undefinedDialogueTexts)
        {
            menu.AddItem(new GUIContent(undefinedDialogueText.name), false, () => {
                Add(l, undefinedDialogueText);
                serializedObject.ApplyModifiedProperties();
            });
        }
        if (m_undefinedDialogueTexts.Count > 0)
        {
            menu.AddSeparator("");
        }
        menu.AddItem(new GUIContent("New"), false, () => {
            Add(l);
            serializedObject.ApplyModifiedProperties();
        });
        menu.ShowAsContext();
    }

    void AddOption(SerializedProperty optionsProp)
    {
        int index = optionsProp.arraySize;
        optionsProp.InsertArrayElementAtIndex(index);
        serializedObject.ApplyModifiedProperties();

        SerializedProperty element = optionsProp.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dialogueOptionText").objectReferenceValue = null;
        element.FindPropertyRelative("m_next").objectReferenceValue = null;
    }

    void DeleteOption(SerializedProperty optionsProp, int index)
    {
        optionsProp.DeleteArrayElementAtIndex(index);
    }

    void DeleteDialogueNode(int index)
    {
        dialogueNodesProp.DeleteArrayElementAtIndex(index);
    }

    void RemoveDialogueNodeElement(ReorderableList l)
    {
        if (EditorUtility.DisplayDialog("Delete Dialogue Node", "Are you sure you want to delete?", "Yes", "No"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        }
    }

    T PromptToCreate<T>(string title, string defaultName) where T : ScriptableObject
    {
        string directoryPath = Utils.GetActiveDirectory();
        string createdAssetPath = EditorUtility.SaveFilePanel(title, directoryPath, defaultName, "asset");
        if (createdAssetPath.Length != 0)
        {
            createdAssetPath = createdAssetPath.Replace(Application.dataPath, "Assets");
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, createdAssetPath);
            AssetDatabase.SaveAssets();
            return asset;
        }
        return null;
    }
}

}
