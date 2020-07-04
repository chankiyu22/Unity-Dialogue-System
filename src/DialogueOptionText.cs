using UnityEngine;

namespace Chankiyu22.DialogueSystem
{

[CreateAssetMenu(menuName="Dialogue System/Dialogue Option Text")]
public class DialogueOptionText : ScriptableObject
{
    [SerializeField]
    private string m_text = null;

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
