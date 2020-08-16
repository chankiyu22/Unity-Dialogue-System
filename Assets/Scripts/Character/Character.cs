using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Characters
{

[Serializable]
public class CharacterData
{
    [SerializeField]
    private CharacterDataKey m_dataKey = null;

    public CharacterDataKey dataKey
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

[CreateAssetMenu(menuName="Dialogue System/Characters/Character")]
public class Character : ScriptableObject
{
    [SerializeField]
    private List<CharacterData> m_dataList = new List<CharacterData>();

    public List<CharacterData> dataList
    {
        get
        {
            return m_dataList;
        }
    }

    [SerializeField]
    private string m_description = null;

    public string description
    {
        get
        {
            return m_description;
        }
    }
}

}
