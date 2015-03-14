namespace Entitas {
    public partial class Entity {
        public MoveComponent move { get { return (MoveComponent)GetComponent(ComponentIds.Move); } }

        public bool hasMove { get { return HasComponent(ComponentIds.Move); } }

        public void AddMove(MoveComponent component) {
            AddComponent(ComponentIds.Move, component);
        }

        public void AddMove(UnityEngine.Vector3 newMoveSPD) {
            var component = new MoveComponent();
            component.moveSPD = newMoveSPD;
            AddMove(component);
        }

        public void ReplaceMove(UnityEngine.Vector3 newMoveSPD) {
            MoveComponent component;
            if (hasMove) {
                WillRemoveComponent(ComponentIds.Move);
                component = move;
            } else {
                component = new MoveComponent();
            }
            component.moveSPD = newMoveSPD;
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