namespace Entitas {
    public partial class Entity {
        public EncounterTriggerComponent encounterTrigger { get { return (EncounterTriggerComponent)GetComponent(ComponentIds.EncounterTrigger); } }

        public bool hasEncounterTrigger { get { return HasComponent(ComponentIds.EncounterTrigger); } }

        public void AddEncounterTrigger(EncounterTriggerComponent component) {
            AddComponent(ComponentIds.EncounterTrigger, component);
        }

        public void AddEncounterTrigger(System.String[] newResourceTrigger, System.String[] newBattleIDs) {
            var component = new EncounterTriggerComponent();
            component.resourceTrigger = newResourceTrigger;
            component.battleIDs = newBattleIDs;
            AddEncounterTrigger(component);
        }

        public void ReplaceEncounterTrigger(System.String[] newResourceTrigger, System.String[] newBattleIDs) {
            EncounterTriggerComponent component;
            if (hasEncounterTrigger) {
                WillRemoveComponent(ComponentIds.EncounterTrigger);
                component = encounterTrigger;
            } else {
                component = new EncounterTriggerComponent();
            }
            component.resourceTrigger = newResourceTrigger;
            component.battleIDs = newBattleIDs;
            ReplaceComponent(ComponentIds.EncounterTrigger, component);
        }

        public void RemoveEncounterTrigger() {
            RemoveComponent(ComponentIds.EncounterTrigger);
        }
    }

    public partial class Pool {
        public Entity encounterTriggerEntity { get { return GetGroup(Matcher.EncounterTrigger).GetSingleEntity(); } }

        public EncounterTriggerComponent encounterTrigger { get { return encounterTriggerEntity.encounterTrigger; } }

        public bool hasEncounterTrigger { get { return encounterTriggerEntity != null; } }

        public Entity SetEncounterTrigger(EncounterTriggerComponent component) {
            if (hasEncounterTrigger) {
                throw new SingleEntityException(Matcher.EncounterTrigger);
            }
            var entity = CreateEntity();
            entity.AddEncounterTrigger(component);
            return entity;
        }

        public Entity SetEncounterTrigger(System.String[] newResourceTrigger, System.String[] newBattleIDs) {
            if (hasEncounterTrigger) {
                throw new SingleEntityException(Matcher.EncounterTrigger);
            }
            var entity = CreateEntity();
            entity.AddEncounterTrigger(newResourceTrigger, newBattleIDs);
            return entity;
        }

        public Entity ReplaceEncounterTrigger(System.String[] newResourceTrigger, System.String[] newBattleIDs) {
            var entity = encounterTriggerEntity;
            if (entity == null) {
                entity = SetEncounterTrigger(newResourceTrigger, newBattleIDs);
            } else {
                entity.ReplaceEncounterTrigger(newResourceTrigger, newBattleIDs);
            }

            return entity;
        }

        public void RemoveEncounterTrigger() {
            DestroyEntity(encounterTriggerEntity);
        }
    }

    public static partial class Matcher {
        static AllOfMatcher _matcherEncounterTrigger;

        public static AllOfMatcher EncounterTrigger {
            get {
                if (_matcherEncounterTrigger == null) {
                    _matcherEncounterTrigger = Matcher.AllOf(new [] { ComponentIds.EncounterTrigger });
                }

                return _matcherEncounterTrigger;
            }
        }
    }
}