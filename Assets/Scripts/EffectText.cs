using UnityEngine;
using System.Collections;

public class EffectText {

	GameObject objectAffected;
	MainMechanics mainMechs;
	string text;
	public int timer = 10;
	
	// Update is called once per frame
	public void set(string inputText, GameObject goingOffOf, MainMechanics main)
	{
		text = inputText;
		objectAffected = goingOffOf;
		mainMechs = main;
	}

	/*public void Update () 
	{
		Vector3 screenCoords = 
		
		Vector2 placement = new Vector3(screenCoords.x, Screen.width - screenCoords.y + (10-timer));

		Vector2 nameSize = GUIContent.CalcSize(text);

		Rect textRectangle = new Rect(placement.x,placement.y, nameSize.x, nameSize.y); 

		GUI.Label(textRectangle, text);
		timer--;
	}*/
}
