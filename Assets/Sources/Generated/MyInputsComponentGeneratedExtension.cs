namespace Entitas {
    public partial class Entity {
        public MyInputsComponent myInputs { get { return (MyInputsComponent)GetComponent(ComponentIds.MyInputs); } }

        public bool hasMyInputs { get { return HasComponent(ComponentIds.MyInputs); } }

        public void AddMyInputs(MyInputsComponent component) {
            AddComponent(ComponentIds.MyInputs, component);
        }

        public void AddMyInputs(System.Collections.Generic.Dictionary<string, string> newCommands) {
            var component = new MyInputsComponent();
            component.commands = newCommands;
            AddMyInputs(component);
        }

        public void ReplaceMyInputs(System.Collections.Generic.Dictionary<string, string> newCommands) {
            MyInputsComponent component;
            if (hasMyInputs) {
                WillRemoveComponent(ComponentIds.MyInputs);
                component = myInputs;
            } else {
                component = new MyInputsComponent();
            }
            component.commands = newCommands;
            ReplaceComponent(ComponentIds.MyInputs, component);
        }

        public void RemoveMyInputs() {
            RemoveComponent(ComponentIds.MyInputs);
        }
    }

    public partial class Pool {
        public Entity myInputsEntity { get { return GetGroup(Matcher.MyInputs).GetSingleEntity(); } }

        public MyInputsComponent myInputs { get { return myInputsEntity.myInputs; } }

        public bool hasMyInputs { get { return myInputsEntity != null; } }

        public Entity SetMyInputs(MyInputsComponent component) {
            if (hasMyInputs) {
                throw new SingleEntityException(Matcher.MyInputs);
            }
            var entity = CreateEntity();
            entity.AddMyInputs(component);
            return entity;
        }

        public Entity SetMyInputs(System.Collections.Generic.Dictionary<string, string> newCommands) {
            if (hasMyInputs) {
                throw new SingleEntityException(Matcher.MyInputs);
            }
            var entity = CreateEntity();
            entity.AddMyInputs(newCommands);
            return entity;
        }

        public Entity ReplaceMyInputs(System.Collections.Generic.Dictionary<string, string> newCommands) {
            var entity = myInputsEntity;
            if (entity == null) {
                entity = SetMyInputs(newCommands);
            } else {
                entity.ReplaceMyInputs(newCommands);
            }

            return entity;
        }

        public void RemoveMyInputs() {
            DestroyEntity(myInputsEntity);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherMyInputs;

        public static AllOfMatcher MyInputs {
            get {
                if (_matcherMyInputs == null) {
                    _matcherMyInputs = Matcher.AllOf(new [] { ComponentIds.MyInputs });
                }

                return _matcherMyInputs;
            }
        }
    }
}