using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class SavedInputsComponent : IComponent
{
    public List<List<KeyCode>> commandButton;
    public List<List<string>> commandAxis;
}
