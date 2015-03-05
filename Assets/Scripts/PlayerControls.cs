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
	private int combatCount; //is this unused?
	public bool inCombat;
	public bool waitingForFader = false;
	public List<Combatant> partyList = new List<Combatant> ();
	public List<InventorySlot> inventory = new List<InventorySlot>();

	public void checkInventorySlots()
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i].ammountHere <= 0)
			{
				inventory.RemoveAt(i);
				i--;
			}
		}
	}

	public void setMainMechs(MainMechanics input)
	{
		mainMechs = input;
	}

    /// <summary>
    /// Initializes values, clears wintext, set incombat to false
    /// </summary>
	void Start()
	{
		SetCountText();
		winText.text = "";
		inCombat = false;
		for(int i = 0; i < 20; i++)
		{
			InventorySlot potion = new InventorySlot();
			potion.set (mainMechs.itemCreator.getPotion(i), 1);
			inventory.Add(potion);
		}
	}

    /// <summary>
    /// Returns the current position.
    /// </summary>
	public Vector3 getPosition()
	{
		return transform.position;
	}

    /// <summary>
    /// Change the players position
    /// </summary>
    /// <param name="startPos">New position: Vector3</param>
	public void setPosition(Vector3 startPos)
	{
		transform.position = startPos;
	}

    /// <summary>
    /// Handles movement at the physics interval, called by unity
    /// </summary>
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

    /// <summary>
    /// Zeroes rigidbody velocity and angularvelocity
    /// </summary>
	public void removeForce()
	{
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

    /// <summary>
    /// Detects enemy collision, calls battle state
    /// </summary>
    /// <param name="other">Collider, reads tag component to determine if enemy</param>
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
		}
	}

    /// <summary>
    /// Displays Score, if 18 displays wintext
    /// </summary>
	void SetCountText()
	{
		scoreText.text = "Score: " + score.ToString ();
		if (score == 18) 
		{
			winText.text = "YOU ARE WINNER!";
		}
	}
}
