using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleMap : MonoBehaviour
{
	//PlayerControls: 
	//	1) controls/has values for player movement 
	//	2) produces/displays win text and 
	//	3) counts produces and displays score text.
	//	4) detects enemy encounters/collisions, flags them, calls MainMechanics

	public PlayerControls player;
	public MainMechanics mainMechanics;
	private Vector3 offset;
	public List<GameObject> combatantsInFight = new List<GameObject> ();
	public List<GameObject> enemyList = new List<GameObject> ();
	public List<InitiativeToken> initiativeList = new List<InitiativeToken> ();
	public GameObject[] spawnPositions = new GameObject[3];
	Combatant targetedCombatant;
	public GameObject selectorModel;
	float waitAmount = 0.15f;
	float weGonnaWaitOnThisShit;
	public Texture2D barOutline;
	List<EffectText> effectsList = new List<EffectText> ();

	Vector2 itemUIOffset = new Vector2 (0,0);

	//public bool activateEndCombatMenu = false;
	int xpGained;
	public List<InventorySlot> droppedItems = new List<InventorySlot>();

	public enum combatUiState
	{
		optionGet,
		endCombatScreen
	}

	public combatUiState uiState;

	// Use this for initialization
	void Start ()
	{
		offset = transform.position;
	}
	
	public void DisplayInventoryUI(GUIStyle style, Combatant selector)
	{
		Rect outline = new Rect (0, Screen.height / 2, Screen.width, Screen.height / 2);
		Rect itemInterior = new Rect (outline.x + 4, outline.y + 4, outline.width - 8, outline.height - 8);


		GUI.color = Color.black;
		GUI.DrawTexture (outline, Texture2D.whiteTexture);
		
		GUI.color = Color.white;
		GUI.DrawTexture (itemInterior, Texture2D.whiteTexture);

		//int numberOfColumns = 2;
		int columnX = 0;

		style.fontSize = ((Screen.height / 20));
		for(int i = 0; i < player.inventory.Count; i++)
		{
			GUI.color = Color.black;

			GUIContent nameCalc = new GUIContent (player.inventory[i].itemHere.itemName + " x255");
			Vector2 nameSize = style.CalcSize (nameCalc);

			Rect itemTextRect = new Rect(itemInterior.x+(itemInterior.width/40)+(itemInterior.width/2*(columnX))+itemUIOffset.x,
			                             itemInterior.y+((nameSize.y+(itemInterior.height/40))*((int)(i/2)))+itemUIOffset.y,
			                             nameSize.x,nameSize.y);



			if(itemTextRect.Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y))
			   && itemTextRect.y >= itemInterior.y && itemTextRect.y+itemTextRect.height <= itemInterior.y+itemInterior.height)
			{
				selector.itemChosenOption = i;
				if(Input.GetMouseButtonUp(0))
				{
					Item inspectedItem = player.inventory[selector.itemChosenOption].itemHere;
					selector.itemUse.setItem(player.inventory[selector.itemChosenOption]);
					selector.chosenAction = selector.itemUse;
					wait (0.1f);
					selectTarget (inspectedItem.isOffensive, selector);
				}
			}

			if(selector.itemChosenOption == i)
			{
				GUI.color = Color.black;
				GUI.DrawTexture(itemTextRect, Texture2D.whiteTexture);
				GUI.color = Color.white;
				if(itemTextRect.y+itemTextRect.height > itemInterior.y+itemInterior.height)
				{
					itemUIOffset.y -= itemTextRect.height+itemInterior.height/40;
				}
				else if(itemTextRect.y < itemInterior.y)
				{
					itemUIOffset.y += itemTextRect.height+itemInterior.height/40;
				}
			}

			if(itemTextRect.y >= itemInterior.y && itemTextRect.y+itemTextRect.height <= itemInterior.y+itemInterior.height)
			{
				GUI.Label(itemTextRect, player.inventory[i].itemHere.itemName+ " x" + player.inventory[i].ammountHere);
			}


			if(columnX >= 1)
			{
				columnX = 0;
			}
			else
			{
				columnX++;
			}
		}
	}

	public void addXpGain (int xpGain)
	{
		xpGained += xpGain;
	}

	public void addItemGain (Item item)
	{
		bool foundSlot = false;
		for(int i = 0; i < droppedItems.Count; i++)
		{
			if(droppedItems[i].itemHere.itemName == item.itemName)
			{
				droppedItems[i].addToHere(1);
				foundSlot = true;
			}
		}
		if (foundSlot == false)
		{
			InventorySlot adder = new InventorySlot();
			adder.set (item, 1);
			droppedItems.Add(adder);
		}

	}

	public void addEffectText (Vector3 position, string text, Color color)
	{
		EffectText effectText = new EffectText ();

		effectText.set (text, position, mainMechanics, color);
		effectsList.Add (effectText);
	}

	void DisplayEndCombatMenu (GUIStyle style)
	{
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		
		style.fontSize = ((Screen.height / 20));
		for (int i = 0; i < player.partyList.Count; i++) 
		{
			if (xpGained > 0) 
			{
				player.partyList [i].AddXp (1);
			}
			Rect portraitRect;
			GUIContent nameCalc = new GUIContent (player.partyList [i].creatureName);
			GUIContent levelCalc = new GUIContent ("Level: 99");
			Vector2 nameSize = style.CalcSize (nameCalc);
			Vector2 levelSize = style.CalcSize (levelCalc);
			if (i < 2) 
			{
				portraitRect = new Rect (Screen.width / 100, (Screen.height / 8 * (i + 1)) + (i * (Screen.height / 15)), Screen.height / 6, Screen.height / 18);
			} 
			else 
			{
				portraitRect = new Rect (Screen.width / 2 + Screen.width / 100, (Screen.height / 8 * (i - 1)) + ((i - 2) * ((Screen.height / 15))), Screen.height / 6, Screen.height / 18);
			}
			Rect nameTextRect = new Rect (portraitRect.x + portraitRect.width + (Screen.width / 100), portraitRect.y, nameSize.x, nameSize.y);
			Rect xpBarRect = new Rect (portraitRect.x, portraitRect.y + portraitRect.height + Screen.height / 50, Screen.width / 4, Screen.height / 20);
			Rect levelTextRect = new Rect (xpBarRect.x + xpBarRect.width - levelSize.x, nameTextRect.y, levelSize.x, levelSize.y); 
			GUI.DrawTexture (portraitRect, player.partyList [i].partyPortrait);
			GUI.contentColor = Color.black;
			GUI.Label (nameTextRect, player.partyList [i].creatureName);
			style.alignment = TextAnchor.UpperLeft;
			GUI.Label (levelTextRect, "Level: " + player.partyList [i].level);
			style.alignment = TextAnchor.UpperCenter;
			GUI.contentColor = Color.white;

			GUI.DrawTexture (xpBarRect, barOutline);
			
			int xpPercent = (int)(player.partyList [i].getXpPercent () * (xpBarRect.width - 2));
			GUI.color = Color.cyan;
			for (int p = 0; p < xpPercent; p++) 
			{
				GUI.DrawTexture (new Rect (xpBarRect.x + 1 + p, xpBarRect.y + 1, 1, xpBarRect.height - 2), Texture2D.whiteTexture);
			}
			GUI.color = Color.white;
		}
		if (xpGained > 0) 
		{
			xpGained--;
		}
		
		
		Rect outline = new Rect (0, Screen.height / 2, Screen.width, Screen.height / 2);
		Rect itemInterior = new Rect (outline.x + 4, outline.y + 4, outline.width - 8, outline.height - 8);
		
		GUI.color = Color.black;
		GUI.DrawTexture (outline, Texture2D.whiteTexture);
		
		GUI.color = Color.white;
		GUI.DrawTexture (itemInterior, Texture2D.whiteTexture);

		style.fontSize = ((Screen.height / 20));
		
		
		
		int columnX = 0;
		
		for(int i = 0; i < droppedItems.Count; i++)
		{
			GUI.color = Color.black;
			
			GUIContent nameCalc = new GUIContent (droppedItems[i].itemHere.itemName + " x255");
			Vector2 nameSize = style.CalcSize (nameCalc);
			
			Rect itemTextRect = new Rect(itemInterior.x+(itemInterior.width/40)+(itemInterior.width/2*(columnX))+itemUIOffset.x,
			                             itemInterior.y+((nameSize.y+(itemInterior.height/40))*((int)(i/2)))+itemUIOffset.y,
			                             nameSize.x,nameSize.y);
			
			if(itemTextRect.y >= itemInterior.y && itemTextRect.y+itemTextRect.height <= itemInterior.y+itemInterior.height)
			{
				GUI.Label(itemTextRect, droppedItems[i].itemHere.itemName+ " x" + droppedItems[i].ammountHere);
			}
			
			
			if(columnX >= 1)
			{
				columnX = 0;
			}
			else
			{
				columnX++;
			}
		}
		
		
	}
	
	void OnGUI ()
	{
		var centeredStyle = GUI.skin.GetStyle ("Label");
		centeredStyle.alignment = TextAnchor.UpperCenter;
		centeredStyle.fontSize = ((Screen.height / 40));
		
		if (uiState == combatUiState.endCombatScreen) 
		{
			DisplayEndCombatMenu (centeredStyle);
		} else if (initiativeList.Count > 10) 
		{ 
			for (int i = 0; i < 10; i++) 
			{
				Rect initRect = new Rect (Screen.width - ((Screen.height * 2) / 9),
				                     (Screen.height / 18) * i,
				                     ((Screen.height * 2) / 9),
				                          (Screen.height / 18));
				if (initiativeList [i].character == targetedCombatant) 
				{
					initRect.x = initRect.x - Screen.width / 100;
					GUI.contentColor = Color.red * 0.5f;
				}
				GUI.DrawTexture (initRect, initiativeList [i].intiativePortrait);//Draw initiative List
				GUI.TextArea (initRect, "" + initiativeList [i].tickCount + " " + initiativeList [i].character.creatureName);//Draw Tick Count
			
				GUI.contentColor = Color.white;
			
			}

			for (int i = 0; i < player.partyList.Count; i++) 
			{//Draw party's portraits, MP, and HP
				Rect portraitRect = new Rect (((Screen.height * 2) / 9) + (Screen.width / 100), Screen.height - ((Screen.height / 18) * (4 - i)), Screen.height / 6, Screen.height / 18);
				
				centeredStyle.fontSize = ((Screen.height / 20));
				GUIContent nameCalc = new GUIContent ("VIVIAN");
				Vector2 nameSize = centeredStyle.CalcSize (nameCalc);
				Rect nameTextRect = new Rect (portraitRect.x + portraitRect.width + 3, portraitRect.y, nameSize.x, nameSize.y);
				GUI.Label (nameTextRect, player.partyList [i].creatureName);
				
				centeredStyle.fontSize = ((Screen.height / 40));
				GUIContent textCalc = new GUIContent ("" + player.partyList [i].currentHP + "/" + player.partyList [i].maxHP);
				Vector2 textSize = centeredStyle.CalcSize (textCalc);//Gets the x and y size of a string in pixels


				GUI.DrawTexture (portraitRect, player.partyList [i].partyPortrait);//Draw Portrait
				Rect HPTextRect = new Rect (nameTextRect.x + nameTextRect.width + 5, portraitRect.y, Screen.height / 5, textSize.y);
				GUI.Label (HPTextRect, "" + player.partyList [i].currentHP + "/" + player.partyList [i].maxHP);//Draw HP current/Max

				Rect HPbar = new Rect (HPTextRect.x, portraitRect.y + HPTextRect.height - 3, HPTextRect.width, 5);
				GUI.DrawTexture (HPbar, barOutline);//Draws the HP bar background
				int HPBarPercent = (int)(player.partyList [i].getPercentHP () * (HPbar.width - 2));
				int HPpercent = (int)(player.partyList [i].getPercentHP () * 100);//Gets HP percent
				for (int a = 0; a < HPBarPercent; a++) 
				{//Fills and colors the HP bar
					if (HPpercent >= 75) 
					{
						GUI.color = Color.green;

					} 
					else if (HPpercent < 75 && HPpercent >= 30) 
					{
						GUI.color = Color.yellow;

					}
					else 
					{
						GUI.color = Color.red;
					}
					Rect HPbit = new Rect (HPbar.x + 1 + a, HPbar.y + 1, 1, 3);
					GUI.DrawTexture (HPbit, Texture2D.whiteTexture);
					GUI.color = Color.white;
				}

				//Same thing, but for MP
				Rect MPTextRect = new Rect (HPbar.x + HPbar.width + 5, HPTextRect.y, Screen.height / 8, textSize.y);
				GUI.Label (MPTextRect, "" + player.partyList [i].currentMP + "/" + player.partyList [i].maxMP);//Draw MP current/Max
				
				Rect MPbar = new Rect (MPTextRect.x, HPbar.y, MPTextRect.width, 5);
				GUI.DrawTexture (MPbar, barOutline);//Draws the MP bar background
				int MPpercent = (int)(player.partyList [i].getPercentMP () * (MPbar.width - 2));//Gets MP percent
				for (int a = 0; a < MPpercent; a++) 
				{//Fills and colors the MP bar
					GUI.color = Color.blue;
					Rect MPbit = new Rect (MPbar.x + 1 + a, MPbar.y + 1, 1, 3);
					GUI.DrawTexture (MPbit, Texture2D.whiteTexture);
					GUI.color = Color.white;
				}


			}
			if (initiativeList [0].character.isPC == true) 
			{
				centeredStyle.fontSize = Screen.height / 40;
				initiativeList [0].character.getGUI (centeredStyle);
			}
			
		}
		
		for (int i = 0; i < effectsList.Count; i++) 
		{
			if (effectsList [i].timer <= 0) 
			{
				effectsList.RemoveAt (i);
				i--;
			}
			else 
			{
				effectsList [i].Update (centeredStyle);
			}
		}
		
	}

	public void wait (float time)//Waits a number of seconds = to time
	{
		weGonnaWaitOnThisShit = time;
	}
	/// <summary>
	/// Mechanic for selecting attack targets.
	/// </summary>
	/// <param name="isOffensive"></param>
	/// <param name="selectingTarget"></param>
	/// <returns></returns>
	public Combatant selectTarget (bool isOffensive, Combatant selectingTarget)
	{
		if (Input.GetButtonUp ("Action") == true && weGonnaWaitOnThisShit <= 0) 
		{
			selectorModel.SetActive (false);
			selectingTarget.currentChoiceState = Combatant.choiceState.mainUI;
			return targetedCombatant;
		}
		if (weGonnaWaitOnThisShit > 0) 
		{
			weGonnaWaitOnThisShit -= 1.0f * Time.deltaTime;
		}
		selectorModel.SetActive (true);
		if (isOffensive == true && selectingTarget.isChoosing == false) 
		{
			targetedCombatant = combatantsInFight [0].GetComponent ("Combatant") as Combatant;
			selectingTarget.isChoosing = true;
		}
		else if (isOffensive == false && selectingTarget.isChoosing == false) 
		{
			targetedCombatant = selectingTarget;
			selectingTarget.isChoosing = true;
		}

		float moveHorizontal = Input.GetAxis ("Horizontal");//Get controler input
		float moveVertial = Input.GetAxis ("Vertical");

		RaycastHit rayHit;


		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out rayHit)) 
		{
			//Debug.DrawLine(mainMechanics.mainCamera.transform.position, rayHit.point);
			if (rayHit.collider.tag == "Combatant") 
			{
				targetedCombatant = rayHit.collider.gameObject.GetComponent ("Combatant") as Combatant;
				if (Input.GetMouseButtonDown (0) == true && weGonnaWaitOnThisShit <= 0) 
				{
					selectorModel.SetActive (false);
					return targetedCombatant;
				}
			}
		}

		if ((moveVertial > 0.9f || moveVertial < -0.9f || 
			moveHorizontal > 0.9f || moveHorizontal < -0.9f) && weGonnaWaitOnThisShit <= 0) 
		{ //System bywhich an enemy is selected via keyboard/controller
			Vector3 axisDir = new Vector3 (moveHorizontal, 0.0f, moveVertial);//Sets axis direction into a Vector3

			axisDir = mainMechanics.mainCamera.targetTracker.transform.rotation * axisDir; //Sets axis direction relative to camera
			List<Combatant> closenessList = new List<Combatant> ();//Creates a list for testing combatant distances
			if (Mathf.Abs (axisDir.x) > Mathf.Abs (axisDir.z)) 
			{
				if (axisDir.x > 0) 
				{ //if axis is more left or right than up or down
					for (int i = 0; i < combatantsInFight.Count; i++) 
					{
						
						float targetX = targetedCombatant.transform.position.x;
						float targetZ = targetedCombatant.transform.position.z;
						
						float inspectedX = combatantsInFight [i].transform.position.x;
						float inspectedZ = combatantsInFight [i].transform.position.z;
						
						Vector3 distance = new Vector3 (Mathf.Abs (inspectedX - targetX), 0, Mathf.Abs (inspectedZ - targetZ));//Make a vector 3 based on their relative distances



						if (combatantsInFight [i].transform.position.x > targetedCombatant.transform.position.x && distance.x >= distance.z) 
						{//If put in combatants not in opposite direction and within 45 degrees of chosen direction

							Combatant enemyFile = combatantsInFight [i].GetComponent ("Combatant") as Combatant;

							//enemyFile.distanceTracker = distance;
							closenessList.Add (enemyFile);
						}
					}
					if (closenessList.Count > 0) 
					{
						closenessList = closenessList.OrderBy (d => Vector3.Distance (d.gameObject.transform.position, targetedCombatant.transform.position)).ToList ();



						targetedCombatant = closenessList [0];//change target to closest combatant
						weGonnaWaitOnThisShit = waitAmount;
					}
				} 
				else if (axisDir.x < 0) 
				{
					for (int i = 0; i < combatantsInFight.Count; i++) 
					{
						float targetX = targetedCombatant.transform.position.x;
						float targetZ = targetedCombatant.transform.position.z;
						
						float inspectedX = combatantsInFight [i].transform.position.x;
						float inspectedZ = combatantsInFight [i].transform.position.z;
						
						Vector3 distance = new Vector3 (Mathf.Abs (inspectedX - targetX), 0, Mathf.Abs (inspectedZ - targetZ));//Make a vector 3 based on their relative distances

						if (combatantsInFight [i].transform.position.x < targetedCombatant.transform.position.x && distance.x >= distance.z) 
						{
							
							Combatant enemyFile = combatantsInFight [i].GetComponent ("Combatant") as Combatant;
							closenessList.Add (enemyFile);
						}
					}
					if (closenessList.Count > 0) 
					{
						closenessList = closenessList.OrderBy (d => Vector3.Distance (d.gameObject.transform.position, targetedCombatant.transform.position)).ToList ();
						targetedCombatant = closenessList [0];
						weGonnaWaitOnThisShit = waitAmount;
					}
				}
			} 
			else if (Mathf.Abs (axisDir.x) < Mathf.Abs (axisDir.z)) 
			{
				if (axisDir.z > 0) 
				{
					for (int i = 0; i < combatantsInFight.Count; i++) 
					{
						float targetX = targetedCombatant.transform.position.x;
						float targetZ = targetedCombatant.transform.position.z;
						
						float inspectedX = combatantsInFight [i].transform.position.x;
						float inspectedZ = combatantsInFight [i].transform.position.z;
						
						Vector3 distance = new Vector3 (Mathf.Abs (inspectedX - targetX), 0, Mathf.Abs (inspectedZ - targetZ));

						if (combatantsInFight [i].transform.position.z > targetedCombatant.transform.position.z && distance.x <= distance.z) 
						{
							
							Combatant enemyFile = combatantsInFight [i].GetComponent ("Combatant") as Combatant;
							closenessList.Add (enemyFile);
						}
					}
					if (closenessList.Count > 0) 
					{
						closenessList = closenessList.OrderBy (d => Vector3.Distance (d.gameObject.transform.position, targetedCombatant.transform.position)).ToList ();

					
						targetedCombatant = closenessList [0];
						weGonnaWaitOnThisShit = waitAmount;
					}
				} 
				else if (axisDir.z < 0) 
				{
					for (int i = 0; i < combatantsInFight.Count; i++) 
					{
						float targetX = targetedCombatant.transform.position.x;
						float targetZ = targetedCombatant.transform.position.z;
						
						float inspectedX = combatantsInFight [i].transform.position.x;
						float inspectedZ = combatantsInFight [i].transform.position.z;
						
						Vector3 distance = new Vector3 (Mathf.Abs (inspectedX - targetX), 0, Mathf.Abs (inspectedZ - targetZ));

						if (combatantsInFight [i].transform.position.z < targetedCombatant.transform.position.z && distance.x <= distance.z) 
						{
							
							Combatant enemyFile = combatantsInFight [i].GetComponent ("Combatant") as Combatant;
							closenessList.Add (enemyFile);
						}
					}
					if (closenessList.Count > 0) 
					{
						
						closenessList = closenessList.OrderBy (d => Vector3.Distance (d.gameObject.transform.position, targetedCombatant.transform.position)).ToList ();
						//closenessList = closenessList.OrderBy(d => d.distanceTracker.z).ToList();
						//closenessList = closenessList.OrderBy(d => d.distanceTracker.x).ToList();
					
						targetedCombatant = closenessList [0];
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
		selectorModel.transform.position = new Vector3 (targetedCombatant.transform.position.x, 
		                                               0.0f, 
		                                               targetedCombatant.transform.position.z);
		mainMechanics.mainCamera.target = targetedCombatant.gameObject;//Camera looks towards target
		return null;
	}

	public void endCurrentTurn ()
	{
		if (initiativeList.Count > 0) 
		{
			initiativeList.RemoveAt (0);
			mainMechanics.mainCamera.target = initiativeList [0].character.gameObject;
		}
	}

	public void cancleSelect (Combatant turnTaker)
	{
		selectorModel.SetActive (false);
		turnTaker.isChoosing = false;
		mainMechanics.mainCamera.target = turnTaker.gameObject;
		targetedCombatant = null;
	}

	public void SpawnEnemies ()
	{
		combatantsInFight.Add (Instantiate (enemyList [0], spawnPositions [0].transform.position, 
		            					spawnPositions [0].transform.rotation) as GameObject);
		combatantsInFight [0].name = "Enemy 1";

		combatantsInFight.Add (Instantiate (enemyList [0], spawnPositions [1].transform.position, 
		                                spawnPositions [1].transform.rotation)as GameObject);
		combatantsInFight [1].name = "Enemy2";

		combatantsInFight.Add (Instantiate (enemyList [0], spawnPositions [2].transform.position, 
		                                spawnPositions [2].transform.rotation)as GameObject);
		combatantsInFight [2].name = "Enemy3";

		for (int i = 0; i < combatantsInFight.Count; i++) {
			Combatant enemyFile = combatantsInFight [i].GetComponent ("Combatant") as Combatant;
			enemyFile.setBattleMap (this);
			enemyFile.creatureName = enemyFile.gameObject.name;
			initiativeList.Add (enemyFile.initToken.getInitCopy ());
		}
		for (int i = 0; i < player.partyList.Count; i++) {
			combatantsInFight.Add (player.partyList [i].gameObject);
			player.partyList [i].setBattleMap (this);
			initiativeList.Add (player.partyList [i].initToken.getInitCopy ());
		}
		RollForInitiative ();
	}

	void RollForInitiative ()
	{
		int peopleInCombat = 0;
		for (int i = 0; i < initiativeList.Count; i++) {
			initiativeList [i].RollInitiative ();
			peopleInCombat++;
		}
		for (int i = 0; i < peopleInCombat; i++) {
			MakeInitPrediction (initiativeList [i], 4, false);
		}
		SortInitiative ();
	}

	public void MakeInitPrediction (InitiativeToken basePoint, int predictedTicks, bool sortAfterward)
	{
		for (int i = 0; i < initiativeList.Count; i++) {
			if (initiativeList [i].character.gameObject == basePoint.character.gameObject && initiativeList [i] != basePoint) {
				initiativeList.RemoveAt (i);
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

		if (sortAfterward == true) {
			SortInitiative ();
		}
	}

	void SortInitiative ()
	{
		initiativeList = initiativeList.OrderByDescending (Combatant => Combatant.character.isPC).ToList ();
		initiativeList = initiativeList.OrderByDescending (Combatant => Combatant.character.isWaitingOnAnimation).ToList ();
		initiativeList = initiativeList.OrderBy (Combatant => Combatant.tickCount).ToList ();
	}

	public void RemoveFromInitiative (GameObject deadEnemy, Combatant deadEnemyCombatant)
	{
		combatantsInFight.Remove (deadEnemy);
		for (int i = 0; i < initiativeList.Count; i++) {
			if (initiativeList [i].character == deadEnemyCombatant) {
				initiativeList.RemoveAt (i);
				i--;
			}
		}
		for (int i = 0; i < combatantsInFight.Count; i++) {
			MakeInitPrediction (initiativeList [i], 4, false);
		}
		SortInitiative ();
	}

	void Update ()
	{
		if (player.inCombat == true && player.waitingForFader == false && uiState == combatUiState.optionGet) 
		{//Sets player to being in combat
			initiativeList [0].character.getTurn ();
		} 
		else if (uiState == combatUiState.endCombatScreen) 
		{
			
			if (Input.GetButtonUp ("Action") == true) 
			{
				if (xpGained > 0) 
				{
					for (int i = 0; i < player.partyList.Count; i++) 
					{
						player.partyList [i].AddXp (xpGained);
					}
					xpGained = 0;
				}
				CompleteCombat ();
			}
		}
	}

	void CompleteCombat ()//Ends the combat encounter
	{
		initiativeList.Clear ();
		combatantsInFight.Clear ();
		mainMechanics.GoingToActiveateMainMap ();
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		if (player.inCombat == false) { //Battle field class follows the player
			transform.position = player.transform.position + offset;
		}
		if (player.inCombat == true && player.waitingForFader == false) {
			int enemiesInFight = 0;
			for (int i = 0; i < combatantsInFight.Count; i++) {
				Combatant checkedFile = combatantsInFight [i].GetComponent ("Combatant") as Combatant;
				if (checkedFile.isPC == false) {
					enemiesInFight++;
				}
			}
			if (enemiesInFight == 0 && initiativeList [0].character.isReadyToEndTurn () == true) { //If there are no enemies left, end the combat
				uiState = combatUiState.endCombatScreen;
				effectsList.Clear();
				//activateEndCombatMenu = true;
				//CompleteCombat();
			}
		}
	}
}
