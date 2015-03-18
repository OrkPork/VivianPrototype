namespace Entitas {
    public partial class Entity {
        public MyInputsComponent myInputs { get { return (MyInputsComponent)GetComponent(ComponentIds.MyInputs); } }

        public bool hasMyInputs { get { return HasComponent(ComponentIds.MyInputs); } }

        public void AddMyInputs(MyInputsComponent component) {
            AddComponent(ComponentIds.MyInputs, component);
        }

        public void AddMyInputs(System.Collections.Generic.Dictionary<int, string> newInputNames, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, bool>> newInputState, System.Collections.Generic.Dictionary<string, float> newAxisValue, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, UnityEngine.KeyCode>> newMyButtons, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, string>> newMyAxes) {
            var component = new MyInputsComponent();
            component.inputNames = newInputNames;
            component.inputState = newInputState;
            component.axisValue = newAxisValue;
            component.myButtons = newMyButtons;
            component.myAxes = newMyAxes;
            AddMyInputs(component);
        }

        public void ReplaceMyInputs(System.Collections.Generic.Dictionary<int, string> newInputNames, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, bool>> newInputState, System.Collections.Generic.Dictionary<string, float> newAxisValue, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, UnityEngine.KeyCode>> newMyButtons, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, string>> newMyAxes) {
            MyInputsComponent component;
            if (hasMyInputs) {
                WillRemoveComponent(ComponentIds.MyInputs);
                component = myInputs;
            } else {
                component = new MyInputsComponent();
            }
            component.inputNames = newInputNames;
            component.inputState = newInputState;
            component.axisValue = newAxisValue;
            component.myButtons = newMyButtons;
            component.myAxes = newMyAxes;
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

        public Entity SetMyInputs(System.Collections.Generic.Dictionary<int, string> newInputNames, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, bool>> newInputState, System.Collections.Generic.Dictionary<string, float> newAxisValue, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, UnityEngine.KeyCode>> newMyButtons, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, string>> newMyAxes) {
            if (hasMyInputs) {
                throw new SingleEntityException(Matcher.MyInputs);
            }
            var entity = CreateEntity();
            entity.AddMyInputs(newInputNames, newInputState, newAxisValue, newMyButtons, newMyAxes);
            return entity;
        }

        public Entity ReplaceMyInputs(System.Collections.Generic.Dictionary<int, string> newInputNames, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, bool>> newInputState, System.Collections.Generic.Dictionary<string, float> newAxisValue, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, UnityEngine.KeyCode>> newMyButtons, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, string>> newMyAxes) {
            var entity = myInputsEntity;
            if (entity == null) {
                entity = SetMyInputs(newInputNames, newInputState, newAxisValue, newMyButtons, newMyAxes);
            } else {
                entity.ReplaceMyInputs(newInputNames, newInputState, newAxisValue, newMyButtons, newMyAxes);
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