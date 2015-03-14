using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class ContactFights : MonoBehaviour {

    void Start()
    {
        Debug.Log("I'm alive!");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //GameController.d
            Debug.Log("I work I work I work");
        }
    }
}
