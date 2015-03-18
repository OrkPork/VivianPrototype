namespace Entitas {
    public partial class Entity {
        static readonly ReceivesInputComponent receivesInputComponent = new ReceivesInputComponent();

        public bool isReceivesInput {
            get { return HasComponent(ComponentIds.ReceivesInput); }
            set {
                if (value != isReceivesInput) {
                    if (value) {
                        AddComponent(ComponentIds.ReceivesInput, receivesInputComponent);
                    } else {
                        RemoveComponent(ComponentIds.ReceivesInput);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherReceivesInput;

        public static AllOfMatcher ReceivesInput {
            get {
                if (_matcherReceivesInput == null) {
                    _matcherReceivesInput = Matcher.AllOf(new [] { ComponentIds.ReceivesInput });
                }

                return _matcherReceivesInput;
            }
        }
    }
}