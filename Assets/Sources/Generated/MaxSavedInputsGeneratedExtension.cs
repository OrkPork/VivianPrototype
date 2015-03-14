namespace Entitas {
    public partial class Entity {
        public MaxSavedInputs maxSavedInputs { get { return (MaxSavedInputs)GetComponent(ComponentIds.MaxSavedInputs); } }

        public bool hasMaxSavedInputs { get { return HasComponent(ComponentIds.MaxSavedInputs); } }

        public void AddMaxSavedInputs(MaxSavedInputs component) {
            AddComponent(ComponentIds.MaxSavedInputs, component);
        }

        public void AddMaxSavedInputs(int newMaxInputs) {
            var component = new MaxSavedInputs();
            component.maxInputs = newMaxInputs;
            AddMaxSavedInputs(component);
        }

        public void ReplaceMaxSavedInputs(int newMaxInputs) {
            MaxSavedInputs component;
            if (hasMaxSavedInputs) {
                WillRemoveComponent(ComponentIds.MaxSavedInputs);
                component = maxSavedInputs;
            } else {
                component = new MaxSavedInputs();
            }
            component.maxInputs = newMaxInputs;
            ReplaceComponent(ComponentIds.MaxSavedInputs, component);
        }

        public void RemoveMaxSavedInputs() {
            RemoveComponent(ComponentIds.MaxSavedInputs);
        }
    }

    public partial class Pool {
        public Entity maxSavedInputsEntity { get { return GetGroup(Matcher.MaxSavedInputs).GetSingleEntity(); } }

        public MaxSavedInputs maxSavedInputs { get { return maxSavedInputsEntity.maxSavedInputs; } }

        public bool hasMaxSavedInputs { get { return maxSavedInputsEntity != null; } }

        public Entity SetMaxSavedInputs(MaxSavedInputs component) {
            if (hasMaxSavedInputs) {
                throw new SingleEntityException(Matcher.MaxSavedInputs);
            }
            var entity = CreateEntity();
            entity.AddMaxSavedInputs(component);
            return entity;
        }

        public Entity SetMaxSavedInputs(int newMaxInputs) {
            if (hasMaxSavedInputs) {
                throw new SingleEntityException(Matcher.MaxSavedInputs);
            }
            var entity = CreateEntity();
            entity.AddMaxSavedInputs(newMaxInputs);
            return entity;
        }

        public Entity ReplaceMaxSavedInputs(int newMaxInputs) {
            var entity = maxSavedInputsEntity;
            if (entity == null) {
                entity = SetMaxSavedInputs(newMaxInputs);
            } else {
                entity.ReplaceMaxSavedInputs(newMaxInputs);
            }

            return entity;
        }

        public void RemoveMaxSavedInputs() {
            DestroyEntity(maxSavedInputsEntity);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherMaxSavedInputs;

        public static AllOfMatcher MaxSavedInputs {
            get {
                if (_matcherMaxSavedInputs == null) {
                    _matcherMaxSavedInputs = Matcher.AllOf(new [] { ComponentIds.MaxSavedInputs });
                }

                return _matcherMaxSavedInputs;
            }
        }
    }
}