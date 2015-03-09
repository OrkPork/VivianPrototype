using Entitas;
using UnityEngine;

public class InputSystem : IEntitySystem, ISetEntityRepository {
    EntityRepository _repo;

    public void SetEntityRepository(EntityRepository repo) {
        _repo = repo;
    }

    public void Execute() {
        //needs references to player, pause
        _repo.ReplaceCharMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Jump"));
        //_repo.char
        //_repo.hSpeed = Input.GetAxis("Horizontal");
        //_repo.vSpeed = Input.GetAxis("Vertical");
        //accelerate button
        _repo.isAccelerate = Input.GetMouseButton(0);
    }
}

