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
	float weGonnaWaitOnThisShit = 0;
	public Texture2D buttonBackground;
	public Texture2D buttonSelectBackground;
	public int basicAttackTicks = 4;
	int magicAttackTicks = 5;
	int itemUseTicks = 4;
	int defendSelfTicks = 3;
	float waitAmount = 0.1f;
	public bool isChoosing = false;
	bool actionIsOffensive;
	public Vector3 distanceTracker;
	public CombatAction basicAttack = new WhiteBallBasicAttack ();
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
	void Start ()
	{
		basicAttack.Start ();
	}

    /// <summary>
    /// Receives experience points, if exp is greater than XpToNextLevel it increases the cap
    /// </summary>
    /// <param name="addedXp"> Earned experience points</param>
	public void AddXp(int addedXp)
	{
		xp += addedXp;
		if(xp >= xpToNextLevel)
		{
			xp -= xpToNextLevel;
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

	public void getTurn ()
	{
		isDefending = false;
		if (isWaitingOnAnimation == true) 
		{
			chosenAction.Update ();
		} 
		else 
		{
			if (isPC == true) 
			{
				if (isChoosing == false) 
				{
					float moveVertial = Input.GetAxis ("Vertical");//Gets keyboard/controller Up/Down rating
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
						
					predictAction (chosenOption);//predict initiative change from chosen option
						
					
				} 
				else if (isChoosing == true) 
				{
					Combatant target = battleMechanics.selectTarget (actionIsOffensive, this);
					if (target != null) 
					{
						chosenAction.playAnimation (target, this);
					} 
					else if (Input.GetButtonUp ("Cancel")) 
					{
						battleMechanics.cancleSelect (this);
					}
				}
			} 
			else 
			{ //If isPC is false, default to AI;
				if (getPercentHP () >= 0.5f) {
					determineAction (0);
				} else if (getPercentHP () < 0.5f) {
					determineAction (3);
				}
			}
		}
	}

	public void Update ()
	{

	}

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

	void determineAction (int choice)
	{
		switch (choice) {
		case 0://attack
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], basicAttackTicks, true);
			actionIsOffensive = true;
			chosenAction = basicAttack;
			if (isPC == true) {
				battleMechanics.wait (0.1f);
				battleMechanics.selectTarget (actionIsOffensive, this);
			} 
			else 
			{
				int target = Random.Range (0, battleMechanics.player.partyList.Count);
				Combatant targetOfAI = battleMechanics.player.partyList [target];
				chosenAction.playAnimation (targetOfAI, this);
			}
			break;
			
		case 1://magic
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], magicAttackTicks, true);
			endTurn ();
			break;
			
		case 2://Item
			battleMechanics.MakeInitPrediction (battleMechanics.initiativeList [0], itemUseTicks, true);
			endTurn ();
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

	public void getGUI ()
	{


		if (isChoosing == false) {
			for (int i = 0; i < buttonAreaList.Count; i++) {
				string buttonName;
				if (chosenOption != i) {
					GUI.DrawTexture (buttonAreaList [i], buttonBackground);
				} else {
					GUI.DrawTexture (buttonAreaList [i], buttonSelectBackground);
				}

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
	}

	public void endTurn ()
	{
		isWaitingOnAnimation = false;
		isChoosing = false;
		battleMechanics.endCurrentTurn ();
	}

	public void Awake ()
	{
		initToken.character = this;
		
		initToken.intiativePortrait = intiativePortrait;
	}

	public void isKill ()
	{
		battleMechanics.RemoveFromInitiative (gameObject, this);
		Destroy (this.gameObject);
	}

	public void setBattleMap (BattleMap battleMechs)
	{
		battleMechanics = battleMechs;
	}

	public void beDealtDamage (int damage)
	{
		if (isDefending == true) 
		{
			damage = damage / 2;
		}

		battleMechanics.addEffectText(gameObject.transform.position+new Vector3(0,2,0), ""+damage);
		currentHP -= damage;

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
