using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.DialogueSystem.Dialogues
{

public abstract class VariableAssignmentsController: MonoBehaviour
{
    public abstract List<VariableAssignment> GetVariableAssignments();
}

}
