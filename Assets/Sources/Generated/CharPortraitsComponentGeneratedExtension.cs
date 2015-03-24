namespace Entitas {
    public partial class Entity {
        public CharPortraitsComponent charPortraits { get { return (CharPortraitsComponent)GetComponent(ComponentIds.CharPortraits); } }

        public bool hasCharPortraits { get { return HasComponent(ComponentIds.CharPortraits); } }

        public void AddCharPortraits(CharPortraitsComponent component) {
            AddComponent(ComponentIds.CharPortraits, component);
        }

        public void AddCharPortraits(string newUI, string newMneu) {
            var component = new CharPortraitsComponent();
            component.UI = newUI;
            component.mneu = newMneu;
            AddCharPortraits(component);
        }

        public void ReplaceCharPortraits(string newUI, string newMneu) {
            CharPortraitsComponent component;
            if (hasCharPortraits) {
                WillRemoveComponent(ComponentIds.CharPortraits);
                component = charPortraits;
            } else {
                component = new CharPortraitsComponent();
            }
            component.UI = newUI;
            component.mneu = newMneu;
            ReplaceComponent(ComponentIds.CharPortraits, component);
        }

        public void RemoveCharPortraits() {
            RemoveComponent(ComponentIds.CharPortraits);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherCharPortraits;

        public static AllOfMatcher CharPortraits {
            get {
                if (_matcherCharPortraits == null) {
                    _matcherCharPortraits = Matcher.AllOf(new [] { ComponentIds.CharPortraits });
                }

                return _matcherCharPortraits;
            }
        }
    }
}