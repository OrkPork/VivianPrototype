namespace Entitas {
    public partial class Entity {
        static readonly OverWorldComponent overWorldComponent = new OverWorldComponent();

        public bool isOverWorld {
            get { return HasComponent(ComponentIds.OverWorld); }
            set {
                if (value != isOverWorld) {
                    if (value) {
                        AddComponent(ComponentIds.OverWorld, overWorldComponent);
                    } else {
                        RemoveComponent(ComponentIds.OverWorld);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherOverWorld;

        public static AllOfMatcher OverWorld {
            get {
                if (_matcherOverWorld == null) {
                    _matcherOverWorld = Matcher.AllOf(new [] { ComponentIds.OverWorld });
                }

                return _matcherOverWorld;
            }
        }
    }
}