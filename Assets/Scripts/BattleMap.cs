using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleMap : MonoBehaviour {
	public PlayerControls player;
	public MainMechanics mainMechanics;
	private Vector3 offset;
	public List<GameObject> combatantsInFight = new List<GameObject>();
	public List<GameObject> enemyList = new List<GameObject>();
	public List<InitiativeToken> initiativeList = new List<InitiativeToken> ();
	public GameObject[] spawnPositions = new GameObject[3];
	Combatant targetedCombatant;
	public GameObject selectorModel;
	float waitAmount = 0.15f;
	float weGonnaWaitOnThisShit;
	// Use this for initialization
	void Start () 
	{
		offset = transform.position;
	}

	void OnGUI()
	{
		if (initiativeList.Count > 0) 
		{
			GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b);
			for (int i = 0; i < 10; i++) 
			{
				GUI.DrawTexture (new Rect (Screen.width - ((Screen.height * 2) / 9),
			                           (Screen.height / 18) * i,
			                           ((Screen.height * 2) / 9),
			                           (Screen.height / 18)), initiativeList [i].intiativePortrait);
				GUI.TextArea (new Rect (Screen.width - ((Screen.height * 2) / 9),
			                       (Screen.height / 18) * i,
			                       ((Screen.height * 2) / 9),
				                        (Screen.height / 18)), "" + initiativeList [i].tickCount + " " + initiativeList [i].character.name);
			}
			
		}
		
	}

	public void wait(float time)
	{
		weGonnaWaitOnThisShit = time;
	}
	
	public Combatant selectTarget(bool isOffensive, Combatant selectingTarget)
	{
		if(Input.GetButtonUp("Action") == true && weGonnaWaitOnThisShit <= 0)
		{
			selectorModel.SetActive(false);
			return targetedCombatant;
		}
		if (weGonnaWaitOnThisShit > 0) 
		{
			weGonnaWaitOnThisShit -= 1.0f * Time.deltaTime;
		}
		selectorModel.SetActive (true);
		if (isOffensive == true && selectingTarget.isChoosing == false) 
		{
			targetedCombatant = combatantsInFight[0].GetComponent("Combatant") as Combatant;
			selectingTarget.isChoosing = true;
		}

		float moveHorizontal = Input.GetAxis ("Horizontal");//Get controler input
		float moveVertial = Input.GetAxis ("Vertical");

		RaycastHit rayHit;


		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out rayHit)) 
		{
			//Debug.DrawLine(mainMechanics.mainCamera.transform.position, rayHit.point);
			if(rayHit.collider.tag =="Combatant")
			{
				targetedCombatant = rayHit.collider.gameObject.GetComponent("Combatant") as Combatant;
				if(Input.GetMouseButtonDown(0) == true && weGonnaWaitOnThisShit <=0)
				{
					selectorModel.SetActive(false);
					return targetedCombatant;
				}
			}
		}

		if ((moveVertial > 0.9f || moveVertial < -0.9f || 
		     moveHorizontal > 0.9f || moveHorizontal < -0.9f) && weGonnaWaitOnThisShit <= 0) //System bywhich an enemy is selected via keyboard/controller
		{
			Vector3 axisDir = new Vector3(moveHorizontal, 0.0f, moveVertial);//Sets axis direction into a Vector3

			//float angle = Mathf.Acos(Vector3.Dot (Camera.main.transform.forward, axisDir));

			//Vector3 relativeFacing = new Vector3(Mathf.Cos((Mathf.PI/2)+angle), 0, Mathf.Sin ((Mathf.PI/2)+angle));

			//float angle = Vector3.Angle(mainMechanics.mainCamera.transform.forward, Vector3.zero);

			axisDir = mainMechanics.mainCamera.targetTracker.transform.rotation * axisDir; //Sets axis direction relative to camera
			List<Combatant> closenessList = new List<Combatant>();//Creates a list for testing combatant distances
			if(Mathf.Abs(axisDir.x) > Mathf.Abs(axisDir.z))
			{
				if(axisDir.x >0) //if axis is more left or right than up or down
				{
					for(int i = 0; i < combatantsInFight.Count; i++)
					{
						if(combatantsInFight[i].transform.position.x > targetedCombatant.transform.position.x)//If put in combatants not in opposite direction
						{
							float targetX = targetedCombatant.transform.position.x;
							float targetZ = targetedCombatant.transform.position.z;

							float inspectedX = combatantsInFight[i].transform.position.x;
							float inspectedZ = combatantsInFight[i].transform.position.z;

							Vector3 distance = new Vector3(Mathf.Abs(inspectedX - targetX), 0, Mathf.Abs(inspectedZ-targetZ));//Make a vector 3 based on their relative distances

							Combatant enemyFile = combatantsInFight[i].GetComponent("Combatant") as Combatant;

							enemyFile.distanceTracker = distance;
							closenessList.Add(enemyFile);
						}
					}
					if(closenessList.Count > 0)
					{
					closenessList = closenessList.OrderBy(d => d.distanceTracker.x).ToList();//compare relative distances and order the list
					closenessList = closenessList.OrderBy(d => d.distanceTracker.z).ToList();//do so again for the other axis

					targetedCombatant = closenessList[0];//change target to closest combatant
					weGonnaWaitOnThisShit = waitAmount;
					}
				}
				else if(axisDir.x < 0)
				{
					for(int i = 0; i < combatantsInFight.Count; i++)
					{
						if(combatantsInFight[i].transform.position.x < targetedCombatant.transform.position.x)
						{
							float targetX = targetedCombatant.transform.position.x;
							float targetZ = targetedCombatant.transform.position.z;
							
							float inspectedX = combatantsInFight[i].transform.position.x;
							float inspectedZ = combatantsInFight[i].transform.position.z;
							
							Vector3 distance = new Vector3(Mathf.Abs(inspectedX - targetX), 0, Mathf.Abs(inspectedZ-targetZ));
							
							Combatant enemyFile = combatantsInFight[i].GetComponent("Combatant") as Combatant;
							
							enemyFile.distanceTracker = distance;
							closenessList.Add(enemyFile);
						}
					}
					if(closenessList.Count > 0)
					{
					closenessList = closenessList.OrderBy(d => d.distanceTracker.x).ToList();
					closenessList = closenessList.OrderBy(d => d.distanceTracker.z).ToList();
					
					targetedCombatant = closenessList[0];
					weGonnaWaitOnThisShit = waitAmount;
					}
				}
			}
			else if(Mathf.Abs(axisDir.x) < Mathf.Abs(axisDir.z))
			{
				if(axisDir.z >0)
				{
					for(int i = 0; i < combatantsInFight.Count; i++)
					{
						if(combatantsInFight[i].transform.position.z > targetedCombatant.transform.position.z)
						{
							float targetX = targetedCombatant.transform.position.x;
							float targetZ = targetedCombatant.transform.position.z;
							
							float inspectedX = combatantsInFight[i].transform.position.x;
							float inspectedZ = combatantsInFight[i].transform.position.z;
							
							Vector3 distance = new Vector3(Mathf.Abs(inspectedX - targetX), 0, Mathf.Abs(inspectedZ-targetZ));
							
							Combatant enemyFile = combatantsInFight[i].GetComponent("Combatant") as Combatant;
							
							enemyFile.distanceTracker = distance;
							closenessList.Add(enemyFile);
						}
					}
					if(closenessList.Count > 0)
					{
					closenessList = closenessList.OrderBy(d => d.distanceTracker.z).ToList();
					closenessList = closenessList.OrderBy(d => d.distanceTracker.x).ToList();
					
					targetedCombatant = closenessList[0];
					weGonnaWaitOnThisShit = waitAmount;
					}
				}
				else if(axisDir.z < 0)
				{
					for(int i = 0; i < combatantsInFight.Count; i++)
					{
						if(combatantsInFight[i].transform.position.z < targetedCombatant.transform.position.z)
						{
							float targetX = targetedCombatant.transform.position.x;
							float targetZ = targetedCombatant.transform.position.z;
							
							float inspectedX = combatantsInFight[i].transform.position.x;
							float inspectedZ = combatantsInFight[i].transform.position.z;
							
							Vector3 distance = new Vector3(Mathf.Abs(inspectedX - targetX), 0, Mathf.Abs(inspectedZ-targetZ));
							
							Combatant enemyFile = combatantsInFight[i].GetComponent("Combatant") as Combatant;
							
							enemyFile.distanceTracker = distance;
							closenessList.Add(enemyFile);
						}
					}if(closenessList.Count > 0)
					{
					closenessList = closenessList.OrderBy(d => d.distanceTracker.z).ToList();
					closenessList = closenessList.OrderBy(d => d.distanceTracker.x).ToList();
					
					targetedCombatant = closenessList[0];
					weGonnaWaitOnThisShit = waitAmount;
					}
				}
			}

			//axisDir = new Vector3(moveHorizontal+angle, 0.0f, moveVertial+angle);
			/*
			ray = new Ray(targetedCombatant.transform.position, axisDir);
			RaycastHit hitLine;
			if(Physics.Raycast(ray, out hitLine))
			{
				Debug.DrawLine(targetedCombatant.transform.position, hitLine.point);
				if(hitLine.collider.tag == "Enemy")
				{
					targetedCombatant = hitLine.collider.gameObject;
					weGonnaWaitOnThisShit = waitAmount;
				}
			}*/

		}
		selectorModel.transform.position = new Vector3(targetedCombatant.transform.position.x, 
		                                               0.0f, 
		                                               targetedCombatant.transform.position.z);
		mainMechanics.mainCamera.target = targetedCombatant.gameObject;//Camera looks towards target
		return null;
	}

	public void endCurrentTurn()
	{
		if (initiativeList.Count > 0) 
		{
			initiativeList.RemoveAt (0);
		}
	}

	public void cancleSelect(Combatant turnTaker)
	{
		selectorModel.SetActive (false);
		turnTaker.isChoosing = false;
	}

	public void SpawnEnemies()
	{
		combatantsInFight.Add( Instantiate (enemyList[0], spawnPositions [0].transform.position, 
		            					spawnPositions [0].transform.rotation) as GameObject);
		combatantsInFight [0].name = "Enemy1";
		combatantsInFight.Add(Instantiate (enemyList[0], spawnPositions [1].transform.position, 
		                                spawnPositions [1].transform.rotation)as GameObject);
		combatantsInFight [1].name = "Enemy2";
		combatantsInFight.Add(Instantiate (enemyList[0], spawnPositions [2].transform.position, 
		                                spawnPositions [2].transform.rotation)as GameObject);
		combatantsInFight [2].name = "Enemy3";

		for(int i = 0; i < combatantsInFight.Count; i++)
		{
			Combatant enemyFile = combatantsInFight[i].GetComponent("Combatant") as Combatant;
			enemyFile.setBattleMap(this);
			initiativeList.Add( enemyFile.initToken.getInitCopy() );
		}
		for (int i = 0; i < player.partyList.Count; i++) 
		{
			combatantsInFight.Add (player.partyList[i].gameObject);
			initiativeList.Add (player.partyList[i].initToken.getInitCopy());
		}
		RollForInitiative ();
	}

	void RollForInitiative()
	{
		int peopleInCombat = 0;
		for (int i = 0; i < initiativeList.Count; i++) 
		{
			initiativeList[i].RollInitiative();
			peopleInCombat++;
		}
		for (int i = 0; i < peopleInCombat; i++) 
		{
			MakeInitPrediction( initiativeList[i], 4, false);
		}
		SortInitiative ();
	}

	public void MakeInitPrediction(InitiativeToken basePoint, int predictedTicks, bool sortAfterward)
	{
		for(int i = 0; i < initiativeList.Count; i++)
		{
			if(initiativeList[i].character == basePoint.character && initiativeList[i] != basePoint)
			{
				initiativeList.RemoveAt(i);
				i--;
			}
		}
		InitiativeToken prediction1 = basePoint.getInitCopy ();
		prediction1.setInit (basePoint.tickCount + predictedTicks);
		initiativeList.Add (prediction1);

		InitiativeToken prediction2 = basePoint.getInitCopy ();
		prediction2.setInit (prediction1.tickCount + basePoint.character.basicAttackTicks);
		initiativeList.Add (prediction2);

		
		InitiativeToken prediction3 = basePoint.getInitCopy ();
		prediction3.setInit (prediction2.tickCount + basePoint.character.basicAttackTicks);
		initiativeList.Add (prediction3);

		InitiativeToken prediction4 = basePoint.getInitCopy ();
		prediction4.setInit (prediction3.tickCount + basePoint.character.basicAttackTicks);
		initiativeList.Add (prediction4);
		
		
		InitiativeToken prediction5 = basePoint.getInitCopy ();
		prediction5.setInit (prediction4.tickCount + basePoint.character.basicAttackTicks);
		initiativeList.Add (prediction5);

		if (sortAfterward == true) 
		{
			SortInitiative ();
		}
	}

	void SortInitiative()
	{
		initiativeList = initiativeList.OrderByDescending (Combatant => Combatant.character.isPC).ToList ();
		initiativeList = initiativeList.OrderBy (Combatant => Combatant.tickCount).ToList ();
	}

	public void KillEnemy(GameObject deadEnemy, Combatant deadEnemyCombatant)
	{
		combatantsInFight.Remove (deadEnemy);
		for (int i = 0; i < initiativeList.Count; i++)  
		{
			if(initiativeList[i].character == deadEnemyCombatant)
			{
				initiativeList.RemoveAt(i);
				i--;
			}
		}
		for (int i = 0; i < combatantsInFight.Count + player.partyList.Count-1; i++) 
		{
			MakeInitPrediction(initiativeList[i], 4, false);
		}
		SortInitiative ();
	}

	void Update()
	{
		if (player.inCombat == true && player.waitingForFader == false)//Sets player to being in combat 
		{
			initiativeList [0].character.isTurn = true;
		}
	}

	void CompleteCombat()//Ends the combat encounter
	{
		initiativeList [0].character.isTurn = false;
		initiativeList.Clear ();
		combatantsInFight.Clear ();
		mainMechanics.GoingToActiveateMainMap ();
	}

	// Update is called once per frame
	void LateUpdate () 
	{
		if (player.inCombat == false) //Battle field class follows the player
		{
			transform.position = player.transform.position + offset;
		}
		if (player.inCombat == true && player.waitingForFader == false) 
		{
			int enemiesInFight = 0;
			for(int i = 0; i < combatantsInFight.Count; i++)
			{
				Combatant checkedFile = combatantsInFight[i].GetComponent("Combatant") as Combatant;
				if(checkedFile.isPC == false)
				{
					enemiesInFight++;
				}
			}
			if (enemiesInFight == 0) //If there are no enemies left, end the combat
			{
				CompleteCombat();
			}
		}
	}
}
