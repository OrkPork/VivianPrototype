using UnityEngine;
using System.Collections;

public class WhiteBallBasicAttack : CombatAction {

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
		isMelee = true;
		damage = 100;
		range = 2;
	}

	public override void playAnimation (Combatant target, Combatant user)
	{
		base.playAnimation (target, user);
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update ();
		if (state == animationState.trueAnimate) 
		{
			Combatant enemyFile = targetEnemy.GetComponent("Combatant") as Combatant;
			enemyFile.beDealtDamage(damage);
			state = animationState.returnToPosition;
		}
	}

	public override void beginTrueAnimation ()
	{
		base.beginTrueAnimation ();
	}
}
