using Entitas;
using UnityEngine;

public class RenderPositionSystem : IReactiveSystem {
    public IMatcher GetTriggeringMatcher() {
        //     position
        return Matcher.AllOf(Matcher.ObjectRef, Matcher.Position);
    }

    public GroupEventType GetEventType() {
        //GroupEventType.
        return GroupEventType.OnEntityAdded;
    }

    public void Execute(Entity[] entities) {
        foreach (var e in entities) {
            e.objectRef.gameObject.transform.position = e.position.myPos;
        }
    }
}

