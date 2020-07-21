using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Chankiyu22.DialogueSystem.Dialogues;

namespace Chankiyu22.DialogueSystem
{

[CustomEditor(typeof(Plot))]
public class PlotEditor : Editor
{
    SerializedProperty plotItemsProp;
    ReorderableList plotItemReorderableList;

    void OnEnable()
    {
        plotItemsProp = serializedObject.FindProperty("m_plotItems");
        plotItemReorderableList = new ReorderableList(serializedObject, plotItemsProp, false, true, true, true);
        plotItemReorderableList.drawElementCallback = DrawPlotItemElement;
        plotItemReorderableList.elementHeightCallback = PlotItemHeight;
        plotItemReorderableList.onAddCallback = AddElement;
        plotItemReorderableList.onRemoveCallback = RemoveElement;
        plotItemReorderableList.drawHeaderCallback = DrawPlotItemListHeader;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty dialogueProp = serializedObject.FindProperty("m_dialogue");
        Dialogue dialogue = (Dialogue) dialogueProp.objectReferenceValue;

        Rect dialogueRowRect = EditorGUILayout.GetControlRect(false);
        Rect dialoguePropRect = new Rect(dialogueRowRect.x, dialogueRowRect.y, dialogueRowRect.width - 70, dialogueRowRect.height);
        Rect syncDialogueButtonRect = new Rect(dialogueRowRect.xMax - 60, dialogueRowRect.y, 60, dialogueRowRect.height);

        EditorGUI.PropertyField(dialoguePropRect, dialogueProp, GUIContent.none);
        using (new EditorGUI.DisabledScope(dialogue == null))
        {
            if (GUI.Button(syncDialogueButtonRect, "Sync", EditorStyles.miniButton))
            {
                SyncPlotWithDialogue((Plot) target, dialogue);
                serializedObject.Update();
            }
        }

        EditorGUILayout.Space();

        plotItemReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawPlotItemListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Plot Items");
    }

    void DrawPlotItemElement(Rect rect, int index, bool isActive, bool isFocus)
    {
        SerializedProperty plotItemProp = plotItemsProp.GetArrayElementAtIndex(index);

        rect.y += 2;
        EditorGUI.PropertyField(rect, plotItemProp);
    }

    float PlotItemHeight(int index)
    {
        SerializedProperty plotItemProp = plotItemsProp.GetArrayElementAtIndex(index);
        return EditorGUI.GetPropertyHeight(plotItemProp) + 4;
    }

    void AddElement(ReorderableList l)
    {
        int index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        l.index = index;

        SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("m_dialogueText").objectReferenceValue = null;
        element.FindPropertyRelative("m_character").objectReferenceValue = null;
        element.FindPropertyRelative("m_avatarTextureSource").objectReferenceValue = null;
    }

    void RemoveElement(ReorderableList l)
    {
        if (EditorUtility.DisplayDialog("Delete Plot Item", "Are you sure you want to delete?", "Yes", "No"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        }
    }

    void SyncPlotWithDialogue(Plot plot, Dialogue dialogue)
    {
        Dictionary<DialogueText, PlotItem> dialogueTextPlotItemMap = new Dictionary<DialogueText, PlotItem>();

        foreach (PlotItem plotItem in plot.plotItems)
        {
            if (plotItem.dialogueText != null && !dialogueTextPlotItemMap.ContainsKey(plotItem.dialogueText))
            {
                dialogueTextPlotItemMap.Add(plotItem.dialogueText, plotItem);
            }
        }

        plot.plotItems.Clear();

        List<DialogueText> dialogueTexts = dialogue.GetDialogueTexts();
        foreach (DialogueText dialogueText in dialogueTexts)
        {
            if (dialogueTextPlotItemMap.ContainsKey(dialogueText))
            {
                plot.plotItems.Add(dialogueTextPlotItemMap[dialogueText]);
            }
            else
            {
                plot.plotItems.Add(new PlotItem() {
                    dialogueText = dialogueText
                });
            }
        }
    }
}

}
