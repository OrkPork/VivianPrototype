namespace Entitas {
    public partial class Entity {
        static readonly SelectingComponent selectingComponent = new SelectingComponent();

        public bool isSelecting {
            get { return HasComponent(ComponentIds.Selecting); }
            set {
                if (value != isSelecting) {
                    if (value) {
                        AddComponent(ComponentIds.Selecting, selectingComponent);
                    } else {
                        RemoveComponent(ComponentIds.Selecting);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfEntityMatcher _matcherSelecting;

        public static AllOfEntityMatcher Selecting {
            get {
                if (_matcherSelecting == null) {
                    _matcherSelecting = Matcher.AllOf(new [] { ComponentIds.Selecting });
                }

                return _matcherSelecting;
            }
        }
    }
}
