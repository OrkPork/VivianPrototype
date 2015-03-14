using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameObject[] players;
	public float offsetZ = -10;
	public float OffsetY = 2f;
	public float dampTime = 0.15f;

	public static float leftBorder;		// left border
	public static float rightBorder;	// right border
	public static float bottomBorder;	// bottom border
	public static float topBorder;		// top border

	Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		calcBorders();
	}

	
	// Update is called once per frame
	void Update () {
		if(players == null){
			checkPlayers();
		}
		calcBorders(); 
		moveCamera();
	}

	public void checkPlayers(){
		players = GameObject.FindGameObjectsWithTag("Robot");
	}

	void moveCamera(){
		Vector3 center = calcCenter();
		Vector3 point = camera.WorldToViewportPoint(center);
		Vector3 delta = center - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination  = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	}

	Vector3 calcCenter(){
		Vector3 center = players[0].transform.position;

		if(players.Length > 1){
			int cnt=1;
			for(int i=0; i<players.Length; i++){
				if(i!=0){
					center += players[i].transform.position;
					cnt++;
				}
			}
				center = center/cnt;
		}
		return new Vector3(center.x, center.y+OffsetY, center.z);
	}

	void calcBorders(){
		leftBorder = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, -offsetZ)).x;
		rightBorder = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, -offsetZ)).x;
		topBorder = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, -offsetZ)).y;
		bottomBorder = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, -offsetZ)).y;
	}

	// Draw Cam Borders
	void OnDrawGizmos () {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (Camera.main.ViewportToWorldPoint (new Vector3 (0.51f, 0.49f, -offsetZ)), Camera.main.ViewportToWorldPoint (new Vector3 (0.49f, 0.51f, -offsetZ)));
		Gizmos.DrawLine (Camera.main.ViewportToWorldPoint (new Vector3 (0.51f, 0.51f, -offsetZ)), Camera.main.ViewportToWorldPoint (new Vector3 (0.49f, 0.49f, -offsetZ)));
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, -offsetZ)), Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, -offsetZ)));
		Gizmos.DrawLine (Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, -offsetZ)), Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, -offsetZ)));
		Gizmos.DrawLine (Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, -offsetZ)), Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, -offsetZ)));
		Gizmos.DrawLine (Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, -offsetZ)), Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, -offsetZ)));
	}
}
