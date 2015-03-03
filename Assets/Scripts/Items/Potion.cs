using UnityEngine;
using System.Collections;

public class Potion : Item
{

	int potionStrength;

	public void set(int strength, string name, bool isOffensivePotion)
	{
		itemName = name;
		potionStrength = strength;
		isOffensive = isOffensivePotion;
	}

	public override void UseItem (Combatant user, Combatant target)
	{
		base.UseItem (user, target);
		target.beDealtDamage(potionStrength);

	}
}
