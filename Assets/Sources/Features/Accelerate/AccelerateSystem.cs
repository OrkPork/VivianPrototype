using Entitas;

public class AccelerateSystem : IReactiveSystem, ISetPool {
    public IMatcher GetTriggeringMatcher() {
        return Matcher.Accelerate;
    }

    public GroupEventType GetEventType() {
        return GroupEventType.OnEntityAddedOrRemoved;
    }

    Group _collection;

    public void SetPool(Pool repo) {
        _collection = repo.GetGroup(Matcher.AllOf(Matcher.Acceleratable, Matcher.Move));
    }

    public void Execute(Entity[] entities) {
        var accelerate = entities.SingleEntity().isAccelerate;
        foreach (var e in _collection.GetEntities()) {
            var move = e.move;
            //acceleration calculation, currently just jumps to max speed, zero if cannot accelerate
            var speed = accelerate ? move.maxSpeed : 0;
            e.ReplaceMove(speed, move.maxSpeed);
        }
    }
}

