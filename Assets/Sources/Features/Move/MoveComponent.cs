using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class MoveComponent : IComponent {
    public float hSpeed;
    public float vSpeed;
}

