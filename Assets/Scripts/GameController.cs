using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour {
    EntityRepository _repo;

    IEntitySystem[] _systems;

    void Start() {
        Random.seed = 42;
        _repo = new EntityRepository(ComponentIds.TotalComponents);
        createSystems();
        createPlayer();
        createOpponents();
    }

    void createSystems() {
        /*
             * Input
             * 
             * Apply Acceleration/movement modifiers
             * 
             * Handle all movements.
             * 
             * Handle contacts/collisions
             * 
             * Render scene
             * 
             * 
             * 
             * */
        _systems = new [] {
            
            _repo.CreateSystem<InputSystem>(),
            
            _repo.CreateSystem<AccelerateSystem>(),
            _repo.CreateSystem<MoveSystem>(),

            _repo.CreateSystem<RenderSpawnSystem>(),
            _repo.CreateSystem<RenderPositionSystem>(),

            _repo.CreateSystem<DestroySystem>(),
            _repo.CreateSystem<RenderDespawnSystem>()
        };
    }

    void createPlayer() {
        var buttlord = _repo.CreateEntity();
        buttlord.AddResource("supremebuttlord");
        var e = _repo.CreateEntity();
        e.AddResource("Player");
        e.AddPosition(0, 0, 0);
        e.AddMove(0, 0.025f);
        e.isAcceleratable = true;
    }

    void createOpponents() {
        const string resourceName = "Opponent";
        for (int i = 1; i < 10; i++) {
            var e = _repo.CreateEntity();
            e.AddResource(resourceName);
            e.AddPosition(i, 0, 0);
            var speed = Random.value * 0.02f;
            e.AddMove(speed, speed);
        }
    }

    void Update() {
        foreach (var system in _systems) {
            system.Execute();
        }
    }
}
