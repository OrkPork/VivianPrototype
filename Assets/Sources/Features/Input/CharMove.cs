using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class CharMove : IComponent
{
    //Maintains User move modifier for traversing the environment, values are changed by road conditions etc
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
}
