namespace Entitas {
    public partial class Entity {
        static readonly InputDetectedComponent inputDetectedComponent = new InputDetectedComponent();

        public bool isInputDetected {
            get { return HasComponent(ComponentIds.InputDetected); }
            set {
                if (value != isInputDetected) {
                    if (value) {
                        AddComponent(ComponentIds.InputDetected, inputDetectedComponent);
                    } else {
                        RemoveComponent(ComponentIds.InputDetected);
                    }
                }
            }
        }
    }

    public partial class Pool {
        public Entity inputDetectedEntity { get { return GetGroup(Matcher.InputDetected).GetSingleEntity(); } }

        public bool isInputDetected {
            get { return inputDetectedEntity != null; }
            set {
                var entity = inputDetectedEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isInputDetected = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherInputDetected;

        public static AllOfMatcher InputDetected {
            get {
                if (_matcherInputDetected == null) {
                    _matcherInputDetected = Matcher.AllOf(new [] { ComponentIds.InputDetected });
                }

                return _matcherInputDetected;
            }
        }
    }
}