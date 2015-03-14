using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class MyInputsComponent : IComponent
{
    public Dictionary<string, string> commands;

}
