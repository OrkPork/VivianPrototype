namespace Entitas {
    public partial class Entity {
        public PositionComponent position { get { return (PositionComponent)GetComponent(ComponentIds.Position); } }

        public bool hasPosition { get { return HasComponent(ComponentIds.Position); } }

        public void AddPosition(PositionComponent component) {
            AddComponent(ComponentIds.Position, component);
        }

        public void AddPosition(UnityEngine.Vector3 newMyPos) {
            var component = new PositionComponent();
            component.myPos = newMyPos;
            AddPosition(component);
        }

        public void ReplacePosition(UnityEngine.Vector3 newMyPos) {
            PositionComponent component;
            if (hasPosition) {
                WillRemoveComponent(ComponentIds.Position);
                component = position;
            } else {
                component = new PositionComponent();
            }
            component.myPos = newMyPos;
            ReplaceComponent(ComponentIds.Position, component);
        }

        public void RemovePosition() {
            RemoveComponent(ComponentIds.Position);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherPosition;

        public static AllOfMatcher Position {
            get {
                if (_matcherPosition == null) {
                    _matcherPosition = Matcher.AllOf(new [] { ComponentIds.Position });
                }

                return _matcherPosition;
            }
        }
    }
}