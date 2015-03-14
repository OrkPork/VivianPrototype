using Entitas;
using UnityEngine;

public class RenderPositionSystem : IReactiveSystem {
    public IMatcher GetTriggeringMatcher() {
        //     position
        return Matcher.AllOf(Matcher.View, Matcher.Position);
    }

    public GroupEventType GetEventType() {
        return GroupEventType.OnEntityAdded;
    }

    public void Execute(Entity[] entities) {
        foreach (var e in entities) {
            //var pos = e.position.myPos;
            e.view.gameObject.transform.position = e.position.myPos;
        }
    }
}

