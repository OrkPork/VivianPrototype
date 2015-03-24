namespace Entitas {
    public partial class Entity {
        static readonly MyTurnComponent myTurnComponent = new MyTurnComponent();

        public bool isMyTurn {
            get { return HasComponent(ComponentIds.MyTurn); }
            set {
                if (value != isMyTurn) {
                    if (value) {
                        AddComponent(ComponentIds.MyTurn, myTurnComponent);
                    } else {
                        RemoveComponent(ComponentIds.MyTurn);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherMyTurn;

        public static AllOfMatcher MyTurn {
            get {
                if (_matcherMyTurn == null) {
                    _matcherMyTurn = Matcher.AllOf(new [] { ComponentIds.MyTurn });
                }

                return _matcherMyTurn;
            }
        }
    }
}