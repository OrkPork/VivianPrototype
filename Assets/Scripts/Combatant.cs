using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combatant : MonoBehaviour
{
	public int level = 1;
	public int currentHP;
	public int maxHP;
	public int currentMP;
	public int maxMP;
	public bool inInitiative = false;
	public string creatureName;
	public BattleMap battleMechanics;
	public InitiativeToken initToken = new InitiativeToken ();
	public Texture2D intiativePortrait;
	public Texture2D partyPortrait;
	public bool isPC;
	List<Rect> buttonAreaList = new List<Rect> ();
	int chosenOption = 0;
	public int itemChosenOption = 0;
	float weGonnaWaitOnThisShit = 0;
	public Texture2D buttonBackground;
	public Texture2D buttonSelectBackground;
	public int basicAttackTicks = 4;
	int magicAttackTicks = 5;
	int itemUseTicks = 4;
	int defendSelfTicks = 3;
	float waitAmount = 0.1f;
	public bool isChoosing = false;

	public enum choiceState
	{
		mainUI,
		chooseItem,
		chooseSpell
	}
	public choiceState currentChoiceState;

	bool actionIsOffensive;
	public Vector3 distanceTracker;
	CombatAction basicAttack = new WhiteBallBasicAttack ();
	public UseItem itemUse = new UseItem();
	public CombatAction chosenAction;

	public bool isWaitingOnAnimation = false;
	bool isDefending = false;
	int xp = 0;
	int xpToNextLevel = 1000;
	public int xpValue;

	public bool isReadyToEndTurn()
	{
		
		if(chosenAction == null)
		{
			return true;
		}
		else
		{
			return chosenAction.isReadyToEndTurn ();
		}
	}

    //calls basicattack, a function that only calls a function?
	//MonoBehaviors call Start() at the beginning of the system load. Non-MonoBehaviors do not.
	//Normally, this would call the .Start() function of every attack ability the combatant has.
	void Start ()
	{
		basicAttack.Start ();
		itemUse.Start();
	}

    /// <summary>
    /// Receives experience points, if exp is greater than XpToNextLevel it increases the cap
    /// </summary>
    /// <param name="addedXp"> Earned experience points</param>
	public void AddXp(int addedXp)
	{
		xp += addedXp;
		while(xp >= xpToNextLevel)
		{
			xp -= xpToNextLevel;
			level++;
			xpToNextLevel += 1000;
		}
	}

    /// <summary>
    /// Percentage of the experience cap currently earned
    /// </summary>
    /// <returns>float</returns>
	public float getXpPercent()
	{
		float percent = (float)xp / (float)xpToNextLevel;
		return percent;
	}

    /// <summary>
    /// Percentage of the HP cap currently at
    /// </summary>
    /// <returns>float</returns>
	public float getPercentHP ()
	{
		float percent = (float)currentHP / (float)maxHP;
		return percent;
	}

    /// <summary>
    /// Percentage of the MP cap currently at
    /// </summary>
    /// <returns>float</returns>
	public float getPercentMP ()
	{
		float percent = currentMP / maxMP;
		return percent;
	}

    /// <summary>
    /// Enables menu mechanics if in proper game state, there is a delay to keep you from fast scrolling
    /// </summary>
	public void getTurn ()
	{
        //we are not defending against an attack
		isDefending = false;
        //if animating skip menu mechanics and resolve animation.
		if (isWaitingOnAnimation == true) 
		{
			chosenAction.Update ();
		} 
		else 
		{
            //if this combatant is the player character
			if (isPC == true) 
			{
                //if we haven't clicked any buttons
				if (isChoosing == false) 
				{
					float moveVertial = Input.GetAxis ("Vertical");//Gets keyboard/controller Up/Down rating
					float moveHorizontal = Input.GetAxis ("Horizontal");//Gets keyboard/controller Left/Right rating

					switch(currentChoiceState)
					{

					case choiceState.mainUI:
					buttonAreaList.Clear ();//Clears buttons
					for (int i = 4; i != 0; i--) 
					{//Reforms buttons
						Rect buttonRect = new Rect (0,
						                            Screen.height - (Screen.height / 18) * i,
						                            ((Screen.height * 2) / 9),
						                            (Screen.height / 18));
						buttonAreaList.Add (buttonRect);
					}
					Vector2 mousePos = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);//get mouse positions
						
					//keyboard/controller controls	
					if (weGonnaWaitOnThisShit <= 0) 
					{//Wait to make sure the system doesn't scroll through the menu 1 button per frame
							
						if (moveVertial < -0.8f) 
						{//If down is greater than 80%
							chosenOption++;
							weGonnaWaitOnThisShit = waitAmount;//start waiting
							if (chosenOption > buttonAreaList.Count - 1) 
							{
								chosenOption = 0;
							}
						} else if (moveVertial > 0.8f) 
						{//If up is greater than 80%
							chosenOption--;
							weGonnaWaitOnThisShit = waitAmount;//start waiting
							if (chosenOption < 0) 
							{
								chosenOption = buttonAreaList.Count - 1;
							}
						}
							
					} 
					else 
					{
						weGonnaWaitOnThisShit -= 1 * Time.deltaTime;//Count down wait timer
					}
					//Mouse controls	
					for (int i = 0; i < buttonAreaList.Count; i++) 
					{
						if (buttonAreaList [i].Contains (mousePos)) 
						{//If the mouse is within the area of a button
							chosenOption = i;//set selected button to that button
							if (Input.GetMouseButtonUp (0) == true) 
							{//If the mouse had clicked
								determineAction (chosenOption);//perform the action
							}
						}
					}
						
					if (Input.GetButtonUp ("Action") == true) 
					{//If "Action" button/key is hit
						determineAction (chosenOption); //do chosen action
					}
						
					predictAction (chosenOption);//recalculate initiative order
						
						
						break;
						//End MainUI
					case choiceState.chooseItem:

						if (Input.GetButtonUp ("Cancel") == true) 
						{
							currentChoiceState = choiceState.mainUI;
							battleMechanics.uiState = BattleMap.combatUiState.optionGet;
						}
						else if (Input.GetButtonUp ("Action") == true) 
						{
							Item inspectedItem = battleMechanics.player.inventory[itemChosenOption].itemHere;
							itemUse.setItem(battleMechanics.player.inventory[itemChosenOption]);
							chosenAction = itemUse;
							battleMechanics.wait (0.1f);
							battleMechanics.selectTarget (inspectedItem.isOffensive, this);
						}

						if(weGonnaWaitOnThisShit <= 0)
						{
							bool dirChosen = false;
							if(Mathf.Abs(moveVertial) > Mathf.Abs(moveHorizontal) && Mathf.Abs(moveVertial) > 0.8)
							{
								if(moveVertial > 0)
								{
									itemChosenOption -= 2;
								}
								else if(moveVertial < 0)
								{
									itemChosenOption += 2;
								}
								dirChosen = true;
							}
							else if(Mathf.Abs(moveVertial) < Mathf.Abs(moveHorizontal) && Mathf.Abs(moveHorizontal) > 0.8)
							{
								if(moveHorizontal > 0)
								{
									itemChosenOption += 1;
								}
								else if(moveHorizontal < 0)
								{
									itemChosenOption -= 1;
								}
								dirChosen = true;
							}
							if(dirChosen == true)
							{
								weGonnaWaitOnThisShit = waitAmount;
							}
						}
						else
						{
							weGonnaWaitOnThisShit -= Time.deltaTime;
						}

						
						if(itemChosenOption < 0)
						{
							itemChosenOption = 0;
						}
						else if(itemChosenOption > battleMechanics.player.inventory.Count-1)
						{
							itemChosenOption = battleMechanics.player.inventory.Count-1;
						}
						
						//End chooseItem
						break;

					case choiceState.chooseSpell:



						if (Input.GetButtonUp ("Cancel") == true) 
						{
							currentChoiceState = choiceState.mainUI;
						}

						//End chooseSpell
						break;
					}
				}
				else if (isChoosing == true) 
				{
                    //Call target selection function and save reference to targeted selection
					Combatant target = battleMechanics.selectTarget (actionIsOffensive, this);
                    //if the player actually selects a target 
                    //(if the player has not yet selected a target battlemap returns null)
                    //target selection doesn't change the game state to a target selection mode so this function is constantly called
                    //and needs a fail state inbetween people actually making a choice or exiting choice selection loop.
					if (target != null) 
					{
						chosenAction.playAnimation (target, this);
					} 
					else if (Input.GetButtonUp ("Cancel")) 
					{
                        //exits choice selection loop
						battleMechanics.cancleSelect (this);
					}
				}
			} 
			else 
			{ //If isPC is false, default to AI;
                //AI logic determined by HP level
				if (getPercentHP () >= 0.5f) {
					determineAction (0);
				} else if (getPercentHP () < 0.5f) {
					determineAction (3);
				}
			}
		}
	}

    /// <summary>
    /// Unused/Empty.
    /// </summary>
	public void Update ()
	{

	}

    /// <summary>
    /// Submits action speed values for determining initiative order
    /// </summary>
    /// <param name="choice">int reference to an action</param>
	void predictAction (int choice)
	{
		switch (choice) {
		case 0://attack
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], basicAttackTicks, true);
			break;
			
		case 1://magic
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], magicAttackTicks, true);
			break;
			
		case 2://Item
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], itemUseTicks, true);
			break;
			
		case 3://Defend
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], defendSelfTicks, true);
			break;
			
		default://error
			
			break;
		}
	}

    /// <summary>
    /// Submits action speed values for determining initiative order and resolves chosen action
    /// </summary>
    /// <param name="choice">int reference to an action</param>
	void determineAction (int choice)
	{
		switch (choice) {
		case 0://attack
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], basicAttackTicks, true);
			actionIsOffensive = true;
			chosenAction = basicAttack;
			if (isPC == true) 
			{
				battleMechanics.wait (0.1f);
				battleMechanics.selectTarget (actionIsOffensive, this);
			} 
			else 
			{
                //more enemy AI scattered about!
                //AI picks a target at random
				int target = Random.Range (0, battleMechanics.player.partyList.Count);
				Combatant targetOfAI = battleMechanics.player.partyList [target];
				chosenAction.playAnimation (targetOfAI, this);
			}
			break;
			//rest of the commands do nothing atm
		case 1://magic
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], magicAttackTicks, true);
			endTurn ();
			break;
			
		case 2://Item
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], itemUseTicks, true);
			currentChoiceState = choiceState.chooseItem;
			break;
			
		case 3://Defend
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], defendSelfTicks, true);
			isDefending = true;
			endTurn ();
			break;
			
		default://error
			
			break;
		}
	}

    /// <summary>
    /// Get appearance change in combatGUI
    /// </summary>
	public void getGUI (GUIStyle style)
	{


		if (isChoosing == false) 
		{
			if(currentChoiceState == choiceState.mainUI)
			{
            //cycle through buttons changing their appearance
			for (int i = 0; i < buttonAreaList.Count; i++) 
				{
				string buttonName;
				if (chosenOption != i) 
					{
                    	//Not a chosen button normal background state
						GUI.DrawTexture (buttonAreaList [i], buttonBackground);
				
					} 
					else 
					{
                    	//We found the selected button
						GUI.DrawTexture (buttonAreaList [i], buttonSelectBackground);
				
					}
                	//find the buttons name hardcoded
				switch (i) {
				case 0:
					buttonName = "Attack";
					break;
				case 1:
					buttonName = "Magic";
					break;
				case 2:
					buttonName = "Item";
					break;
				case 3:
					buttonName = "Defend";
					break;
				default:
					buttonName = "ButtonError";
					break;
				}
				GUI.contentColor = Color.red;
				GUI.Label (buttonAreaList [i], buttonName);
				GUI.contentColor = Color.white;
				}
			}

			else if(currentChoiceState == choiceState.chooseItem)
			{
				battleMechanics.DisplayInventoryUI(style, this);
			}
		}
	}

    /// <summary>
    /// Sets bool values, calls battlemechanics to end turn
    /// </summary>
	public void endTurn ()
	{
		isWaitingOnAnimation = false;
		isChoosing = false;
		battleMechanics.endCurrentTurn ();
	}

    /// <summary>
    /// Gives initToken a reference to this character and its portrait
    /// </summary>
	public void Awake ()
	{
		initToken.character = this;
		
		initToken.intiativePortrait = intiativePortrait;
	}

    /// <summary>
    /// Rmeoves this combatant from initiative list then destroys it
    /// </summary>
	public void isKill ()
	{
		battleMechanics.RemoveFromInitiative (gameObject, this);
		Destroy (this.gameObject);
	}


    /// <summary>
    /// Sets reference to battlemap
    /// </summary>
    /// <param name="battleMechs">provided BattleMap reference</param>
	public void setBattleMap (BattleMap battleMechs)
	{
		battleMechanics = battleMechs;
	}

    /// <summary>
    /// Calculates damage, if this is an enemy award exp if it's a player "Man down!"
    /// </summary>
    /// <param name="damage">damage taken</param>
	public void beDealtDamage (int damage)
	{
		if (isDefending == true && damage > 0) 
		{
			damage = damage / 2;
		}

		Color color = Color.white;
		
		currentHP -= damage;
		
		if(damage > 0)
		{
			color = Color.red;
		}
		else if (damage < 0)
		{
			color = Color.green;
			damage = damage * (-1);
		}

		battleMechanics.addEffectText(gameObject.transform.position+new Vector3(0,2,0), ""+damage, color);

		if(currentHP > maxHP)
		{
			currentHP = maxHP;
		}

		if (currentHP < 0) 
		{
			currentHP = 0;
		}
		if (currentHP <= 0) 
		{
			if (isPC == false) 
			{
				battleMechanics.addXpGain(xpValue);
				isKill ();
			} 
			else 
			{
				Debug.Log ("Man down!");
			}
		}
	}
}
