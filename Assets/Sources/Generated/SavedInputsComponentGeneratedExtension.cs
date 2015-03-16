namespace Entitas {
    public partial class Entity {
        public SavedInputsComponent savedInputs { get { return (SavedInputsComponent)GetComponent(ComponentIds.SavedInputs); } }

        public bool hasSavedInputs { get { return HasComponent(ComponentIds.SavedInputs); } }

        public void AddSavedInputs(SavedInputsComponent component) {
            AddComponent(ComponentIds.SavedInputs, component);
        }

        public void AddSavedInputs(System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.KeyCode>> newCommandButton, System.Collections.Generic.List<System.Collections.Generic.List<string>> newCommandAxis) {
            var component = new SavedInputsComponent();
            component.commandButton = newCommandButton;
            component.commandAxis = newCommandAxis;
            AddSavedInputs(component);
        }

        public void ReplaceSavedInputs(System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.KeyCode>> newCommandButton, System.Collections.Generic.List<System.Collections.Generic.List<string>> newCommandAxis) {
            SavedInputsComponent component;
            if (hasSavedInputs) {
                WillRemoveComponent(ComponentIds.SavedInputs);
                component = savedInputs;
            } else {
                component = new SavedInputsComponent();
            }
            component.commandButton = newCommandButton;
            component.commandAxis = newCommandAxis;
            ReplaceComponent(ComponentIds.SavedInputs, component);
        }

        public void RemoveSavedInputs() {
            RemoveComponent(ComponentIds.SavedInputs);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherSavedInputs;

        public static AllOfMatcher SavedInputs {
            get {
                if (_matcherSavedInputs == null) {
                    _matcherSavedInputs = Matcher.AllOf(new [] { ComponentIds.SavedInputs });
                }

                return _matcherSavedInputs;
            }
        }
    }
}