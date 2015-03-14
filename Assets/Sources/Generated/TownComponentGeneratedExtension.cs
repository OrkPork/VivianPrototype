namespace Entitas {
    public partial class Entity {
        static readonly TownComponent townComponent = new TownComponent();

        public bool isTown {
            get { return HasComponent(ComponentIds.Town); }
            set {
                if (value != isTown) {
                    if (value) {
                        AddComponent(ComponentIds.Town, townComponent);
                    } else {
                        RemoveComponent(ComponentIds.Town);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherTown;

        public static AllOfMatcher Town {
            get {
                if (_matcherTown == null) {
                    _matcherTown = Matcher.AllOf(new [] { ComponentIds.Town });
                }

                return _matcherTown;
            }
        }
    }
}