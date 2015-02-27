using UnityEngine;
using System.Collections;

public class Potion : Item
{

	int potionStrength;

	public void set(int strength, string name)
	{
		name = itemName;
		potionStrength = strength;
	}

	public override void UseItem (Combatant user, Combatant target)
	{
		base.UseItem (user, target);
		target.beDealtDamage(potionStrength);

	}
}
