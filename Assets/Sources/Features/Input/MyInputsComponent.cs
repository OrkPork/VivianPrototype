using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;
using UnityEngine;

[SingleEntity]
public class MyInputsComponent : IComponent
{

    //Name for command, button that activates command
    public List<string> commandNames;
    public List<List<KeyCode>> commandButton;
    public List<List<string>> commandAxis;
    public Dictionary<string, string> commands;

}
