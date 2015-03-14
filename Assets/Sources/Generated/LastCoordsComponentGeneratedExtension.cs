namespace Entitas {
    public partial class Entity {
        public LastCoordsComponent lastCoords { get { return (LastCoordsComponent)GetComponent(ComponentIds.LastCoords); } }

        public bool hasLastCoords { get { return HasComponent(ComponentIds.LastCoords); } }

        public void AddLastCoords(LastCoordsComponent component) {
            AddComponent(ComponentIds.LastCoords, component);
        }

        public void AddLastCoords(float newX, float newY, float newZ) {
            var component = new LastCoordsComponent();
            component.x = newX;
            component.y = newY;
            component.z = newZ;
            AddLastCoords(component);
        }

        public void ReplaceLastCoords(float newX, float newY, float newZ) {
            LastCoordsComponent component;
            if (hasLastCoords) {
                WillRemoveComponent(ComponentIds.LastCoords);
                component = lastCoords;
            } else {
                component = new LastCoordsComponent();
            }
            component.x = newX;
            component.y = newY;
            component.z = newZ;
            ReplaceComponent(ComponentIds.LastCoords, component);
        }

        public void RemoveLastCoords() {
            RemoveComponent(ComponentIds.LastCoords);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherLastCoords;

        public static AllOfMatcher LastCoords {
            get {
                if (_matcherLastCoords == null) {
                    _matcherLastCoords = Matcher.AllOf(new [] { ComponentIds.LastCoords });
                }

                return _matcherLastCoords;
            }
        }
    }
}