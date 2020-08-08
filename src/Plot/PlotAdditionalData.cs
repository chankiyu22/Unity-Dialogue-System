using System;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Plots
{

[Serializable]
public class PlotAdditionalData
{
    [SerializeField]
    private PlotAdditionalDataKey m_dataKey = null;

    public PlotAdditionalDataKey dataKey
    {
        get
        {
            return m_dataKey;
        }

        set
        {
            m_dataKey = value;
        }
    }

    [SerializeField]
    private int m_intValue;

    public int intValue
    {
        get
        {
            return m_intValue;
        }

        set
        {
            m_intValue = value;
        }
    }

    [SerializeField]
    private float m_floatValue;

    public float floatValue
    {
        get
        {
            return m_floatValue;
        }

        set
        {
            m_floatValue = value;
        }
    }

    [SerializeField]
    private bool m_boolValue;

    public bool boolValue
    {
        get
        {
            return m_boolValue;
        }

        set
        {
            m_boolValue = value;
        }
    }

    [SerializeField]
    private string m_stringValue;

    public string stringValue
    {
        get
        {
            return m_stringValue;
        }

        set
        {
            m_stringValue = value;
        }
    }
}

}
