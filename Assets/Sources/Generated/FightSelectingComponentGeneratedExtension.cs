namespace Entitas {
    public partial class Entity {
        static readonly FightSelectingComponent fightSelectingComponent = new FightSelectingComponent();

        public bool isFightSelecting {
            get { return HasComponent(ComponentIds.FightSelecting); }
            set {
                if (value != isFightSelecting) {
                    if (value) {
                        AddComponent(ComponentIds.FightSelecting, fightSelectingComponent);
                    } else {
                        RemoveComponent(ComponentIds.FightSelecting);
                    }
                }
            }
        }
    }

    public partial class Pool {
        public Entity fightSelectingEntity { get { return GetGroup(Matcher.FightSelecting).GetSingleEntity(); } }

        public bool isFightSelecting {
            get { return fightSelectingEntity != null; }
            set {
                var entity = fightSelectingEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isFightSelecting = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherFightSelecting;

        public static AllOfMatcher FightSelecting {
            get {
                if (_matcherFightSelecting == null) {
                    _matcherFightSelecting = Matcher.AllOf(new [] { ComponentIds.FightSelecting });
                }

                return _matcherFightSelecting;
            }
        }
    }
}