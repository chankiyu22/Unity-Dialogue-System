using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

[Serializable]
public class PlotAdditionalDataKeyUnityEvent
{
    [SerializeField]
    private PlotAdditionalDataKey m_dataKey = null;

    public PlotAdditionalDataKey dataKey
    {
        get
        {
            return m_dataKey;
        }
    }

    [SerializeField]
    private UnityIntEvent m_intEvent = null;

    [SerializeField]
    private UnityFloatEvent m_floatEvent = null;

    [SerializeField]
    private UnityBoolEvent m_boolEvent = null;

    [SerializeField]
    private UnityStringEvent m_stringEvent = null;

    [SerializeField]
    private UnityVoidEvent m_voidEvent = null;

    public void Dispatch(int intValue)
    {
        m_intEvent.Invoke(intValue);
    }

    public void Dispatch(float floatValue)
    {
        m_floatEvent.Invoke(floatValue);
    }

    public void Dispatch(bool boolValue)
    {
        m_boolEvent.Invoke(boolValue);
    }

    public void Dispatch(string stringValue)
    {
        m_stringEvent.Invoke(stringValue);
    }

    public void Dispatch()
    {
        m_voidEvent.Invoke();
    }
}

public class PlotAdditionalDataDispatcher : MonoBehaviour
{
    [SerializeField]
    private List<PlotAdditionalDataKeyUnityEvent> m_dataKeyEvents = new List<PlotAdditionalDataKeyUnityEvent>();

    private Dictionary<PlotAdditionalDataKey, PlotAdditionalDataKeyUnityEvent> m_map = new Dictionary<PlotAdditionalDataKey, PlotAdditionalDataKeyUnityEvent>();

    void Awake()
    {
        foreach (PlotAdditionalDataKeyUnityEvent m_dataKeyEvent in m_dataKeyEvents)
        {
            m_map.Add(m_dataKeyEvent.dataKey, m_dataKeyEvent);
        }
    }

    public void Dispatch(List<PlotAdditionalData> additionalDataList)
    {
        foreach (PlotAdditionalData data in additionalDataList)
        {
            PlotAdditionalDataKey dataKey = data.dataKey;
            if (m_map.ContainsKey(dataKey))
            {
                PlotAdditionalDataKeyUnityEvent e = m_map[dataKey];
                switch (dataKey.GetKeyType())
                {
                    case DataKeyType.INTEGER:
                        e.Dispatch(data.intValue);
                        break;
                    case DataKeyType.FLOAT:
                        e.Dispatch(data.floatValue);
                        break;
                    case DataKeyType.BOOLEAN:
                        e.Dispatch(data.boolValue);
                        break;
                    case DataKeyType.STRING:
                        e.Dispatch(data.stringValue);
                        break;
                    case DataKeyType.VOID:
                        e.Dispatch();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

}
