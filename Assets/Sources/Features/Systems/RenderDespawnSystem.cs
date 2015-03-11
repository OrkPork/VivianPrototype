using Entitas;
using UnityEngine;

public class RenderDespawnSystem : IReactiveSystem
{
    public IMatcher GetTriggeringMatcher() {
        return Matcher.View;
    }

    public GroupEventType GetEventType()
    {
        return GroupEventType.OnEntityRemoved;
    }

    public void Execute(Entity[] entities)
    {
        foreach (var e in entities) {
            var view = e.view;
            Object.Destroy(view.gameObject);
        }
    }
}

