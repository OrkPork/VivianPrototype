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
    public List<List<bool>> isUp;
    public List<List<bool>> isHeld;
    public List<List<bool>> isDown;
    public List<List<float>> axisValue;
    public List<bool> buttonAxis;

}
