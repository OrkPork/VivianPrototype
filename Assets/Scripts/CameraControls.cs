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
	float orbitSpeed = 0.5f;
	public float cameraDistance;
	public float relativeAngel;

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
			bool cameraRotating = false;
			if (inCombat == false) 
			{
				float moveHorizontal = Input.GetAxis ("Camera Horizontal");
				float moveVertial = Input.GetAxis ("Camera Vertical");
				if(Mathf.Abs(moveVertial) > 0 || Mathf.Abs(moveHorizontal) > 0)
				{
					cameraRotating = true;
					transform.LookAt(target.transform.position);

					if(transform.rotation.eulerAngles.x <= 90)
					{
						relativeAngel = transform.rotation.eulerAngles.x;
					}
					else if(transform.rotation.eulerAngles.x >= 270)
					{
						relativeAngel = transform.rotation.eulerAngles.x - 360;
					}

					if((relativeAngel < 65 || moveVertial < 0) 
					   && (relativeAngel > -65 || moveVertial > 0))
					{
						transform.Translate(Vector3.up*moveVertial*orbitSpeed);
					}
					transform.Translate(Vector3.left*moveHorizontal*orbitSpeed);


					offset = transform.position-target.transform.position;

				}
				transform.position = target.transform.position + offset;
				cameraDistance = Vector3.Distance(transform.position, target.transform.position);
				if(cameraDistance > 11)
				{
					transform.position += transform.forward*(cameraDistance-11); 
				}
				else if(cameraDistance < 11)
				{
					transform.position -= transform.forward*(11-cameraDistance); 
				}
				
				RaycastHit rayHit;
				Quaternion returnPos = target.transform.rotation;
				target.transform.LookAt(transform.position);

				
				Ray ray = new Ray(target.transform.position, target.transform.forward);
				if (Physics.Raycast (ray, out rayHit, 11.0f)) 
				{
						transform.position = rayHit.point+new Vector3(0,0.1f,0);
				}
				target.transform.rotation = returnPos;
			}
			if(cameraRotating == false)
			{
			Quaternion currentRot = transform.rotation; //Remember current roation
			transform.LookAt (target.transform.position);//set current rotation to desired rotation
			transform.rotation = Quaternion.Lerp (currentRot, transform.rotation, trackingSpeed * Time.deltaTime);//Interpolates towards target.
			}
		} 
		else 
		{
			target = player;
		}
	}
}
