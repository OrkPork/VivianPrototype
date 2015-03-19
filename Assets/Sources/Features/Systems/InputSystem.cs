using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class InputSystem : IExecuteSystem, ISetPool {
    /*public IMatcher GetTriggeringMatcher()// trigger this on....
    {
        return Matcher.ReceivesInput;
    }

    public GroupEventType GetEventType()//...this event...
    {
        //...if entity is added or removed, looking at reactivesystem it seems execute stops being called if matcher count=0
        //So if we have no receivesInput components, this thing will stop crunching commands (I hope)
        //I'm confused though
        //A component isn't an entity, I create nameless entities to push into a pool and attach components to them
        //Will this trigger if I remove a component from an entity? That's what I need/want it to do...
        // will test
        //alright it checks if any entity has my matching component, so if I remove the matching component(s) from every entity
        //it won't execute.
        //if something can receive, start processing
        //wish there was a silent version that just kept track of count instead of copying the entities and pushing them
        //to me
        //I should make a 'HandsOffReactiveSystem'
        //All it needs to know is that things can receive input, it doesn't need to know who 
        //I don't know maybe I'm approaching all of this all wrong...
        //this is my first entity system, kind of weird to get my head around

        //OH FORGET ME
        //I forgot this event is part of the matcher SO IF THERE'S NOTHING BEING ADDED OR REMOVED
        //THIS 
        //DOES
        //NOTHING

        //Saved as a monument to my dumbness
        return GroupEventType.OnEntityAddedOrRemoved;
    }*/
    /////CONSIDER: Turning the stateDictionary[command] into an object instead of 4 key-value pairs of string-bool
    ///// also CONSIDER: turning the entire thing into
    //
    // 1) an object array for discovery commandObj[] mycommands
    //for(i=0; i< mycommands[i].count)
    // axesRef= mycommands[i].axis
    // bttn= mycommands[i].key
    // for axes change value mycommands[i].axisValue = input.getaxis(axesref);
    // for bttns change state mycommands[i].isUp=true;
    //
    // And then a dictionary of commands -> object...
    //2) After I have everything finalized I'LL KNOW all possible commands so I can just make an enum and completely ignore
    //this name stuff...idk..i'm just not sure

    //I could do an enum of isDown,isHeld, etc...and then set that instead of string...seems like a waste of time maybe
    //later

    Pool _repo;
    Group _collection;
    string command;
    string axes;
    float vAxis;
    bool inputMovement;
    KeyCode bttn;

    public void SetPool(Pool repo) {
        _repo = repo;
        _collection = _repo.GetGroup(Matcher.ReceivesInput);
    }

    public void Execute() {
        //Debug.Log("???");
        
        //entities.SingleEntity().isInputDetected = false;
        if(_collection.Count != 0) {
            inputMovement = false; // ZERO IT VERY IMPORTANT
            //only set here ^
            for (int i = 0; i < _repo.myInputs.inputNames.Count; i++)
            {
                command = _repo.myInputs.inputNames[i];
                //get axes
                for (int x = 0; x < _repo.myInputs.myAxes[command].Count; x++)
                {
                    //I should rethink putting empty strings in
                    // or whatever
                    axes = _repo.myInputs.myAxes[command][x];
                    if (axes != "") 
                    {
                    
                        try
                        {
                            //reject values of 0 (let's be sane)
                            vAxis = Input.GetAxis(axes);
                            if( vAxis != 0f){
                                _repo.myInputs.axisValue[command] = vAxis;
                                //_repo.isReceivingInputComponent;
                                //Debug.Log(axes + " axis is being pushed: " + _repo.myInputs.axisValue[command]);
                                inputMovement = true;
                                //we've captured movement at this command, time to leave
                                break;
                            }
                            else
                            {
                                //NO INPUT HERE
                                _repo.myInputs.axisValue[command] = 0f;
                                //inputMovement = false;
                            }
                        }
                        catch
                        {
                            Debug.Log("Axis named improperly: " + axes);
                            Debug.Log("Zeroed axis value");
                            _repo.myInputs.axisValue[command] = 0f;
                        }
                    }
                    else
                    {
                        //NO INPUT HERE
                        _repo.myInputs.axisValue[command] = 0f;
                        //inputMovement = false;
                    }
                }

                //get buttons
                for (int z = 0; z < _repo.myInputs.myButtons[command].Count; z++)
                {
                    //already retrieved these when we checked axes, ignore here
                    if (_repo.myInputs.inputState[command]["isAxis"])
                    {
                        continue;
                    }
                    bttn = _repo.myInputs.myButtons[command][z];
                    if (Input.GetKey(bttn))
                    {
                        //caught
                        _repo.myInputs.inputState[command]["isHeld"] = true;
                        //rejected
                        _repo.myInputs.inputState[command]["isDown"] = false;
                        _repo.myInputs.inputState[command]["isUp"] = false;
                        //Debug.Log(command + " is being held.");
                        inputMovement = true;

                    }
                    else if (Input.GetKeyDown(bttn))
                    {
                        //caught
                        _repo.myInputs.inputState[command]["isDown"] = true;
                        //rejected
                        _repo.myInputs.inputState[command]["isHeld"] = false;
                        _repo.myInputs.inputState[command]["isUp"] = false;
                        inputMovement = true;
                    }
                    else if (Input.GetKeyUp(bttn))
                    {
                        //caught
                        _repo.myInputs.inputState[command]["isUp"] = true;
                        //rejected
                        _repo.myInputs.inputState[command]["isDown"] = false;
                        _repo.myInputs.inputState[command]["isHeld"] = false;
                        inputMovement = true;
                    }
                    else
                    {
                        //NO INPUT HERE
                        _repo.myInputs.inputState[command]["isUp"] = false;
                        _repo.myInputs.inputState[command]["isDown"] = false;
                        _repo.myInputs.inputState[command]["isHeld"] = false;
                        //inputMovement = false;
                    }
                

                }
            }
            //IMPORTANT
            //inform the inputReceivers that we've processed input
            _repo.isInputDetected = inputMovement;
        }

        
    }

    
}

