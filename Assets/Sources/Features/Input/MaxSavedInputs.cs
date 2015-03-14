using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class MaxSavedInputs : IComponent
{
    public int maxInputs = 5;
}
