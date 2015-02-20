using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour 
{
	public float speed = 8.0f;
	public GUIText scoreText;
	public GUIText winText;
	private int score = 0;
	public MainMechanics mainMechs;
	private int combatCount;
	public bool inCombat;
	public bool waitingForFader = false;
	public List<Combatant> partyList = new List<Combatant> ();
	public void setMainMechs(MainMechanics input)
	{
		mainMechs = input;
	}

	void Start()
	{
		SetCountText();
		winText.text = "";
		inCombat = false;
	}

	public Vector3 getPosition()
	{
		return transform.position;
	}

	public void setPosition(Vector3 startPos)
	{
		transform.position = startPos;
	}

	void FixedUpdate()
	{
		if (waitingForFader == false && inCombat == false)
		{
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertial = Input.GetAxis ("Vertical");
			Vector3 forceVector = new Vector3 (moveHorizontal, 0.0f, moveVertial);

			transform.Translate(forceVector*Time.deltaTime*speed, mainMechs.mainCamera.targetTracker.transform);

			//rigidbody.AddForce (forceVector * speed * Time.deltaTime);
		}
	}

	public void removeForce()
	{
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Enemy") 
		{
			if(inCombat == false)
			{
				other.gameObject.SetActive (false);
				inCombat = true;
				mainMechs.GoingToActiveateBattleMap();
			}
			else if (inCombat == true)
			{
				Combatant dyingEnemy = other.gameObject.GetComponent("Combatant") as Combatant;
				dyingEnemy.isKill();
				Destroy(other.gameObject);
				score++;
				SetCountText();
			}
		}
	}

	void SetCountText()
	{
		scoreText.text = "Score: " + score.ToString ();
		if (score == 18) 
		{
			winText.text = "YOU ARE WINNER!";
		}
	}
}
