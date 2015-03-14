namespace Entitas {
    public partial class Entity {
        static readonly FreeToMoveComponent freeToMoveComponent = new FreeToMoveComponent();

        public bool isFreeToMove {
            get { return HasComponent(ComponentIds.FreeToMove); }
            set {
                if (value != isFreeToMove) {
                    if (value) {
                        AddComponent(ComponentIds.FreeToMove, freeToMoveComponent);
                    } else {
                        RemoveComponent(ComponentIds.FreeToMove);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherFreeToMove;

        public static AllOfMatcher FreeToMove {
            get {
                if (_matcherFreeToMove == null) {
                    _matcherFreeToMove = Matcher.AllOf(new [] { ComponentIds.FreeToMove });
                }

                return _matcherFreeToMove;
            }
        }
    }
}