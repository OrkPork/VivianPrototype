namespace Entitas {
    public partial class Entity {
        public MapPlayerComponent mapPlayer { get { return (MapPlayerComponent)GetComponent(ComponentIds.MapPlayer); } }

        public bool hasMapPlayer { get { return HasComponent(ComponentIds.MapPlayer); } }

        public void AddMapPlayer(MapPlayerComponent component) {
            AddComponent(ComponentIds.MapPlayer, component);
        }

        public void AddMapPlayer(string newPlayerName) {
            var component = new MapPlayerComponent();
            component.playerName = newPlayerName;
            AddMapPlayer(component);
        }

        public void ReplaceMapPlayer(string newPlayerName) {
            MapPlayerComponent component;
            if (hasMapPlayer) {
                WillRemoveComponent(ComponentIds.MapPlayer);
                component = mapPlayer;
            } else {
                component = new MapPlayerComponent();
            }
            component.playerName = newPlayerName;
            ReplaceComponent(ComponentIds.MapPlayer, component);
        }

        public void RemoveMapPlayer() {
            RemoveComponent(ComponentIds.MapPlayer);
        }
    }

    public partial class Pool {
        public Entity mapPlayerEntity { get { return GetGroup(Matcher.MapPlayer).GetSingleEntity(); } }

        public MapPlayerComponent mapPlayer { get { return mapPlayerEntity.mapPlayer; } }

        public bool hasMapPlayer { get { return mapPlayerEntity != null; } }

        public Entity SetMapPlayer(MapPlayerComponent component) {
            if (hasMapPlayer) {
                throw new SingleEntityException(Matcher.MapPlayer);
            }
            var entity = CreateEntity();
            entity.AddMapPlayer(component);
            return entity;
        }

        public Entity SetMapPlayer(string newPlayerName) {
            if (hasMapPlayer) {
                throw new SingleEntityException(Matcher.MapPlayer);
            }
            var entity = CreateEntity();
            entity.AddMapPlayer(newPlayerName);
            return entity;
        }

        public Entity ReplaceMapPlayer(string newPlayerName) {
            var entity = mapPlayerEntity;
            if (entity == null) {
                entity = SetMapPlayer(newPlayerName);
            } else {
                entity.ReplaceMapPlayer(newPlayerName);
            }

            return entity;
        }

        public void RemoveMapPlayer() {
            DestroyEntity(mapPlayerEntity);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherMapPlayer;

        public static AllOfMatcher MapPlayer {
            get {
                if (_matcherMapPlayer == null) {
                    _matcherMapPlayer = Matcher.AllOf(new [] { ComponentIds.MapPlayer });
                }

                return _matcherMapPlayer;
            }
        }
    }
}