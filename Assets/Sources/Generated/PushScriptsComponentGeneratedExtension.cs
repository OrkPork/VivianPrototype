namespace Entitas {
    public partial class Entity {
        public PushScriptsComponent pushScripts { get { return (PushScriptsComponent)GetComponent(ComponentIds.PushScripts); } }

        public bool hasPushScripts { get { return HasComponent(ComponentIds.PushScripts); } }

        public void AddPushScripts(PushScriptsComponent component) {
            AddComponent(ComponentIds.PushScripts, component);
        }

        public void AddPushScripts(System.Collections.Generic.List<string> newPushedScripts) {
            var component = new PushScriptsComponent();
            component.pushedScripts = newPushedScripts;
            AddPushScripts(component);
        }

        public void ReplacePushScripts(System.Collections.Generic.List<string> newPushedScripts) {
            PushScriptsComponent component;
            if (hasPushScripts) {
                WillRemoveComponent(ComponentIds.PushScripts);
                component = pushScripts;
            } else {
                component = new PushScriptsComponent();
            }
            component.pushedScripts = newPushedScripts;
            ReplaceComponent(ComponentIds.PushScripts, component);
        }

        public void RemovePushScripts() {
            RemoveComponent(ComponentIds.PushScripts);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherPushScripts;

        public static AllOfMatcher PushScripts {
            get {
                if (_matcherPushScripts == null) {
                    _matcherPushScripts = Matcher.AllOf(new [] { ComponentIds.PushScripts });
                }

                return _matcherPushScripts;
            }
        }
    }
}