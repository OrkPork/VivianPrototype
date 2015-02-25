using UnityEngine;
using System.Collections;

public class EffectText {

    /*
     * Apparently GUI is legacy the following link might be helpful in a different approach
     * maybe not since this is just a text effect and not user interface
     * http://unity3d.com/learn/tutorials/modules/beginner/ui/ui-canvas
     * 
     * */

    Vector3 affectedSpace;
	MainMechanics mainMechs;
	string text;
    //battlemaps reads the timer value and removes the effect when the timer is completed
	public float timer = 1;
	
	// Update is called once per frame
    /// <summary>
    /// Receives text and relevant params
    /// </summary>
    /// <param name="inputText">Text to be displayed</param>
    /// <param name="goingOffOf">Position to be centered around</param>
    /// <param name="main">Reference to main</param>
	public void set(string inputText, Vector3 goingOffOf, MainMechanics main)
	{
		text = inputText;
		affectedSpace = goingOffOf;
		mainMechs = main;
	}
    /// <summary>
    /// Centers text above a provided position
    /// </summary>
    /// <param name="centeredStyle">The GUIStyle used</param>
	public void Update (GUIStyle centeredStyle) 
	{
        //gets the coords relative to the screen not the world.
		Vector3 screenCoords = mainMechs.mainCamera.camera.WorldToScreenPoint(affectedSpace);
		
        //Fixes the co-ordinates of the text on screen, has the y co-ordinates drift downward over time
		Vector2 placement = new Vector3(screenCoords.x, Screen.height - screenCoords.y + ((timer*10)-10));

        //text size is relative to screensize
		centeredStyle.fontSize = ((Screen.height/20));

		GUIContent nameCalc = new GUIContent(text);

		GUI.contentColor = Color.red;

        //determine how big our text area will be
		Vector2 nameSize = centeredStyle.CalcSize(nameCalc);

		Rect textRectangle = new Rect(placement.x,placement.y, nameSize.x, nameSize.y); 

		GUI.Label(textRectangle, text);
		timer -= Time.deltaTime;
        
        //...makes the font smaller, I don't understand why, when GUI.Label is called it displays the text
        //and when the update function is called the font will be set larger 
		centeredStyle.fontSize = ((Screen.height/40));
	}
}
