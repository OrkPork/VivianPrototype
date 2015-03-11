namespace Entitas {
    public partial class Entity {
        public AccelerationComponent acceleration { get { return (AccelerationComponent)GetComponent(ComponentIds.Acceleration); } }

        public bool hasAcceleration { get { return HasComponent(ComponentIds.Acceleration); } }

        public void AddAcceleration(AccelerationComponent component) {
            AddComponent(ComponentIds.Acceleration, component);
        }

        public void AddAcceleration(float newXInc, float newYInc, float newZInc, float newXMax, float newYMax, float newZMax) {
            var component = new AccelerationComponent();
            component.xInc = newXInc;
            component.yInc = newYInc;
            component.zInc = newZInc;
            component.xMax = newXMax;
            component.yMax = newYMax;
            component.zMax = newZMax;
            AddAcceleration(component);
        }

        public void ReplaceAcceleration(float newXInc, float newYInc, float newZInc, float newXMax, float newYMax, float newZMax) {
            AccelerationComponent component;
            if (hasAcceleration) {
                WillRemoveComponent(ComponentIds.Acceleration);
                component = acceleration;
            } else {
                component = new AccelerationComponent();
            }
            component.xInc = newXInc;
            component.yInc = newYInc;
            component.zInc = newZInc;
            component.xMax = newXMax;
            component.yMax = newYMax;
            component.zMax = newZMax;
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