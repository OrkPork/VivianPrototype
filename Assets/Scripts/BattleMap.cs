using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleMap : MonoBehaviour {
	//PlayerControls: 
	//	1) controls/has values for player movement 
	//	2) produces/displays win text and 
	//	3) counts produces and displays score text.
	//	4) detects enemy encounters/collisions, flags them, calls MainMechanics
	//	

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
	public Texture2D barOutline;
	List<EffectText> effectsList = new List<EffectText>();
	// Use this for initialization
	void Start () 
	{
		offset = transform.position;
	}

	public void addEffectText(GameObject position, string text)
	{
		EffectText effectText = new EffectText();

		effectText.set (text, position, mainMechanics);
		effectsList.Add(effectText);
	}

	void OnGUI()
	{
		//
		if (initiativeList.Count > 10) 
		{ 
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.fontSize = ((Screen.height/40));
			for (int i = 0; i < 10; i++) 
			{
				Rect initRect = new Rect (Screen.width - ((Screen.height * 2) / 9),
				                     (Screen.height / 18) * i,
				                     ((Screen.height * 2) / 9),
				                          (Screen.height / 18));
				if(initiativeList[i].character == targetedCombatant)
				{
					initRect.x = initRect.x-Screen.width/100;
					GUI.contentColor = Color.red*0.5f;
				}
				GUI.DrawTexture (initRect, initiativeList [i].intiativePortrait);//Draw initiative List
				GUI.TextArea (initRect, "" + initiativeList [i].tickCount + " " + initiativeList [i].character.name);//Draw Tick Count
			
				GUI.contentColor = Color.white;
			
			}

			for(int i = 0; i < player.partyList.Count; i++)//Draw party's portraits, MP, and HP
			{
				Rect portraitRect = new Rect(((Screen.height * 2) / 9)+(Screen.width/100), Screen.height-((Screen.height/18)*(4-i)), Screen.height/6, Screen.height/18);
				
				centeredStyle.fontSize = ((Screen.height/20));
				GUIContent nameCalc = new GUIContent("VIVIAN");
				Vector2 nameSize = centeredStyle.CalcSize(nameCalc);
				Rect nameTextRect = new Rect(portraitRect.x+portraitRect.width+3, portraitRect.y, nameSize.x, nameSize.y);
				GUI.Label(nameTextRect,player.partyList[i].name);
				
				centeredStyle.fontSize = ((Screen.height/40));
				GUIContent textCalc = new GUIContent(""+player.partyList[i].currentHP+"/"+player.partyList[i].maxHP);
				Vector2 textSize = centeredStyle.CalcSize(textCalc);//Gets the x and y size of a string in pixels


				GUI.DrawTexture(portraitRect ,player.partyList[i].partyPortrait);//Draw Portrait
				Rect HPTextRect = new Rect(nameTextRect.x+nameTextRect.width+5, portraitRect.y, Screen.height/5, textSize.y);
				GUI.Label(HPTextRect, ""+player.partyList[i].currentHP+"/"+player.partyList[i].maxHP);//Draw HP current/Max

				Rect HPbar = new Rect(HPTextRect.x,portraitRect.y+HPTextRect.height-3, HPTextRect.width, 5);
				GUI.DrawTexture(HPbar, barOutline);//Draws the HP bar background
				int HPBarPercent = (int)(player.partyList[i].getPercentHP()*(HPbar.width-2));
				int HPpercent = (int)(player.partyList[i].getPercentHP()*100);//Gets HP percent
				for(int a = 0; a < HPBarPercent; a++)//Fills and colors the HP bar
				{
					if(HPpercent >= 75)
					{
						GUI.color = Color.green;

					}
					else if (HPpercent < 75 && HPpercent >=30)
					{
						GUI.color = Color.yellow;

					}
					else
					{
					GUI.color = Color.red;
					}
					Rect HPbit = new Rect(HPbar.x+1+a, HPbar.y+1, 1, 3);
					GUI.DrawTexture(HPbit, Texture2D.whiteTexture);
					GUI.color = Color.white;
				}

				//Same thing, but for MP
				Rect MPTextRect = new Rect(HPbar.x+HPbar.width+5, HPTextRect.y, Screen.height/8, textSize.y);
				GUI.Label(MPTextRect, ""+player.partyList[i].currentMP+"/"+player.partyList[i].maxMP);//Draw MP current/Max
				
				Rect MPbar = new Rect(MPTextRect.x,HPbar.y, MPTextRect.width, 5);
				GUI.DrawTexture(MPbar, barOutline);//Draws the MP bar background
				int MPpercent = (int)(player.partyList[i].getPercentMP()*(MPbar.width-2));//Gets MP percent
				for(int a = 0; a < MPpercent; a++)//Fills and colors the MP bar
				{
					GUI.color = Color.blue;
					Rect MPbit = new Rect(MPbar.x+1+a, MPbar.y+1, 1, 3);
					GUI.DrawTexture(MPbit, Texture2D.whiteTexture);
					GUI.color = Color.white;
				}


			}
				if(initiativeList[0].character.isPC == true)
				{
				centeredStyle.fontSize = Screen.height/40;
					initiativeList[0].character.getGUI();
				}
			
		}
		
	}

	public void wait(float time)//Waits a number of seconds = to time
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
			mainMechanics.mainCamera.target=initiativeList[0].character.gameObject;
		}
	}

	public void cancleSelect(Combatant turnTaker)
	{
		selectorModel.SetActive (false);
		turnTaker.isChoosing = false;
		mainMechanics.mainCamera.target = turnTaker.gameObject;
		targetedCombatant = null;
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

	public void RemoveFromInitiative(GameObject deadEnemy, Combatant deadEnemyCombatant)
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
			initiativeList [0].character.getTurn();
		}
	}

	void CompleteCombat()//Ends the combat encounter
	{
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
			if (enemiesInFight == 0 && initiativeList[0].character.isReadyToEndTurn() == true) //If there are no enemies left, end the combat
			{
				CompleteCombat();
			}
		}
	}
}
