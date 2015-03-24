namespace Entitas {
    public partial class Entity {
        public InitiativeComponent initiative { get { return (InitiativeComponent)GetComponent(ComponentIds.Initiative); } }

        public bool hasInitiative { get { return HasComponent(ComponentIds.Initiative); } }

        public void AddInitiative(InitiativeComponent component) {
            AddComponent(ComponentIds.Initiative, component);
        }

        public void AddInitiative(int newValue) {
            var component = new InitiativeComponent();
            component.value = newValue;
            AddInitiative(component);
        }

        public void ReplaceInitiative(int newValue) {
            InitiativeComponent component;
            if (hasInitiative) {
                WillRemoveComponent(ComponentIds.Initiative);
                component = initiative;
            } else {
                component = new InitiativeComponent();
            }
            component.value = newValue;
            ReplaceComponent(ComponentIds.Initiative, component);
        }

        public void RemoveInitiative() {
            RemoveComponent(ComponentIds.Initiative);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherInitiative;

        public static AllOfMatcher Initiative {
            get {
                if (_matcherInitiative == null) {
                    _matcherInitiative = Matcher.AllOf(new [] { ComponentIds.Initiative });
                }

                return _matcherInitiative;
            }
        }
    }
}