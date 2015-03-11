namespace Entitas {
    public partial class Entity {
        public CharMove charMove { get { return (CharMove)GetComponent(ComponentIds.CharMove); } }

        public bool hasCharMove { get { return HasComponent(ComponentIds.CharMove); } }

        public void AddCharMove(CharMove component) {
            AddComponent(ComponentIds.CharMove, component);
        }

        public void AddCharMove(float newXSpeed, float newYSpeed, float newZSpeed) {
            var component = new CharMove();
            component.xSpeed = newXSpeed;
            component.ySpeed = newYSpeed;
            component.zSpeed = newZSpeed;
            AddCharMove(component);
        }

        public void ReplaceCharMove(float newXSpeed, float newYSpeed, float newZSpeed) {
            CharMove component;
            if (hasCharMove) {
                WillRemoveComponent(ComponentIds.CharMove);
                component = charMove;
            } else {
                component = new CharMove();
            }
            component.xSpeed = newXSpeed;
            component.ySpeed = newYSpeed;
            component.zSpeed = newZSpeed;
            ReplaceComponent(ComponentIds.CharMove, component);
        }

        public void RemoveCharMove() {
            RemoveComponent(ComponentIds.CharMove);
        }
    }

    public partial class Pool {
        public Entity charMoveEntity { get { return GetGroup(Matcher.CharMove).GetSingleEntity(); } }

        public CharMove charMove { get { return charMoveEntity.charMove; } }

        public bool hasCharMove { get { return charMoveEntity != null; } }

        public Entity SetCharMove(CharMove component) {
            if (hasCharMove) {
                throw new SingleEntityException(Matcher.CharMove);
            }
            var entity = CreateEntity();
            entity.AddCharMove(component);
            return entity;
        }

        public Entity SetCharMove(float newXSpeed, float newYSpeed, float newZSpeed) {
            if (hasCharMove) {
                throw new SingleEntityException(Matcher.CharMove);
            }
            var entity = CreateEntity();
            entity.AddCharMove(newXSpeed, newYSpeed, newZSpeed);
            return entity;
        }

        public Entity ReplaceCharMove(float newXSpeed, float newYSpeed, float newZSpeed) {
            var entity = charMoveEntity;
            if (entity == null) {
                entity = SetCharMove(newXSpeed, newYSpeed, newZSpeed);
            } else {
                entity.ReplaceCharMove(newXSpeed, newYSpeed, newZSpeed);
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