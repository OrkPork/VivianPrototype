namespace Entitas {
    public partial class Entity {
        public MyInputsComponent myInputs { get { return (MyInputsComponent)GetComponent(ComponentIds.MyInputs); } }

        public bool hasMyInputs { get { return HasComponent(ComponentIds.MyInputs); } }

        public void AddMyInputs(MyInputsComponent component) {
            AddComponent(ComponentIds.MyInputs, component);
        }

        public void AddMyInputs(System.Collections.Generic.List<string> newCommandNames, System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.KeyCode>> newCommandButton, System.Collections.Generic.List<System.Collections.Generic.List<string>> newCommandAxis, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsUp, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsHeld, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsDown, System.Collections.Generic.List<System.Collections.Generic.List<float>> newAxisValue, System.Collections.Generic.List<bool> newButtonAxis) {
            var component = new MyInputsComponent();
            component.commandNames = newCommandNames;
            component.commandButton = newCommandButton;
            component.commandAxis = newCommandAxis;
            component.isUp = newIsUp;
            component.isHeld = newIsHeld;
            component.isDown = newIsDown;
            component.axisValue = newAxisValue;
            component.buttonAxis = newButtonAxis;
            AddMyInputs(component);
        }

        public void ReplaceMyInputs(System.Collections.Generic.List<string> newCommandNames, System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.KeyCode>> newCommandButton, System.Collections.Generic.List<System.Collections.Generic.List<string>> newCommandAxis, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsUp, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsHeld, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsDown, System.Collections.Generic.List<System.Collections.Generic.List<float>> newAxisValue, System.Collections.Generic.List<bool> newButtonAxis) {
            MyInputsComponent component;
            if (hasMyInputs) {
                WillRemoveComponent(ComponentIds.MyInputs);
                component = myInputs;
            } else {
                component = new MyInputsComponent();
            }
            component.commandNames = newCommandNames;
            component.commandButton = newCommandButton;
            component.commandAxis = newCommandAxis;
            component.isUp = newIsUp;
            component.isHeld = newIsHeld;
            component.isDown = newIsDown;
            component.axisValue = newAxisValue;
            component.buttonAxis = newButtonAxis;
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

        public Entity SetMyInputs(System.Collections.Generic.List<string> newCommandNames, System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.KeyCode>> newCommandButton, System.Collections.Generic.List<System.Collections.Generic.List<string>> newCommandAxis, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsUp, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsHeld, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsDown, System.Collections.Generic.List<System.Collections.Generic.List<float>> newAxisValue, System.Collections.Generic.List<bool> newButtonAxis) {
            if (hasMyInputs) {
                throw new SingleEntityException(Matcher.MyInputs);
            }
            var entity = CreateEntity();
            entity.AddMyInputs(newCommandNames, newCommandButton, newCommandAxis, newIsUp, newIsHeld, newIsDown, newAxisValue, newButtonAxis);
            return entity;
        }

        public Entity ReplaceMyInputs(System.Collections.Generic.List<string> newCommandNames, System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.KeyCode>> newCommandButton, System.Collections.Generic.List<System.Collections.Generic.List<string>> newCommandAxis, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsUp, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsHeld, System.Collections.Generic.List<System.Collections.Generic.List<bool>> newIsDown, System.Collections.Generic.List<System.Collections.Generic.List<float>> newAxisValue, System.Collections.Generic.List<bool> newButtonAxis) {
            var entity = myInputsEntity;
            if (entity == null) {
                entity = SetMyInputs(newCommandNames, newCommandButton, newCommandAxis, newIsUp, newIsHeld, newIsDown, newAxisValue, newButtonAxis);
            } else {
                entity.ReplaceMyInputs(newCommandNames, newCommandButton, newCommandAxis, newIsUp, newIsHeld, newIsDown, newAxisValue, newButtonAxis);
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