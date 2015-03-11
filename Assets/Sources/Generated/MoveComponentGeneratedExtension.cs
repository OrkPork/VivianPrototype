namespace Entitas {
    public partial class Entity {
        public MoveComponent move { get { return (MoveComponent)GetComponent(ComponentIds.Move); } }

        public bool hasMove { get { return HasComponent(ComponentIds.Move); } }

        public void AddMove(MoveComponent component) {
            AddComponent(ComponentIds.Move, component);
        }

        public void AddMove(float newXSpeed, float newYSpeed, float newZSpeed) {
            var component = new MoveComponent();
            component.xSpeed = newXSpeed;
            component.ySpeed = newYSpeed;
            component.zSpeed = newZSpeed;
            AddMove(component);
        }

        public void ReplaceMove(float newXSpeed, float newYSpeed, float newZSpeed) {
            MoveComponent component;
            if (hasMove) {
                WillRemoveComponent(ComponentIds.Move);
                component = move;
            } else {
                component = new MoveComponent();
            }
            component.xSpeed = newXSpeed;
            component.ySpeed = newYSpeed;
            component.zSpeed = newZSpeed;
            ReplaceComponent(ComponentIds.Move, component);
        }

        public void RemoveMove() {
            RemoveComponent(ComponentIds.Move);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherMove;

        public static AllOfMatcher Move {
            get {
                if (_matcherMove == null) {
                    _matcherMove = Matcher.AllOf(new [] { ComponentIds.Move });
                }

                return _matcherMove;
            }
        }
    }
}