namespace Entitas {
    public partial class Entity {
        public ObjectRefComponent objectRef { get { return (ObjectRefComponent)GetComponent(ComponentIds.ObjectRef); } }

        public bool hasObjectRef { get { return HasComponent(ComponentIds.ObjectRef); } }

        public void AddObjectRef(ObjectRefComponent component) {
            AddComponent(ComponentIds.ObjectRef, component);
        }

        public void AddObjectRef(UnityEngine.GameObject newGameObject) {
            var component = new ObjectRefComponent();
            component.gameObject = newGameObject;
            AddObjectRef(component);
        }

        public void ReplaceObjectRef(UnityEngine.GameObject newGameObject) {
            ObjectRefComponent component;
            if (hasObjectRef) {
                WillRemoveComponent(ComponentIds.ObjectRef);
                component = objectRef;
            } else {
                component = new ObjectRefComponent();
            }
            component.gameObject = newGameObject;
            ReplaceComponent(ComponentIds.ObjectRef, component);
        }

        public void RemoveObjectRef() {
            RemoveComponent(ComponentIds.ObjectRef);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherObjectRef;

        public static AllOfMatcher ObjectRef {
            get {
                if (_matcherObjectRef == null) {
                    _matcherObjectRef = Matcher.AllOf(new [] { ComponentIds.ObjectRef });
                }

                return _matcherObjectRef;
            }
        }
    }
}