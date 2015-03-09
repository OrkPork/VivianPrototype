namespace Entitas {
    public partial class Entity {
        static readonly FightSelecting fightSelectingComponent = new FightSelecting();

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

    public partial class EntityRepository {
        public Entity fightSelectingEntity { get { return GetCollection(Matcher.FightSelecting).GetSingleEntity(); } }

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
        static AllOfEntityMatcher _matcherFightSelecting;

        public static AllOfEntityMatcher FightSelecting {
            get {
                if (_matcherFightSelecting == null) {
                    _matcherFightSelecting = Matcher.AllOf(new [] { ComponentIds.FightSelecting });
                }

                return _matcherFightSelecting;
            }
        }
    }
}
