namespace Entitas {
    public partial class Entity {
        public CharMoveComponent charMove { get { return (CharMoveComponent)GetComponent(ComponentIds.CharMove); } }

        public bool hasCharMove { get { return HasComponent(ComponentIds.CharMove); } }

        public void AddCharMove(CharMoveComponent component) {
            AddComponent(ComponentIds.CharMove, component);
        }

        public void AddCharMove(UnityEngine.Vector3 newMoveSPD) {
            var component = new CharMoveComponent();
            component.moveSPD = newMoveSPD;
            AddCharMove(component);
        }

        public void ReplaceCharMove(UnityEngine.Vector3 newMoveSPD) {
            CharMoveComponent component;
            if (hasCharMove) {
                WillRemoveComponent(ComponentIds.CharMove);
                component = charMove;
            } else {
                component = new CharMoveComponent();
            }
            component.moveSPD = newMoveSPD;
            ReplaceComponent(ComponentIds.CharMove, component);
        }

        public void RemoveCharMove() {
            RemoveComponent(ComponentIds.CharMove);
        }
    }

    public partial class Pool {
        public Entity charMoveEntity { get { return GetGroup(Matcher.CharMove).GetSingleEntity(); } }

        public CharMoveComponent charMove { get { return charMoveEntity.charMove; } }

        public bool hasCharMove { get { return charMoveEntity != null; } }

        public Entity SetCharMove(CharMoveComponent component) {
            if (hasCharMove) {
                throw new SingleEntityException(Matcher.CharMove);
            }
            var entity = CreateEntity();
            entity.AddCharMove(component);
            return entity;
        }

        public Entity SetCharMove(UnityEngine.Vector3 newMoveSPD) {
            if (hasCharMove) {
                throw new SingleEntityException(Matcher.CharMove);
            }
            var entity = CreateEntity();
            entity.AddCharMove(newMoveSPD);
            return entity;
        }

        public Entity ReplaceCharMove(UnityEngine.Vector3 newMoveSPD) {
            var entity = charMoveEntity;
            if (entity == null) {
                entity = SetCharMove(newMoveSPD);
            } else {
                entity.ReplaceCharMove(newMoveSPD);
            }

            return entity;
        }

        public void RemoveCharMove() {
            DestroyEntity(charMoveEntity);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherCharMove;

        public static AllOfMatcher CharMove {
            get {
                if (_matcherCharMove == null) {
                    _matcherCharMove = Matcher.AllOf(new [] { ComponentIds.CharMove });
                }

                return _matcherCharMove;
            }
        }
    }
}