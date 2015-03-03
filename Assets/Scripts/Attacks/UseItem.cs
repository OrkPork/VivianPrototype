using UnityEngine;
using System.Collections;

public class UseItem : CombatAction 
{
	InventorySlot itemToUse;

	public override void Start () 
	{
		base.Start ();
		isMelee = false;
	}
	
	public override void playAnimation (Combatant target, Combatant user)
	{
		base.playAnimation (target, user);
		itemToUse.addToHere(-1);
	}

	public void setItem(InventorySlot item)
	{
		itemToUse = item;
	}
	

	public override void Update ()
	{
		base.Update ();
		if (state == animationState.trueAnimate) 
		{
			itemToUse.itemHere.UseItem(user, targetEnemy);
			state = animationState.returnToPosition;
			if(user.isPC == true)
			{
				user.battleMechanics.player.checkInventorySlots();
			}
			itemToUse = null;
		}
	}
	
	public override void beginTrueAnimation ()
	{
		base.beginTrueAnimation ();
	}

}
