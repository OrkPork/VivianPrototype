using Entitas;
using UnityEngine;

public class CharMoveSystem : IExecuteSystem, ISetPool {
    Pool _repo;

    public void SetPool(Pool repo) {
        _repo = repo;
    }

    public void Execute() {
        //if(_repo.myInputs.)
    }

    
}

