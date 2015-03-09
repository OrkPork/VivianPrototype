using Entitas;

public static class PoolExtensions {

    public static IExecuteSystem CreateSystem<T>(this Pool repo) where T: new() {
        var system = new T();
        if (system is ISetPool) {
            ((ISetPool)system).SetPool(repo);
        }
        if (system is IExecuteSystem) {
            return (IExecuteSystem)system;
        }
        if (system is IReactiveSystem) {
            return new ReactiveSystem(repo, (IReactiveSystem)system);
        }
        if (system is IReactiveSubEntityWillBeRemovedSystem) {
            return new ReactiveEntityWillBeRemovedSystem(repo, (IReactiveSubEntityWillBeRemovedSystem)system);
        }

        return null;
    }
}

public interface ISetPool {
    void SetPool(Pool repo);
}
