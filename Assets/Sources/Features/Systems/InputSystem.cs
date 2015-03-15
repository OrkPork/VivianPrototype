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

        //if switch on axis
        //if anykeydown
        //if anykey
        //if anykeyup
        

        //if you use keycodes instead of strings you don't need to do extra work to capture mouse clicks
        //PROBLEM: you will need to make sure the offline data is a corresponding keycode.

        List<string> compareStr = new List<string> {"butts"};
        Dictionary<string, string> furcoat = new Dictionary<string, string>();
        Dictionary<string, string> dictInputs = _repo.myInputs.commands;
        if (Input.anyKeyDown)
        {
            //reference to a component goes here
            List<string> componentkeysreference = getStrokedKeys(compareStr, dictInputs);
        }
        //if held down
        //if just released
        //if an axis
    }

    List<string> getStrokedKeys(List<string> str_sort, Dictionary<string,string> dict_sort)
    {
        var strokedKeys = new List<string>();
        string temp;
        for (int i = 0; i < str_sort.Count; i++)
        {
            if (dict_sort.TryGetValue(str_sort[i], out temp))
            {
                
                if (Input.GetKeyDown(temp))
                {
                    strokedKeys.Add(str_sort[i]);
                }
                else
                {
                    //what about mouse clicks?
                    switch (temp)
                    {
                        case "0":
                            Input.GetMouseButtonDown(0);
                            break;
                        case "1":
                            Input.GetMouseButtonDown(1);
                            break;
                        case "2":
                            Input.GetMouseButtonDown(2);
                            break;
                    }
                    
                }
                
            }
        }
            
        //dict_sort.Keys;
        return strokedKeys;
    }
}

