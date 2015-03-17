using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class InputSystem : IExecuteSystem, ISetPool {
    Pool _repo;

    public void SetPool(Pool repo) {
        _repo = repo;
    }

    public void Execute() {
        //check axes for movement
        string myAxis;
        for (int axCLM = 0; axCLM < _repo.myInputs.commandAxis.Count; axCLM++)
        {
            //for each axis relevant to a command
            var axTemp = _repo.myInputs.commandAxis[axCLM].Count;
            for (int axROW = 0; axROW < axTemp; axROW++)
            {
                myAxis = _repo.myInputs.commandAxis[axCLM][axROW];
                if (myAxis != "") // may want to put or null
                {
                    _repo.myInputs.axisValue[axCLM][axROW] = Input.GetAxis(myAxis);
                }
                else
                {
                    _repo.myInputs.axisValue[axCLM][axROW] = 0f;
                }
                
            }
        }
        //check buttons for presses, ignore buttons that are treated like an axis (they would've been solved above)
        for (int CLM = 0; CLM < _repo.myInputs.commandButton.Count; CLM++)
        {
            var tmp = _repo.myInputs.commandButton[CLM].Count;
            for (int ROW = 0; ROW < tmp; ROW++)
            {
                var myBttn = _repo.myInputs.commandButton[CLM][ROW];
                //if it's a bttn axis skip everything
                if (_repo.myInputs.buttonAxis[ROW])
                {
                    continue;
                }
                if (Input.GetKey(myBttn))
                {
                    _repo.myInputs.isHeld[CLM][ROW] = true;
                    //
                    _repo.myInputs.isUp[CLM][ROW] = false;
                    _repo.myInputs.isDown[CLM][ROW] = false;
                    //Debug.Log(myBttn + "key held");
                }
                else if (Input.GetKeyUp(myBttn))
                {
                    _repo.myInputs.isUp[CLM][ROW] = true;
                    //
                    _repo.myInputs.isHeld[CLM][ROW] = false;
                    _repo.myInputs.isDown[CLM][ROW] = false;
                    //Debug.Log(myBttn + "key released");
                }
                else if (Input.GetKeyDown(myBttn))
                {
                    _repo.myInputs.isDown[CLM][ROW] = true;
                    //
                    _repo.myInputs.isHeld[CLM][ROW] = false;
                    _repo.myInputs.isUp[CLM][ROW] = false;
                   // Debug.Log(myBttn + "key pressed");
                }
                else
                {
                    _repo.myInputs.isDown[CLM][ROW] = false;
                    _repo.myInputs.isHeld[CLM][ROW] = false;
                    _repo.myInputs.isUp[CLM][ROW] = false;
                }
            }

        }

    }

    
}

