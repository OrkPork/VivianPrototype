using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public GameObject target;
	public bool inCombat = false;
	private Vector3 offset;
	private Vector3 combatOffset;
	public GameObject targetTracker;
	public GameObject player;
	public float trackingSpeed = 50;

	// Use this for initialization
	void Start () 
	{
		offset = transform.position;
		combatOffset = new Vector3 (-2f, 2f, -10f);
	}

	public void combatAngleChange()
	{
		transform.position = target.transform.position + combatOffset;
	}

	// Update is called once per frame
	void LateUpdate () 
	{
		if (target != null) 
		{
			if (inCombat == false) 
			{
				transform.position = target.transform.position + offset;
			}
			Quaternion currentRot = transform.rotation; //Remember current roation
			transform.LookAt (target.transform.position);//set current rotation to desired rotation
			transform.rotation = Quaternion.Lerp (currentRot, transform.rotation, trackingSpeed * Time.deltaTime);//Interpolates towards target.
		} 
		else 
		{
			target = player;
		}
	}
}
