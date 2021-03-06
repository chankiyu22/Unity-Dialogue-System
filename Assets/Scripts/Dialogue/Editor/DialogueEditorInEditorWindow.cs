using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class DialogueEditorInEditorWindow
{
    SerializedObject serializedObject;

    SerializedProperty dialogueNodesProp;
    ReorderableList dialogueNodeReorderableList;

    List<ReorderableList> dialogueNodeVariableAssignmentReorderableLists = new List<ReorderableList>();

    DialogueVariableReorderableList dialogueVariableReorderableList;

    List<DialogueText> m_undefinedDialogueTexts = new List<DialogueText>();
    List<DialogueText> m_unusedDialogueTexts = new List<DialogueText>();

    VariableAssignmentListPropertyDrawerManager variableAssignmentListPropertyDrawerManager = new VariableAssignmentListPropertyDrawerManager();
    ConditionReorderableListManager conditionReorderableListManager = new ConditionReorderableListManager();
    DialogueNodeNextListPropertyDrawer m_dialogueNodeNextListPropertyDrawer;

    Rect m_viewportRect = Rect.zero;

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

    float ButtonRowHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    public DialogueEditorInEditorWindow(SerializedObject serializedObject)
    {
        this.serializedObject = serializedObject;
        dialogueNodesProp = serializedObject.FindProperty("m_dialogueNodes");
        dialogueNodeReorderableList = new ReorderableList(serializedObject, dialogueNodesProp, true, true, true, true);
        dialogueNodeReorderableList.drawElementCallback = DrawDialogueNodeElement;
        dialogueNodeReorderableList.elementHeightCallback = GetDialogueNodeElementHeight;
        dialogueNodeReorderableList.onAddDropdownCallback = AddDropdown;
        dialogueNodeReorderableList.onRemoveCallback = RemoveDialogueNodeElement;
        dialogueNodeReorderableList.drawHeaderCallback = DrawDialogueNodeListHeader;

        m_dialogueNodeNextListPropertyDrawer = new DialogueNodeNextListPropertyDrawer(conditionReorderableListManager, DrawDialogueTextLinkageRow);

        SerializedProperty dialogueVariableListProp = serializedObject.FindProperty("m_dialogueVariables");
        dialogueVariableReorderableList = new DialogueVariableReorderableList(serializedObject, dialogueVariableListProp);
    }

    public void OnInspectorGUI(Rect viewportRect)
    {
        m_viewportRect = viewportRect;

        serializedObject.Update();

        Dialogue dialogue = (Dialogue) serializedObject.targetObject;
        (List<DialogueText> undefinedDialogueTexts, List<DialogueText> unusedDialogueTexts) = Utils.GetUnreferencedDialogueTextAndUnusedNodes(dialogue);
        m_undefinedDialogueTexts = undefinedDialogueTexts;
        m_unusedDialogueTexts = unusedDialogueTexts;

        Rect dialogueVariableReorderableListRect = EditorGUILayout.GetControlRect(false, dialogueVariableReorderableList.GetHeight());
        if (isRectInViewport(dialogueVariableReorderableListRect))
        {
            dialogueVariableReorderableList.DoList(dialogueVariableReorderableListRect);
        }

        SerializedProperty beginTextsProp = serializedObject.FindProperty("m_beginTexts");
        SerializedProperty finalBeginTextProp = serializedObject.FindProperty("m_finalBeginText");
        float beginTextsHeight = m_dialogueNodeNextListPropertyDrawer.GetHeight(beginTextsProp);
        Rect beginTextsRect = EditorGUILayout.GetControlRect(false, beginTextsHeight);

        if (isRectInViewport(beginTextsRect))
        {
            m_dialogueNodeNextListPropertyDrawer.DrawDialogueNodeNexts(
                beginTextsRect,
                beginTextsProp,
                finalBeginTextProp,
                "Begin Texts");
        }

        EditorGUILayout.Space();

        Rect dialogueNodeReorderableListRect = EditorGUILayout.GetControlRect(false, dialogueNodeReorderableList.GetHeight());
        if (isRectInViewport(dialogueNodeReorderableListRect))
        {
            dialogueNodeReorderableList.DoList(dialogueNodeReorderableListRect);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DrawDialogueNodeListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Dialogue Items");
    }

    void DrawDialogueNodeElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        if (!isRectInViewport(rect)) {
            return;
        }
        // Handle delete intermediate elements by code
        if (index >= dialogueNodeReorderableList.serializedProperty.arraySize)
        {
            return;
        }

        float prevLabelWidth = EditorGUIUtility.labelWidth;

        SerializedProperty finalBeginTextProp = serializedObject.FindProperty("m_finalBeginText");
        DialogueText finalBeginText = (DialogueText) finalBeginTextProp.objectReferenceValue;
        SerializedProperty dialogueNodeProp = dialogueNodeReorderableList.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty dialogueTextProp = dialogueNodeProp.FindPropertyRelative("m_dialogueText");
        DialogueText dialogueText = (DialogueText) dialogueTextProp.objectReferenceValue;

        rect.y += 2;

        Rect dialogueTextRowColumn1Rect = new Rect(rect.x, rect.y, rect.width - 155, DialogueTextRefFieldHeight);
        Rect dialogueTextRowColumn2Rect = new Rect(rect.xMax - 60 - 90, rect.y, 90, DialogueTextRefFieldHeight);
        Rect dialogueTextRowColumn3Rect = new Rect(rect.xMax - 60, rect.y, 60, DialogueTextRefFieldHeight);

        bool isUnused = m_unusedDialogueTexts.Contains(dialogueText);

        if (isUnused)
        {
            Color originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 205, DialogueTextRefFieldHeight), dialogueTextProp, GUIContent.none);
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
            bool isBeginText = dialogueText == finalBeginText;
            EditorGUI.BeginDisabledGroup(isBeginText);
            if (GUI.Button(dialogueTextRowColumn2Rect, isBeginText ? "Begin Text" : "As Begin Text", isUnused ? EditorStyles.miniButtonMid : EditorStyles.miniButtonLeft))
            {
                finalBeginTextProp.objectReferenceValue = dialogueText;
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

        SerializedProperty assignmentsProp = dialogueNodeProp.FindPropertyRelative("m_assignments");
        VariableAssignmentReorderableList dialogueNodeVariableAssignmentReorderableList = variableAssignmentListPropertyDrawerManager.GetReorderableList(assignmentsProp, "Change Variables");
        float dialogueNodeVariableAssignmentReorderableListHeight = dialogueNodeVariableAssignmentReorderableList.GetHeight();
        Rect dialogueNodeVariableAssignmentReorderableListRect = new Rect(rect.x, rect.y, rect.width, dialogueNodeVariableAssignmentReorderableListHeight);
        dialogueNodeVariableAssignmentReorderableList.DoList(dialogueNodeVariableAssignmentReorderableListRect);

        rect.y += dialogueNodeVariableAssignmentReorderableListHeight;

        rect.y += 2;

        GUIStyle optionsHeaderStyle = new GUIStyle();
        optionsHeaderStyle.normal.background = new Texture2D(1, 1);
        optionsHeaderStyle.padding = EditorStyles.miniButtonLeft.padding;
        optionsHeaderStyle.alignment = TextAnchor.MiddleLeft;
        optionsHeaderStyle.normal.background.SetPixels(new Color[]{ new Color(0.3f, 0.3f, 0.3f) });
        optionsHeaderStyle.normal.background.Apply();
        optionsHeaderStyle.normal.textColor = Color.white;
        Rect optionsHeaderRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
        GUI.Label(optionsHeaderRect, "Options", optionsHeaderStyle);

        rect.y += optionsHeaderRect.height;
        rect.y += 2;

        SerializedProperty nextDialogueOptionsProp = dialogueNodeProp.FindPropertyRelative("m_options");

        for (int i = 0; i < nextDialogueOptionsProp.arraySize; i++)
        {
            SerializedProperty nextDialogueOptionProp = nextDialogueOptionsProp.GetArrayElementAtIndex(i);
            SerializedProperty optionTextProp = nextDialogueOptionProp.FindPropertyRelative("m_dialogueOptionText");
            DialogueOptionText optionText = (DialogueOptionText) optionTextProp.objectReferenceValue;

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

            SerializedProperty optionAssignmentsProp = nextDialogueOptionProp.FindPropertyRelative("m_assignments");
            VariableAssignmentReorderableList optionVariableAssignmentReorderableList = variableAssignmentListPropertyDrawerManager.GetReorderableList(optionAssignmentsProp, "Change variables");
            float optionVariableAssignmentReorderableListHeight = optionVariableAssignmentReorderableList.GetHeight();
            Rect optionVariableAssignmentReorderableListRect = new Rect(rect.x + 20, rect.y, rect.width - 20, optionVariableAssignmentReorderableListHeight);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, DialogueTextRefFieldHeight + 2, optionVariableAssignmentReorderableListHeight + 2), new Color(0.3f, 0.3f, 0.3f));
            optionVariableAssignmentReorderableList.DoList(optionVariableAssignmentReorderableListRect);

            rect.y += optionVariableAssignmentReorderableListHeight;

            rect.y += 2;
        }

        EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, DialogueTextRefFieldHeight + 2, DialogueTextRefFieldHeight + 2), new Color(0.3f, 0.3f, 0.3f));
        EditorGUI.LabelField(new Rect(rect.x + 20, rect.y, 20, ButtonRowHeight), "\u2192");
        if (GUI.Button(new Rect(rect.x + 40, rect.y, rect.width - 40, ButtonRowHeight), "New Option", EditorStyles.miniButton))
        {
            AddOption(nextDialogueOptionsProp);
        }

        rect.y += ButtonRowHeight;

        rect.y += 2;

        SerializedProperty nextsProp = dialogueNodeProp.FindPropertyRelative("m_nexts");
        SerializedProperty finalNextProp = dialogueNodeProp.FindPropertyRelative("m_finalNext");
        float nextsHeight = m_dialogueNodeNextListPropertyDrawer.GetHeight(nextsProp);
        Rect nextsRect = new Rect(rect.x, rect.y, rect.width, nextsHeight);
        m_dialogueNodeNextListPropertyDrawer.DrawDialogueNodeNexts(
            nextsRect,
            nextsProp,
            finalNextProp,
            "Branches");

        rect.y += 2;

        rect.y += 10;

        EditorGUIUtility.labelWidth = prevLabelWidth;
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
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 155, rect.height), property, label);
            GUI.backgroundColor = prevBackgroundColor;
            EditorGUI.BeginDisabledGroup(true);
            GUI.Button(new Rect(rect.xMax - 60 - 90, rect.y, 90, rect.height), "Undefined", EditorStyles.miniButtonLeft);
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 65, rect.height), property, label);
        }

        GUIStyle rightButtonStyle = isUndefined ? EditorStyles.miniButtonRight : EditorStyles.miniButton;

        if (dialogueText == null)
        {
            Rect coverRect = new Rect(rect.x + labelWidth, rect.y, rect.width - 80 - labelWidth, rect.height);
            GUIStyle coverStyle = new GUIStyle();
            coverStyle.normal.background = new Texture2D(1, 1);
            coverStyle.padding = EditorStyles.miniButtonLeft.padding;
            coverStyle.alignment = TextAnchor.MiddleLeft;
            coverStyle.normal.background.SetPixels(new Color[]{ Color.red });
            coverStyle.normal.background.Apply();
            coverStyle.normal.textColor = Color.white;
            GUI.Label(coverRect, "End", coverStyle);
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

        SerializedProperty assignmentsProp = element.FindPropertyRelative("m_assignments");
        VariableAssignmentReorderableList variableAssignmentReorderableList = variableAssignmentListPropertyDrawerManager.GetReorderableList(assignmentsProp, "Change Variables");
        height += variableAssignmentReorderableList.GetHeight();

        height += EditorGUIUtility.singleLineHeight + 2;

        SerializedProperty optionsProp = element.FindPropertyRelative("m_options");
        for (int i = 0; i < optionsProp.arraySize; i++)
        {
            SerializedProperty optionProp = optionsProp.GetArrayElementAtIndex(i);
            SerializedProperty optionAssignmentsProp = optionProp.FindPropertyRelative("m_assignments");
            VariableAssignmentReorderableList optionVariableAssignmentReorderableList = variableAssignmentListPropertyDrawerManager.GetReorderableList(optionAssignmentsProp, "Change Variables");
            height += optionVariableAssignmentReorderableList.GetHeight() + 2;
        }
        height += (2 + DialogueTextRefFieldHeight) * optionsProp.arraySize + 2 + ButtonRowHeight;

        SerializedProperty nextsProp = element.FindPropertyRelative("m_nexts");
        height += m_dialogueNodeNextListPropertyDrawer.GetHeight(nextsProp) + 2;

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
        element.FindPropertyRelative("m_options").arraySize = 0;
        element.FindPropertyRelative("m_nexts").arraySize = 0;
        element.FindPropertyRelative("m_finalNext").objectReferenceValue = null;
    }

    void Add(ReorderableList l, DialogueText dialogueText)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        // Highlight new item
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dialogueText").objectReferenceValue = dialogueText;
        element.FindPropertyRelative("m_options").arraySize = 0;
        element.FindPropertyRelative("m_nexts").arraySize = 0;
        element.FindPropertyRelative("m_finalNext").objectReferenceValue = null;
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
        element.FindPropertyRelative("m_assignments").arraySize = 0;
    }

    void DeleteOption(SerializedProperty optionsProp, int index)
    {
        optionsProp.DeleteArrayElementAtIndex(index);
    }

    void AddNext(SerializedProperty nextsProp)
    {
        int index = nextsProp.arraySize;
        nextsProp.InsertArrayElementAtIndex(index);
        serializedObject.ApplyModifiedProperties();

        SerializedProperty element = nextsProp.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_conditions").arraySize = 0;
        element.FindPropertyRelative("m_next").objectReferenceValue = null;
    }

    void DeleteNext(SerializedProperty nextsProp, int index)
    {
        nextsProp.DeleteArrayElementAtIndex(index);
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

    bool isRectInViewport(Rect rect)
    {
        if (m_viewportRect != Rect.zero)
        {
            if (rect.y > m_viewportRect.y + m_viewportRect.height)
            {
                return false;
            }
            if (rect.y + rect.height < m_viewportRect.y)
            {
                return false;
            }
        }
        return true;
    }
}

}
