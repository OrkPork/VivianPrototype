using Entitas;
using UnityEngine;

public class RenderDespawnSystem : ReactiveSystem
{
    public AllOfMatcher GetTriggeringMatcher() {
        return Matcher.View;
    }

    public void Execute(Entity[] entities)
    {
        foreach (var e in pairs) {
            var view = (ViewComponent)pair.component;
            Object.Destroy(view.gameObject);
        }
    }
}

