using UnityEngine;
using System.Collections;

public class MainMechanics : MonoBehaviour {

	public Vector3[] coordList = new Vector3[5];
	public int currentMap;
	public PlayerControls player;
	public GameObject battleMap;
	public GameObject mainMap;
	public CameraControls mainCamera;
	public BattleMap battleMechanics;
	public Fades fader;
	private bool waitingForFade = false;
	private bool goingToBattle = false;
	private bool goingToMap = false;

	public void SetMap(int map)
	{
		currentMap = map;
		//Application.LoadLevel (map);
		SetPlayerPosition ();
	}

	public void GoingToActiveateBattleMap()
	{
		fader.BeginFade (+1);
		goingToBattle = true;
		goingToMap = false;
		waitingForFade = true;
		player.waitingForFader = true;
	}
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
    void BattleMapState(bool entered)
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
	void FixedUpdate () 
	{
		if (player.inCombat == false) {
			coordList [currentMap] = player.getPosition ();
		}
	}

	void SetPlayerPosition()
	{
		player.setPosition (coordList [currentMap]);
	}
}
