using Entitas;
using UnityEngine;

public class CharMoveSystem : IExecuteSystem, ISetPool {
    Pool _repo;
    Group _collection;
    public void SetPool(Pool repo) {
        _repo = repo;
        _collection = _repo.GetGroup(Matcher.AllOf(Matcher.CharMove, Matcher.Position));
    }

    public void Execute() {
        if (_repo.isInputDetected)
        {
            //Debug.Log("Input found");
            foreach (var e in _collection.GetEntities())
            {
                var mov = e.charMove.moveSPD;
                var pos = e.position.myPos;
                //....fix this, two buttons that are members of the same axis are being checked
                //for the same state
                //eh
                //Do axis buttons have priority? 
                //why do I even need to check if a button is an axis... it just means ignore it
                //it's value will be retrieved by checking all axes values which I have to anyway!
                //why do I ask these dumb questions
                //and movement keys are all bound to axes so...
                //forget checking buttons at all
                //it's still weird that one axis (say horizontal) is bound as an attribute to both the command
                //left and right
                //well hmmm...
                //massive brain farts I refuse to think about this right now
                //I'm not checking for positive or negative values I set that in the editor
                //I should reverse the buttonasaxis value for WASD and L->R->U->D-> under "L""R""U""D"Commands
                //and instead attach those buttons asaxis for a...MoveVertical, MoveHorizontal command
                //then isaxis will only be useful for discovery and things that need the virtual axis can just check
                //MoveVertical, MoveHorizontal axes 
                //finished with a few short moments in notepad
                mov.x = mov.x * _repo.myInputs.axisValue["MoveHorizontal"];
                mov.z = mov.z * _repo.myInputs.axisValue["MoveVertical"];
                e.ReplacePosition(new Vector3(pos.x + mov.x, pos.y + mov.y, pos.z + mov.z));
            }
        }
        
    }

    
}

