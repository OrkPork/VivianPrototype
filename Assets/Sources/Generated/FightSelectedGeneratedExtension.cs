namespace Entitas {
    public partial class Entity {
        static readonly FightSelected fightSelectedComponent = new FightSelected();

        public bool isFightSelected {
            get { return HasComponent(ComponentIds.FightSelected); }
            set {
                if (value != isFightSelected) {
                    if (value) {
                        AddComponent(ComponentIds.FightSelected, fightSelectedComponent);
                    } else {
                        RemoveComponent(ComponentIds.FightSelected);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfEntityMatcher _matcherFightSelected;

        public static AllOfEntityMatcher FightSelected {
            get {
                if (_matcherFightSelected == null) {
                    _matcherFightSelected = Matcher.AllOf(new [] { ComponentIds.FightSelected });
                }

                return _matcherFightSelected;
            }
        }
    }
}
