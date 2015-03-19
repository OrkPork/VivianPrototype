using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;
using UnityEngine;

[SingleEntity]
public class MyInputsComponent : IComponent
{


    /////CONSIDER: Turning the stateDictionary into an object instead of 4 key-value pairs of string-bool
    public Dictionary<int, string> inputNames; // for discovery
    //public Dictionary<string, buttonState>> inputState; //ex: if(inputState["Left"].isUp)
    public Dictionary<string, Dictionary<string, bool>> inputState; // ex: if(inputState["Left"]["isUp"]);   for reading
    public Dictionary<string, float> axisValue; // for reading
    public Dictionary<string, Dictionary<int, KeyCode>> myButtons; //for discovery
    public Dictionary<string, Dictionary<int, string>> myAxes; // for discovery


}
