namespace Entitas {
    public partial class Entity {
        public AccelerationComponent acceleration { get { return (AccelerationComponent)GetComponent(ComponentIds.Acceleration); } }

        public bool hasAcceleration { get { return HasComponent(ComponentIds.Acceleration); } }

        public void AddAcceleration(AccelerationComponent component) {
            AddComponent(ComponentIds.Acceleration, component);
        }

        public void AddAcceleration(UnityEngine.Vector3 newAccBy, UnityEngine.Vector3 newAccCaps) {
            var component = new AccelerationComponent();
            component.accBy = newAccBy;
            component.accCaps = newAccCaps;
            AddAcceleration(component);
        }

        public void ReplaceAcceleration(UnityEngine.Vector3 newAccBy, UnityEngine.Vector3 newAccCaps) {
            AccelerationComponent component;
            if (hasAcceleration) {
                WillRemoveComponent(ComponentIds.Acceleration);
                component = acceleration;
            } else {
                component = new AccelerationComponent();
            }
            component.accBy = newAccBy;
            component.accCaps = newAccCaps;
            ReplaceComponent(ComponentIds.Acceleration, component);
        }

        public void RemoveAcceleration() {
            RemoveComponent(ComponentIds.Acceleration);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherAcceleration;

        public static AllOfMatcher Acceleration {
            get {
                if (_matcherAcceleration == null) {
                    _matcherAcceleration = Matcher.AllOf(new [] { ComponentIds.Acceleration });
                }

                return _matcherAcceleration;
            }
        }
    }
}