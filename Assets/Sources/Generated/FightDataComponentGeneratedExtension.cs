namespace Entitas {
    public partial class Entity {
        public FightDataComponent fightData { get { return (FightDataComponent)GetComponent(ComponentIds.FightData); } }

        public bool hasFightData { get { return HasComponent(ComponentIds.FightData); } }

        public void AddFightData(FightDataComponent component) {
            AddComponent(ComponentIds.FightData, component);
        }

        public void AddFightData(int newFightLVL, int newCurrentHP, int newCurrentMP, int newMaxHP, int newMaxMP) {
            var component = new FightDataComponent();
            component.fightLVL = newFightLVL;
            component.currentHP = newCurrentHP;
            component.currentMP = newCurrentMP;
            component.maxHP = newMaxHP;
            component.maxMP = newMaxMP;
            AddFightData(component);
        }

        public void ReplaceFightData(int newFightLVL, int newCurrentHP, int newCurrentMP, int newMaxHP, int newMaxMP) {
            FightDataComponent component;
            if (hasFightData) {
                WillRemoveComponent(ComponentIds.FightData);
                component = fightData;
            } else {
                component = new FightDataComponent();
            }
            component.fightLVL = newFightLVL;
            component.currentHP = newCurrentHP;
            component.currentMP = newCurrentMP;
            component.maxHP = newMaxHP;
            component.maxMP = newMaxMP;
            ReplaceComponent(ComponentIds.FightData, component);
        }

        public void RemoveFightData() {
            RemoveComponent(ComponentIds.FightData);
        }
    }

    public static partial class Matcher {
        static AllOfEntityMatcher _matcherFightData;

        public static AllOfEntityMatcher FightData {
            get {
                if (_matcherFightData == null) {
                    _matcherFightData = Matcher.AllOf(new [] { ComponentIds.FightData });
                }

                return _matcherFightData;
            }
        }
    }
}
