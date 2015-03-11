namespace Entitas {
    public partial class Entity {
        static readonly TouchingComponent touchingComponent = new TouchingComponent();

        public bool isTouching {
            get { return HasComponent(ComponentIds.Touching); }
            set {
                if (value != isTouching) {
                    if (value) {
                        AddComponent(ComponentIds.Touching, touchingComponent);
                    } else {
                        RemoveComponent(ComponentIds.Touching);
                    }
                }
            }
        }
    }

    public partial class Pool {
        public Entity touchingEntity { get { return GetGroup(Matcher.Touching).GetSingleEntity(); } }

        public bool isTouching {
            get { return touchingEntity != null; }
            set {
                var entity = touchingEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isTouching = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherTouching;

        public static AllOfMatcher Touching {
            get {
                if (_matcherTouching == null) {
                    _matcherTouching = Matcher.AllOf(new [] { ComponentIds.Touching });
                }

                return _matcherTouching;
            }
        }
    }
}