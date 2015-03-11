using Entitas;

public class MoveSystem : IExecuteSystem, ISetPool {
    Group _collection;

    public void SetPool(Pool repo) {
        _collection = repo.GetGroup(Matcher.AllOf(Matcher.Move, Matcher.Position));
    }

    public void Execute() {
        foreach (var e in _collection.GetEntities()) {
            var move = e.move;
            var pos = e.position;
            e.ReplacePosition(pos.x + move.xSpeed, pos.y + move.ySpeed, pos.z + move.zSpeed);
        }
    }
}

