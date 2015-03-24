namespace Entitas {
    public partial class Entity {
        public InitiativePortraitComponent initiativePortrait { get { return (InitiativePortraitComponent)GetComponent(ComponentIds.InitiativePortrait); } }

        public bool hasInitiativePortrait { get { return HasComponent(ComponentIds.InitiativePortrait); } }

        public void AddInitiativePortrait(InitiativePortraitComponent component) {
            AddComponent(ComponentIds.InitiativePortrait, component);
        }

        public void AddInitiativePortrait(string newInitPortrait) {
            var component = new InitiativePortraitComponent();
            component.initPortrait = newInitPortrait;
            AddInitiativePortrait(component);
        }

        public void ReplaceInitiativePortrait(string newInitPortrait) {
            InitiativePortraitComponent component;
            if (hasInitiativePortrait) {
                WillRemoveComponent(ComponentIds.InitiativePortrait);
                component = initiativePortrait;
            } else {
                component = new InitiativePortraitComponent();
            }
            component.initPortrait = newInitPortrait;
            ReplaceComponent(ComponentIds.InitiativePortrait, component);
        }

        public void RemoveInitiativePortrait() {
            RemoveComponent(ComponentIds.InitiativePortrait);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherInitiativePortrait;

        public static AllOfMatcher InitiativePortrait {
            get {
                if (_matcherInitiativePortrait == null) {
                    _matcherInitiativePortrait = Matcher.AllOf(new [] { ComponentIds.InitiativePortrait });
                }

                return _matcherInitiativePortrait;
            }
        }
    }
}