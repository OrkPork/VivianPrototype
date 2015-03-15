using Entitas;
using System.Collections.Generic;
using Entitas.CodeGenerator;

[SingleEntity]
public class InputStateComponent : IComponent
{
    //Variables go here
    public List<List<bool>> isUp;
    public List<List<bool>> isDown;
    public List<List<bool>> isTapped;
    public List<List<float>> axisValue;
}
