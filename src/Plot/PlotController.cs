using System;
using System.Collections.Generic;
using UnityEngine;

using Chankiyu22.DialogueSystem.Dialogues;
using Chankiyu22.DialogueSystem.Characters;
using Chankiyu22.DialogueSystem.Avatars;

namespace Chankiyu22.DialogueSystem.Plots
{

public class PlotController : MonoBehaviour
{
    [SerializeField]
    private Plot m_plot = null;

    private Dictionary<DialogueText, PlotItem> m_map = new Dictionary<DialogueText, PlotItem>();

    [SerializeField]
    private DialogueTextUnityEvent m_OnDialogueText = null;

    public event EventHandler<DialogueTextEventArgs> OnDialogueText;

    [SerializeField]
    private CharacterUnityEvent m_OnCharacter = null;

    public event EventHandler<CharacterEventArgs> OnCharacter;

    [SerializeField]
    private AvatarTextureSourceUnityEvent m_OnAvatarTextureSource = null;

    public event EventHandler<AvatarTextureSourceEventArgs> OnAvatarTextureSource;

    [SerializeField]
    private PlotAdditionalDataListUnityEvent m_OnPlotAdditionalDataList = null;

    public event EventHandler<PlotAdditionalDataListEventArgs> OnPlotAdditionalDataList;

    void Awake()
    {
        m_map.Clear();
        foreach (PlotItem plotItem in m_plot.plotItems)
        {
            m_map.Add(plotItem.dialogueText, plotItem);
        }
    }

    public void Dispatch(DialogueText dialogueText)
    {
        if (m_map.ContainsKey(dialogueText))
        {
            PlotItem plotItem = m_map[dialogueText];
            EmitDialogueText(plotItem.dialogueText);
            EmitCharacter(plotItem.character);
            EmitAvatarTextureSource(plotItem.avatarTextureSource);
            EmitPlotAdditionalDataList(plotItem.additionalDataList);
        }
    }

    void EmitDialogueText(DialogueText dialogueText)
    {
        m_OnDialogueText.Invoke(dialogueText);
        if (OnDialogueText != null)
        {
            OnDialogueText.Invoke(this, new DialogueTextEventArgs() {
                dialogueText = dialogueText
            });
        }
    }

    void EmitCharacter(Character character)
    {
        m_OnCharacter.Invoke(character);
        if (OnCharacter != null)
        {
            OnCharacter.Invoke(this, new CharacterEventArgs() {
                character = character
            });
        }
    }

    void EmitAvatarTextureSource(AvatarTextureSource avatarTextureSource)
    {
        m_OnAvatarTextureSource.Invoke(avatarTextureSource);
        if (OnAvatarTextureSource != null)
        {
            OnAvatarTextureSource.Invoke(this, new AvatarTextureSourceEventArgs() {
                avatarTextureSource = avatarTextureSource
            });
        }
    }

    void EmitPlotAdditionalDataList(List<PlotAdditionalData> additionalDataList)
    {
        m_OnPlotAdditionalDataList.Invoke(additionalDataList);
        if (OnPlotAdditionalDataList != null)
        {
            OnPlotAdditionalDataList.Invoke(this, new PlotAdditionalDataListEventArgs() {
                additionalDataList = additionalDataList
            });
        }
    }
}

}
