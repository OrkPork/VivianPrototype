using UnityEngine;
using System.Collections;

public class DontOffload : MonoBehaviour {

	
	
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
}
