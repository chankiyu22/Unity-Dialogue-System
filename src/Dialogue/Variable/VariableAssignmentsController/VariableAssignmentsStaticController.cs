using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public class VariableAssignmentsStaticController : VariableAssignmentsController
{
    [SerializeField]
    private List<VariableAssignment> m_variableAssignments = new List<VariableAssignment>();

    public List<VariableAssignment> variableAssignments
    {
        get
        {
            return m_variableAssignments;
        }
    }

    public override List<VariableAssignment> GetVariableAssignments()
    {
        return m_variableAssignments;
    }
}

}