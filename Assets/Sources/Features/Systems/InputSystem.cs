using Entitas;
using UnityEngine;

public class InputSystem : IExecuteSystem, ISetPool {
    Pool _repo;

    public void SetPool(Pool repo) {
        _repo = repo;
    }

    public void Execute() {
        //needs references to player, pause
        _repo.ReplaceCharMove(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));
        //_repo.char
        //_repo.hSpeed = Input.GetAxis("Horizontal");
        //_repo.vSpeed = Input.GetAxis("Vertical");
        //accelerate button
        //_repo.isAccelerate = Input.GetMouseButton(0);
    }
}

