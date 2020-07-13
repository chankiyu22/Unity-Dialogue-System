using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Characters
{

[Serializable]
public class CharacterDataKeyEvent
{
    [SerializeField]
    private CharacterDataKey m_dataKey = null;

    public CharacterDataKey dataKey
    {
        get
        {
            return m_dataKey;
        }
    }

    [SerializeField]
    private IntUnityEvent m_OnIntEvents = null;

    public event EventHandler<IntEventArgs> OnIntEvents;

    [SerializeField]
    private FloatUnityEvent m_OnFloatEvents = null;

    public event EventHandler<FloatEventArgs> OnFloatEvents;

    [SerializeField]
    private BoolUnityEvent m_OnBoolEvents = null;

    public event EventHandler<BoolEventArgs> OnBoolEvents;

    [SerializeField]
    private StringUnityEvent m_OnStringEvents = null;

    public event EventHandler<StringEventArgs> OnStringEvents;

    public void Dispatch(int intValue)
    {
        m_OnIntEvents.Invoke(intValue);
        if (OnIntEvents != null)
        {
            OnIntEvents.Invoke(this, new IntEventArgs() {
                value = intValue
            });
        }
    }

    public void Dispatch(float floatValue)
    {
        m_OnFloatEvents.Invoke(floatValue);
        if (OnFloatEvents != null)
        {
            OnFloatEvents.Invoke(this, new FloatEventArgs() {
                value = floatValue
            });
        }
    }

    public void Dispatch(bool boolValue)
    {
        m_OnBoolEvents.Invoke(boolValue);
        if (OnBoolEvents != null)
        {
            OnBoolEvents.Invoke(this, new BoolEventArgs() {
                value = boolValue
            });
        }
    }

    public void Dispatch(string stringValue)
    {
        m_OnStringEvents.Invoke(stringValue);
        if (OnStringEvents != null)
        {
            OnStringEvents.Invoke(this, new StringEventArgs() {
                value = stringValue
            });
        }
    }
}

public class CharacterDataDispatcher : MonoBehaviour
{
    [SerializeField]
    private List<CharacterDataKeyEvent> m_characterDataKeyEventList = new List<CharacterDataKeyEvent>();

    private Dictionary<CharacterDataKey, CharacterDataKeyEvent> m_characterDataKeyEventMap = new Dictionary<CharacterDataKey, CharacterDataKeyEvent>();

    void Awake()
    {
        foreach (CharacterDataKeyEvent e in m_characterDataKeyEventList)
        {
            m_characterDataKeyEventMap.Add(e.dataKey, e);
        }
    }

    public void Dispatch(Character character)
    {
        foreach (CharacterData data in character.dataList)
        {
            CharacterDataKey dataKey = data.dataKey;
            if (m_characterDataKeyEventMap.ContainsKey(dataKey))
            {
                CharacterDataKeyEvent e = m_characterDataKeyEventMap[dataKey];
                switch (dataKey.GetKeyType())
                {
                    case DataKeyType.INTEGER:
                    {
                        e.Dispatch(data.intValue);
                        break;
                    }
                    case DataKeyType.FLOAT:
                    {
                        e.Dispatch(data.floatValue);
                        break;
                    }
                    case DataKeyType.BOOLEAN:
                    {
                        e.Dispatch(data.boolValue);
                        break;
                    }
                    case DataKeyType.STRING:
                    {
                        e.Dispatch(data.stringValue);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }
    }
}

}
