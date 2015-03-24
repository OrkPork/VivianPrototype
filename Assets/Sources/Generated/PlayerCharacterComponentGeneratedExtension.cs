namespace Entitas {
    public partial class Entity {
        static readonly PlayerCharacterComponent playerCharacterComponent = new PlayerCharacterComponent();

        public bool isPlayerCharacter {
            get { return HasComponent(ComponentIds.PlayerCharacter); }
            set {
                if (value != isPlayerCharacter) {
                    if (value) {
                        AddComponent(ComponentIds.PlayerCharacter, playerCharacterComponent);
                    } else {
                        RemoveComponent(ComponentIds.PlayerCharacter);
                    }
                }
            }
        }
    }

    public partial class Pool {
        public Entity playerCharacterEntity { get { return GetGroup(Matcher.PlayerCharacter).GetSingleEntity(); } }

        public bool isPlayerCharacter {
            get { return playerCharacterEntity != null; }
            set {
                var entity = playerCharacterEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isPlayerCharacter = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherPlayerCharacter;

        public static AllOfMatcher PlayerCharacter {
            get {
                if (_matcherPlayerCharacter == null) {
                    _matcherPlayerCharacter = Matcher.AllOf(new [] { ComponentIds.PlayerCharacter });
                }

                return _matcherPlayerCharacter;
            }
        }
    }
}