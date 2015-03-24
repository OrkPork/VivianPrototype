using Entitas;
using UnityEngine;

public class RenderDespawnSystem : IReactiveSystem
{
    public IMatcher GetTriggeringMatcher() {
        return Matcher.ObjectRef;
        //return Matcher.Resource;
    }

    public GroupEventType GetEventType()
    {
        return GroupEventType.OnEntityRemoved;
    }

    public void Execute(Entity[] entities)
    {
        foreach (var e in entities) {
            //var view = e.view;
            var obj = e.objectRef;
            Object.Destroy(obj.gameObject);
        }
    }
}

