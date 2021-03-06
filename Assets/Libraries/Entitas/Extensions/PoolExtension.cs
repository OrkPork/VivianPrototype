﻿using System;

namespace Entitas {

    public interface ISetPool {
        void SetPool(Pool pool);
    }

    public static class PoolExtension {
        public static Entity[] GetEntities(this Pool pool, IMatcher matcher) {
            return pool.GetGroup(matcher).GetEntities();
        }

        public static IExecuteSystem CreateSystem<T>(this Pool pool) where T: ISystem, new() {
            return pool.CreateSystem(typeof(T));
        }

        public static IExecuteSystem CreateSystem(this Pool pool, Type systemType) {
            var system = (ISystem)Activator.CreateInstance(systemType);

            var setPool = system as ISetPool;
            if (setPool != null) setPool.SetPool(pool);

            var executeSystem = system as IExecuteSystem;
            if (executeSystem != null) return (IExecuteSystem)system;

            var reactiveSystem = system as IReactiveSystem;
            if (reactiveSystem != null) return new ReactiveSystem(pool, reactiveSystem);

            return null;
        }
    }
}

