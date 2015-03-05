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
		case 2:
			returnPotion.set(100, "Acid Potion", true);
			return returnPotion;
		case 3:
			returnPotion.set(1000, "Super Acid Potion", true);
			return returnPotion;
		case 4:
			returnPotion.set(-500, "Better than Average Potion", false);
			return returnPotion;
		case 5:
			returnPotion.set(-1, "Shit Potion", false);
			return returnPotion;
		case 6:
			returnPotion.set(1, "Shit Acid Potion", true);
			return returnPotion;
		case 7:
			returnPotion.set(0, "Useless Potion", false);
			return returnPotion;
		case 8:
			returnPotion.set(-10000, "Big Dick Potion", false);
			return returnPotion;
		case 9:
			returnPotion.set(-50, "Lesser Potion", false);
			return returnPotion;
		case 10:
			returnPotion.set(50, "Lesser Acid Potion", true);
			return returnPotion;
		case 11:
			returnPotion.set(-300, "Advanced Potion", true);
			return returnPotion;
		case 12:
			returnPotion.set(300, "Advanced Acid Potion", true);
			return returnPotion;
		case 13:
			returnPotion.set(500, "Better than Average Acid Potion", true);
			return returnPotion;
		case 14:
			returnPotion.set(1000, "Big Dicked Acid Potion", true);
			return returnPotion;
		case 15:
			returnPotion.set(-420, "Blaze It Potion", false);
			return returnPotion;
		case 16:
			returnPotion.set(420, "Blaze It Acid Potion", true);
			return returnPotion;
		case 17:
			returnPotion.set(1000000, "God Killer Acid Potion", true);
			return returnPotion;
		case 18:
			returnPotion.set(1000000, "God Healing Potion", false);
			return returnPotion;
		case 19:
			returnPotion.set(-10, "Meh Potion", false);
			return returnPotion;





			
			
		default:
			return null;
		}
	}

}
