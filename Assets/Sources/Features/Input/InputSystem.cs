﻿using Entitas;
using UnityEngine;

public class InputSystem : IEntitySystem, ISetEntityRepository {
    EntityRepository _repo;

    public void SetEntityRepository(EntityRepository repo) {
        _repo = repo;
    }

    public void Execute() {
        //needs references to player, pause
        _repo.hSpeed = Input.GetAxis("Horizontal");
        _repo.vSpeed = Input.GetAxis("Vertical");
        //accelerate button
        _repo.isAccelerate = Input.GetMouseButton(0);
    }
}
