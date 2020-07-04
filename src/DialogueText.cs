using UnityEngine;

namespace Chankiyu22.DialogueSystem
{

[CreateAssetMenu(menuName="Dialogue System/Dialogue Text")]
public class DialogueText : ScriptableObject
{
    [TextArea(3, 3)]
    [SerializeField]
    private string m_text;

    public string text
    {
        get
        {
            return m_text;
        }

        set
        {
            m_text = value;
        }
    }
}
    
}
