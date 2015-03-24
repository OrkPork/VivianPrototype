using System;
using UnityEngine;
using Entitas;
public class TagOnTriggerEnter : MonoBehaviour
{


    /*public string[] battleTriggers = new string[0];
    public string[] allBattleIDs = new string[0];
    public string battleID = "";
    public string triggerID = "";
    public bool foundFight = false;*/
    //public string objTag;

    void Start()
    {
        //Debug.Log("I'm alive!");
    }
    void OnTriggerEnter(Collider other)
    {
        //objTag = other.tag;
        this.tag = other.tag;
        //GameController.re
        /*for (int i = 0; i < battleTriggers.Length; i++)
        {
            if (other.tag == battleTriggers[i])
            {
                Debug.Log("Fight Found");
                triggerID = battleTriggers[i];
                battleID = allBattleIDs[i];
                foundFight = true;
            }
        }*/
    }
}
