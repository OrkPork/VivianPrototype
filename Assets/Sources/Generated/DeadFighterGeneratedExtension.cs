namespace Entitas {
    public partial class Entity {
        static readonly DeadFighter deadFighterComponent = new DeadFighter();

        public bool isDeadFighter {
            get { return HasComponent(ComponentIds.DeadFighter); }
            set {
                if (value != isDeadFighter) {
                    if (value) {
                        AddComponent(ComponentIds.DeadFighter, deadFighterComponent);
                    } else {
                        RemoveComponent(ComponentIds.DeadFighter);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherDeadFighter;

        public static AllOfMatcher DeadFighter {
            get {
                if (_matcherDeadFighter == null) {
                    _matcherDeadFighter = Matcher.AllOf(new [] { ComponentIds.DeadFighter });
                }

                return _matcherDeadFighter;
            }
        }
    }
}