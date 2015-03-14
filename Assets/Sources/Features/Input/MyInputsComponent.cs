using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class MyInputsComponent : IComponent
{
    public Dictionary<string, string> commands = new Dictionary<string, string> { 
        {"Left","Left" },
        {"Right", "Right"},
        {"Up", "Up"},
        {"Down", "Down"},
        {"Jump", "Jump"},
        {"Run","" },
        {"Roll", ""},
        {"Attack","" },
        {"Block","" },
        {"Items","" },
        {"Skills","" },
        {"Select","Submit" },
        {"Cancel","Cancel" },
        {"Settings","" },
        {"Menu","" },
        {"Inventory","" },
        {"Map","" },
        {"Save","" },
        {"Pause","" }
    };

}
