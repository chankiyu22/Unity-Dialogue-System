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

[Serializable]
public class DefaultCharacterDataKeyEvent : CharacterDataKeyEvent
{
    [SerializeField]
    private int m_defaultIntValue = 0;

    public int defaultIntValue
    {
        get
        {
            return m_defaultIntValue;
        }
    }

    [SerializeField]
    private float m_defaultFloatValue = 0;

    public float defaultFloatValue
    {
        get
        {
            return m_defaultFloatValue;
        }
    }

    [SerializeField]
    private bool m_defaultBoolValue = false;

    public bool defaultBoolValue
    {
        get
        {
            return m_defaultBoolValue;
        }
    }

    [SerializeField]
    private string m_defaultStringValue = null;

    public string defaultStringValue
    {
        get
        {
            return m_defaultStringValue;
        }
    }
}

public class CharacterDataDispatcher : MonoBehaviour
{
    [SerializeField]
    private List<CharacterDataKeyEvent> m_characterDataKeyEventList = new List<CharacterDataKeyEvent>();

    private Dictionary<CharacterDataKey, CharacterDataKeyEvent> m_characterDataKeyEventMap = new Dictionary<CharacterDataKey, CharacterDataKeyEvent>();

    [SerializeField]
    private List<DefaultCharacterDataKeyEvent> m_defaultCharacterDataKeyEventList = new List<DefaultCharacterDataKeyEvent>();

    void Awake()
    {
        foreach (CharacterDataKeyEvent e in m_characterDataKeyEventList)
        {
            m_characterDataKeyEventMap.Add(e.dataKey, e);
        }
    }

    public void Dispatch(Character character)
    {
        if (character != null)
        {
            DispatchCharacterDataList(character.dataList);
        }
        else
        {
            DispatchDefaultCharacterDataList();
        }
    }

    void DispatchCharacterDataList(List<CharacterData> characterDataList)
    {
        foreach (CharacterData data in characterDataList)
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

    void DispatchDefaultCharacterDataList()
    {
        foreach (DefaultCharacterDataKeyEvent e in m_defaultCharacterDataKeyEventList)
        {
            CharacterDataKey dataKey = e.dataKey;
            switch (dataKey.GetKeyType())
            {
                case DataKeyType.INTEGER:
                {
                    e.Dispatch(e.defaultIntValue);
                    break;
                }
                case DataKeyType.FLOAT:
                {
                    e.Dispatch(e.defaultFloatValue);
                    break;
                }
                case DataKeyType.BOOLEAN:
                {
                    e.Dispatch(e.defaultBoolValue);
                    break;
                }
                case DataKeyType.STRING:
                {
                    e.Dispatch(e.defaultStringValue);
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
