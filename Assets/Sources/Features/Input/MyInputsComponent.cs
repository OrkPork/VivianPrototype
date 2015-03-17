using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;
using UnityEngine;

[SingleEntity]
public class MyInputsComponent : IComponent
{

    //commandNames should be a dictionary with the indice listed
    //so that a system can query where to look ex:
    //if(MyInputs.isUp[i][commandNames["Left"]])
    //...could just hardcode it to indices
    //...not sure if commandNames["Left"] is any faster than commandNames.
    //public Dictionary<string, Dictionary<string, bool>> inputState; // ex: inputState["Left"]["isUp"];
    //public Dictionary<string, Dictionary<int, KeyCode>> myButtons;
    public List<string> commandNames;
    public List<List<KeyCode>> commandButton;
    public List<List<string>> commandAxis;
    public List<List<bool>> isUp;
    public List<List<bool>> isHeld;
    public List<List<bool>> isDown;
    public List<List<float>> axisValue;
    public List<bool> buttonAxis;

}
