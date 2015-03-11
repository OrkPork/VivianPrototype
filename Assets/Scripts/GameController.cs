using UnityEngine;
using System.Collections.Generic;
using Entitas;

public class GameController : MonoBehaviour {
    Pool _repo;
    int mapIndex = 0;
    public List<string> mapTitles = new List<string> { "TestMap" };
    IExecuteSystem[] _systems;

    void Start() {
        Random.seed = 42;
        _repo = new Pool(ComponentIds.TotalComponents);
        
        createSystems();
        createTestMap();
        createPlayer();
        createOpponents();
    }

    void createSystems() {
        //also need a pool for game objects
        //System order
        /*
             * Input //base input menus and the such
             *
             * Charmovement input and Battle Input 
             * 
             * Apply Acceleration/movement modifiers
             * 
             * Handle all movements.
             * 
             * populate new objects
             * 
             * handle collisions/contacts of objects
             * 
             * 
             * render objects
             * 
             * destroy objects
             * 
             * de-render objects    
             * */

        //Sytems needed
        /*
         * world system
         * 
         * Contact
         * 
         * Populate UI
         * 
         * Create behavior components for UI (shortcuts)
         * 
         * Camera Flavor
         * 
         * Dialogue
         * 
         * Populate Battle
         * 
         * Run function, run animation
         * 
         * calculate damage (restore controls)
         * */

        _systems = new [] {
            
            _repo.CreateSystem<InputSystem>(),
            
            //_repo.CreateSystem<AccelerateSystem>(),
            _repo.CreateSystem<MoveSystem>(),

            _repo.CreateSystem<RenderSpawnSystem>(),
            _repo.CreateSystem<RenderPositionSystem>(),

            _repo.CreateSystem<DestroySystem>(),
            _repo.CreateSystem<RenderDespawnSystem>()
        };
    }

    void createTestMap()
    {
        var e = _repo.CreateEntity();
        e.AddResource(mapTitles[mapIndex]);
        e.AddPosition(0, 0, 0);
    }

    void createPlayer() {
        var e = _repo.CreateEntity();
        e.AddResource("Player");
        e.AddPosition(0, 0.5f, 0);
        e.AddMove(8f, 8f, 0);
        e.isAcceleratable = true;
    }

    void createOpponents() {
        const string resourceName = "Enemy";
        for (int i = 0; i < 6; i++) {
            var e = _repo.CreateEntity();
            e.AddResource(resourceName);
            if (i < 1)
            {
                e.AddPosition(2.5f, 0.5f, 4.7f);
            } else if (i < 2)
            {
                e.AddPosition(-13f, 0.5f, 7.64f);
            } else if (i < 3)
            {
                e.AddPosition(-5.6f, 0.5f, 3.9f);
            } else if (i < 4)
            {
                e.AddPosition(6.6f, 0.5f, 4.7f);
            } else if (i < 5)
            {
                e.AddPosition(2.5f, 0.5f, 4.7f);
            } else
            {
                e.AddPosition(2.5f, 0.5f, 4.7f);
            }

        }
    }

    void Update() {
        foreach (var system in _systems) {
            system.Execute();
        }
    }
}
