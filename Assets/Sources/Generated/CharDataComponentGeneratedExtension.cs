namespace Entitas {
    public partial class Entity {
        public CharDataComponent charData { get { return (CharDataComponent)GetComponent(ComponentIds.CharData); } }

        public bool hasCharData { get { return HasComponent(ComponentIds.CharData); } }

        public void AddCharData(CharDataComponent component) {
            AddComponent(ComponentIds.CharData, component);
        }

        public void AddCharData(string newName, int newLevel, int newHealthCap, int newManaCap, string newCharClass) {
            var component = new CharDataComponent();
            component.name = newName;
            component.level = newLevel;
            component.healthCap = newHealthCap;
            component.manaCap = newManaCap;
            component.charClass = newCharClass;
            AddCharData(component);
        }

        public void ReplaceCharData(string newName, int newLevel, int newHealthCap, int newManaCap, string newCharClass) {
            CharDataComponent component;
            if (hasCharData) {
                WillRemoveComponent(ComponentIds.CharData);
                component = charData;
            } else {
                component = new CharDataComponent();
            }
            component.name = newName;
            component.level = newLevel;
            component.healthCap = newHealthCap;
            component.manaCap = newManaCap;
            component.charClass = newCharClass;
            ReplaceComponent(ComponentIds.CharData, component);
        }

        public void RemoveCharData() {
            RemoveComponent(ComponentIds.CharData);
        }
    }

    public static partial class Matcher {
        static AllOfEntityMatcher _matcherCharData;

        public static AllOfEntityMatcher CharData {
            get {
                if (_matcherCharData == null) {
                    _matcherCharData = Matcher.AllOf(new [] { ComponentIds.CharData });
                }

                return _matcherCharData;
            }
        }
    }
}
