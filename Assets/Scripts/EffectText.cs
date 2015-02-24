using UnityEngine;
using System.Collections;

public class EffectText {

	Vector3 affectedSpace;
	MainMechanics mainMechs;
	string text;
	public float timer = 1;
	
	// Update is called once per frame
    /// <summary>
    /// Receives text and relevant params
    /// </summary>
    /// <param name="inputText">Text to be displayed</param>
    /// <param name="goingOffOf">Position</param>
    /// <param name="main"></param>
	public void set(string inputText, Vector3 goingOffOf, MainMechanics main)
	{
		text = inputText;
		affectedSpace = goingOffOf;
		mainMechs = main;
	}

	public void Update (GUIStyle centeredStyle) 
	{
		Vector3 screenCoords = mainMechs.mainCamera.camera.WorldToScreenPoint(affectedSpace);
		
		Vector2 placement = new Vector3(screenCoords.x, Screen.height - screenCoords.y + ((timer*10)-10));

		centeredStyle.fontSize = ((Screen.height/20));

		GUIContent nameCalc = new GUIContent(text);

		GUI.contentColor = Color.red;

		Vector2 nameSize = centeredStyle.CalcSize(nameCalc);

		Rect textRectangle = new Rect(placement.x,placement.y, nameSize.x, nameSize.y); 

		GUI.Label(textRectangle, text);
		timer -= Time.deltaTime;
	}
}
