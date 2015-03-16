using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class InputSystem : IExecuteSystem, ISetPool {
    Pool _repo;

    public void SetPool(Pool repo) {
        _repo = repo;
    }

    public void Execute() {
        //in the input manager
        //pseudocode
        //cnLength = commandNames.count
        //for (length of commandButtons list)
        //^for (cnLength)
        //@this commandButton (if not keycode.NONE)
        //--get (anykeydown) >tapped
        //--get (anykey) >held
        //--get (anykeyup) >released
        //receive sensitivity (hardcoded?)
        //for (length of commandAxes list)
        //^for (cnLength)
        //@this axes (if not null)
        //if over or below sensitivity -set value or ignore
        //var cnLength = _repo.myInputs.commandNames.Count;
        //Check axis for movement (and if it matters to us)
        //for our total set of axis receivables
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

        for (int CLM = 0; CLM < _repo.myInputs.commandButton.Count; CLM++)
        {
            var tmp = _repo.myInputs.commandButton[CLM].Count;
            for (int ROW = 0; ROW < tmp; ROW++)
            {
                var myBttn = _repo.myInputs.commandButton[CLM][ROW];
                //var bttnType = _repo.myInputs.buttonAxis[ROW];
                //if(bttnType)
                //{
                //  continue;
                //} else 
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

