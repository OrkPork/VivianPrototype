using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class GameMapsComponent : IComponent
{
    //hardcoded list of game maps
    public List<string> mapTitles = new List<string> { "TestMap" };
}
