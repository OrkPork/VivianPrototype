using UnityEngine;
using System.Collections;

public class Fades : MonoBehaviour 
{
	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;

		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public bool IsClear()
	{
		if (alpha == 0.0f) 
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsBlack()
	{
		if (alpha == 1.0f) 
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void BeginFade (int direction)
	{
		fadeDir = direction;
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
	}
}
