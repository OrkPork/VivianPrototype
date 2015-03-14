using Entitas;
using UnityEngine;
using Entitas.CodeGenerator;

[SingleEntity]
public class CharMoveComponent : IComponent
{
    //Maintains User move modifier for traversing the environment, values are changed by road conditions etc
    public Vector3 moveSPD;
}
