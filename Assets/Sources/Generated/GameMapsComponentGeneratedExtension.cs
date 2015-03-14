namespace Entitas {
    public partial class Entity {
        public GameMapsComponent gameMaps { get { return (GameMapsComponent)GetComponent(ComponentIds.GameMaps); } }

        public bool hasGameMaps { get { return HasComponent(ComponentIds.GameMaps); } }

        public void AddGameMaps(GameMapsComponent component) {
            AddComponent(ComponentIds.GameMaps, component);
        }

        public void AddGameMaps(System.Collections.Generic.List<string> newMapTitles) {
            var component = new GameMapsComponent();
            component.mapTitles = newMapTitles;
            AddGameMaps(component);
        }

        public void ReplaceGameMaps(System.Collections.Generic.List<string> newMapTitles) {
            GameMapsComponent component;
            if (hasGameMaps) {
                WillRemoveComponent(ComponentIds.GameMaps);
                component = gameMaps;
            } else {
                component = new GameMapsComponent();
            }
            component.mapTitles = newMapTitles;
            ReplaceComponent(ComponentIds.GameMaps, component);
        }

        public void RemoveGameMaps() {
            RemoveComponent(ComponentIds.GameMaps);
        }
    }

    public partial class Pool {
        public Entity gameMapsEntity { get { return GetGroup(Matcher.GameMaps).GetSingleEntity(); } }

        public GameMapsComponent gameMaps { get { return gameMapsEntity.gameMaps; } }

        public bool hasGameMaps { get { return gameMapsEntity != null; } }

        public Entity SetGameMaps(GameMapsComponent component) {
            if (hasGameMaps) {
                throw new SingleEntityException(Matcher.GameMaps);
            }
            var entity = CreateEntity();
            entity.AddGameMaps(component);
            return entity;
        }

        public Entity SetGameMaps(System.Collections.Generic.List<string> newMapTitles) {
            if (hasGameMaps) {
                throw new SingleEntityException(Matcher.GameMaps);
            }
            var entity = CreateEntity();
            entity.AddGameMaps(newMapTitles);
            return entity;
        }

        public Entity ReplaceGameMaps(System.Collections.Generic.List<string> newMapTitles) {
            var entity = gameMapsEntity;
            if (entity == null) {
                entity = SetGameMaps(newMapTitles);
            } else {
                entity.ReplaceGameMaps(newMapTitles);
            }

            return entity;
        }

        public void RemoveGameMaps() {
            DestroyEntity(gameMapsEntity);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherGameMaps;

        public static AllOfMatcher GameMaps {
            get {
                if (_matcherGameMaps == null) {
                    _matcherGameMaps = Matcher.AllOf(new [] { ComponentIds.GameMaps });
                }

                return _matcherGameMaps;
            }
        }
    }
}