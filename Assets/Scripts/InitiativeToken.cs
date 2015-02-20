using UnityEngine;
using System.Collections;

public class InitiativeToken
{
	public int tickCount;
	public Texture intiativePortrait;
	public Combatant character;
	
	public void RollInitiative()
	{
		if (character.isPC == false)
			tickCount = Random.Range (0, 10);
		else
			tickCount = 0;
	}

	public void setCharacter(Combatant characterFile)
	{
		character = characterFile;
	}

	public void setEverything(int tick, Texture portrait, Combatant characterFile)
	{
		intiativePortrait = portrait;
		tickCount = tick;
		character = characterFile;
	}

	public void setInit(int init)
	{
		tickCount = init;
	}

	public InitiativeToken getInitCopy()
	{
		InitiativeToken returner = new InitiativeToken ();
		returner.setEverything (tickCount, intiativePortrait, character);
		return returner;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
