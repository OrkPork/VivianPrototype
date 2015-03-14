using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class InputSystem : IExecuteSystem, ISetPool {
    Pool _repo;

    public void SetPool(Pool repo) {
        _repo = repo;
    }

    public void Execute() {
        //capture inputs// input stats
        /* detect if commands pressed and if commands are joystick or keyboard
         * 
         * 
         * */
        //replace with component reference
        List<string> compareStr = new List<string> {"butts"};
        Dictionary<string, string> removeaftergenerate = new Dictionary<string,string>() {{"also", "butts"}};
        if (Input.anyKey)
        {
            //reference to a component goes here
            List<string> componentkeysreference = getStrokedKeys(compareStr, removeaftergenerate);
        }
    }

    List<string> getStrokedKeys(List<string> str_sort, Dictionary<string,string> dict_sort)
    {
        var strokedKeys = new List<string>();
        string temp;
        for (int i = 0; i < str_sort.Count; i++)
        {
            if (dict_sort.TryGetValue(str_sort[i], out temp))
            {
                if (Input.GetKey(temp))
                {
                    strokedKeys.Add(temp);
                }
                else
                {
                    //what about mouse clicks?
                }
                
            }
        }
            
        //dict_sort.Keys;
        return strokedKeys;
    }
}

