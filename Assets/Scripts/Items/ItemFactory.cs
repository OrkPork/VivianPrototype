using UnityEngine;
using System.Collections;

public class ItemFactory
{
	public Potion getPotion(int IDnumber)
	{
		
		Potion returnPotion = new Potion();
		switch(IDnumber)
		{

		case 0:
			returnPotion.set(-100, "Potion", false);
			return returnPotion;
		case 1:
			returnPotion.set(-1000, "Super Potion", false);
			return returnPotion;

		default:
			return null;
		}
	}

}
