using Entitas;
using UnityEngine;
using Entitas.CodeGenerator;

[SingleEntity]
public class EncounterTriggerComponent : IComponent
{
    //Variables go here
    public string[] resourceTrigger;
    public string[] battleIDs;
}
