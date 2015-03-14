namespace Entitas {
    public partial class Entity {
        static readonly DefaultInputsComponent defaultInputsComponent = new DefaultInputsComponent();

        public bool isDefaultInputs {
            get { return HasComponent(ComponentIds.DefaultInputs); }
            set {
                if (value != isDefaultInputs) {
                    if (value) {
                        AddComponent(ComponentIds.DefaultInputs, defaultInputsComponent);
                    } else {
                        RemoveComponent(ComponentIds.DefaultInputs);
                    }
                }
            }
        }
    }

    public partial class Pool {
        public Entity defaultInputsEntity { get { return GetGroup(Matcher.DefaultInputs).GetSingleEntity(); } }

        public bool isDefaultInputs {
            get { return defaultInputsEntity != null; }
            set {
                var entity = defaultInputsEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isDefaultInputs = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherDefaultInputs;

        public static AllOfMatcher DefaultInputs {
            get {
                if (_matcherDefaultInputs == null) {
                    _matcherDefaultInputs = Matcher.AllOf(new [] { ComponentIds.DefaultInputs });
                }

                return _matcherDefaultInputs;
            }
        }
    }
}