namespace Entitas {
    public partial class Entity {
        public CharInventoryComponent charInventory { get { return (CharInventoryComponent)GetComponent(ComponentIds.CharInventory); } }

        public bool hasCharInventory { get { return HasComponent(ComponentIds.CharInventory); } }

        public void AddCharInventory(CharInventoryComponent component) {
            AddComponent(ComponentIds.CharInventory, component);
        }

        public void AddCharInventory(System.Collections.Generic.List<string> newInv) {
            var component = new CharInventoryComponent();
            component.inv = newInv;
            AddCharInventory(component);
        }

        public void ReplaceCharInventory(System.Collections.Generic.List<string> newInv) {
            CharInventoryComponent component;
            if (hasCharInventory) {
                WillRemoveComponent(ComponentIds.CharInventory);
                component = charInventory;
            } else {
                component = new CharInventoryComponent();
            }
            component.inv = newInv;
            ReplaceComponent(ComponentIds.CharInventory, component);
        }

        public void RemoveCharInventory() {
            RemoveComponent(ComponentIds.CharInventory);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherCharInventory;

        public static AllOfMatcher CharInventory {
            get {
                if (_matcherCharInventory == null) {
                    _matcherCharInventory = Matcher.AllOf(new [] { ComponentIds.CharInventory });
                }

                return _matcherCharInventory;
            }
        }
    }
}