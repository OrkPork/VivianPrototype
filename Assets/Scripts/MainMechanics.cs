using UnityEngine;
using System.Collections;

public class MainMechanics : MonoBehaviour {

	public Vector3[] coordList = new Vector3[5]; //a 5 element array of Vector3's
	public int currentMap; //accesses coordList array
	public PlayerControls player;
	public GameObject battleMap;
	public GameObject mainMap;
	public CameraControls mainCamera;
	public BattleMap battleMechanics;
	public Fades fader;
    //they're private but playercontrols calls public functions that change them
	private bool waitingForFade = false;
	private bool goingToBattle = false;
	private bool goingToMap = false;


    
    /// <summary>
    /// Set the current map int for accessing coordList, sets playerposition
    /// </summary>
	public void SetMap(int map)
	{
		currentMap = map;
		//Application.LoadLevel (map);
		SetPlayerPosition ();
	}

    //Consider consolidating GoingToActiveateBattleMap() and GoingToActiveateMainMap() into one function
    /*
    void mapFade(bool enterBattle)
    {
        fader.BeginFade(+1);
        battleState(enterBattle);
        waitingForFade = true;
        player.waitingForFader = true;

    }
    */
    /// <summary>
    /// Calls fader, sets relevant values
    /// </summary>
    /// <param name="goingToMap">Sets to false</param>
    /// <param name="goingToBattle">Sets to true</param>
    /// <param name="waitingForFade">Sets to true</param>
    /// <param name="player.waitingForFader">Sets to true</param>
	public void GoingToActiveateBattleMap()
	{
		fader.BeginFade (+1);
		goingToBattle = true;
		goingToMap = false;
		waitingForFade = true;
		player.waitingForFader = true;
	}


    /// <summary>
    /// Calls fader, sets relevant values
    /// </summary>
    /// <param name="goingToMap">Sets to true</param>
    /// <param name="goingToBattle">Sets to false</param>
    /// <param name="waitingForFade">Sets to true</param>
    /// <param name="player.waitingForFader">Sets to true</param>
	public void GoingToActiveateMainMap()
	{
		fader.BeginFade (+1);
		goingToMap = true;
		goingToBattle = false;
		waitingForFade = true;
		player.waitingForFader = true;
	}

    //Consider consolidating activatebattlemap and deactivate battlemap into one function
    /*
    void battleState(bool entered)
    {
        battleMap.SetActive(entered);
        if(entered) battleMechanics.SpawnEnemies();
        mainMap.SetActive(!entered);
        player.removeForce();
        player.inCombat = entered;
        SetPlayerPosition();
        mainCamera.inCombat = entered;
        if (entered) mainCamera.combatAngleChange();
       
    }
    */

    /// <summary>
    /// Activates battlestate, deactivates mapstate, zeroes player velocity, calls enemy spawn, calls camera angle chance, sets relevant bools
    /// </summary>
    /// <param name="battleMap.SetActive">Sets to true, activates MainMechanics.battleMap</param>
    /// <param name="mainMap.SetActive">Sets to false, deactivates MainMechanics.mainMap</param>
    /// <param name="player.inCombat">Sets to true</param>
    /// <param name="mainCamera.inCombat">Sets to true</param>
	void ActivateBattleMap()
	{
		battleMap.SetActive (true);
		battleMechanics.SpawnEnemies ();
		mainMap.SetActive (false);
		player.removeForce ();
		player.inCombat = true;
		SetPlayerPosition ();
		mainCamera.inCombat = true;
		mainCamera.combatAngleChange ();
	}

    /// <summary>
    /// Activates mapstate, deactivates battlestate, zeroes player velocity, sets relevant bools
    /// </summary>
    /// <param name="battleMap.SetActive">Sets to false, deactivates MainMechanics.battleMap</param>
    /// <param name="mainMap.SetActive">Sets to true, activates MainMechanics.mainMap</param>
    /// <param name="player.inCombat">Sets to false</param>
    /// <param name="mainCamera.inCombat">Sets to false</param>
	void DeactivateBattleMap()
	{
		battleMap.SetActive (false);
		mainMap.SetActive (true);
		player.removeForce ();
		player.inCombat = false;
		SetPlayerPosition ();
		mainCamera.inCombat = false;
	}

	// Use this for initialization
	void Start () 
	{
		player.setMainMechs (this);
	}


    /// <summary>
    /// Checks if map is fading or recently faded, resets bools if recently faded if not then checks if we are entering map or battle to call the fader functions.
    /// </summary>
	void Update()
	{

		if (waitingForFade == true) 
		{
			if(fader.IsClear() == true)
			{
				waitingForFade = false;
				player.waitingForFader = false;
			}
			else if (fader.IsBlack() == true)
			{
				if(goingToBattle == true)
				{
					ActivateBattleMap();
					goingToBattle = false;
					fader.BeginFade(-1);
				}
				else if(goingToMap == true)
				{
					DeactivateBattleMap();
					goingToMap = false;
					fader.BeginFade(-1);
				}
			}
		}
	}
	
	// Update is called once per frame
    //OrkPork: Probably shouldn't be a fixed update since it's not physics related.
    /// <summary>
    /// Store the players position every tick of fixedupdate
    /// </summary>
	void FixedUpdate () 
	{
		if (player.inCombat == false) {
			coordList [currentMap] = player.getPosition ();
		}
	}

    /// <summary>
    /// Retrieves and sets last stored playerposition on the current map as the new playerposition.
    /// </summary>
	void SetPlayerPosition()
	{
		player.setPosition (coordList [currentMap]);
	}
}
