using UnityEngine;
using System.Collections;

public class RelativityControls : MonoBehaviour {

	public GameObject mainCamera;
	
	// Update is called once per frame
	void LateUpdate () 
	{
		CameraControls cameraControl = mainCamera.GetComponent ("CameraControls") as CameraControls;

		if (cameraControl.target != null)
		{
			transform.position = new Vector3 (mainCamera.transform.position.x, cameraControl.target.transform.position.y, mainCamera.transform.position.z);

			transform.LookAt (cameraControl.target.transform.position);
		}
	}
}
