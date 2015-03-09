using Entitas;

public class DestroySystem : IExecuteSystem, ISetPool {
    Pool _repo;
    Group _collection;

    public void SetPool(Pool repo) {
        _repo = repo;
        _collection = repo.GetGroup(Matcher.Destroy);
    }

    public void Execute() {
        foreach (var e in _collection.GetEntities()) {
            _repo.DestroyEntity(e);
        }
    }
}

