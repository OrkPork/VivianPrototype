namespace Entitas {
    public partial class Entity {
        public CharSkillsComponent charSkills { get { return (CharSkillsComponent)GetComponent(ComponentIds.CharSkills); } }

        public bool hasCharSkills { get { return HasComponent(ComponentIds.CharSkills); } }

        public void AddCharSkills(CharSkillsComponent component) {
            AddComponent(ComponentIds.CharSkills, component);
        }

        public void AddCharSkills(System.Collections.Generic.List<string> newSkills) {
            var component = new CharSkillsComponent();
            component.skills = newSkills;
            AddCharSkills(component);
        }

        public void ReplaceCharSkills(System.Collections.Generic.List<string> newSkills) {
            CharSkillsComponent component;
            if (hasCharSkills) {
                WillRemoveComponent(ComponentIds.CharSkills);
                component = charSkills;
            } else {
                component = new CharSkillsComponent();
            }
            component.skills = newSkills;
            ReplaceComponent(ComponentIds.CharSkills, component);
        }

        public void RemoveCharSkills() {
            RemoveComponent(ComponentIds.CharSkills);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherCharSkills;

        public static AllOfMatcher CharSkills {
            get {
                if (_matcherCharSkills == null) {
                    _matcherCharSkills = Matcher.AllOf(new [] { ComponentIds.CharSkills });
                }

                return _matcherCharSkills;
            }
        }
    }
}