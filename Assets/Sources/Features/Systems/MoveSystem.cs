using Entitas;
using UnityEngine;

public class MoveSystem : IExecuteSystem, ISetPool {
    Group _collection;

    public void SetPool(Pool repo) {
        _collection = repo.GetGroup(Matcher.AllOf(Matcher.Move, Matcher.Position));
    }

    public void Execute() {
        foreach (var e in _collection.GetEntities()) {
            var move = e.move.moveSPD;
            var pos = e.position.myPos;
            e.ReplacePosition(new Vector3 (pos.x + move.x, pos.y + move.y, pos.z + move.z));
        }
    }
}

