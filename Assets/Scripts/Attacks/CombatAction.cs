using UnityEngine;
using System.Collections;

public class CombatAction
{

	public int damage;
	public Animation combatAnimation;
	public bool isMelee;
	public GameObject bulletType;
	public Combatant user;
	public bool isAnimating;
	public bool trueAnimating = false;
	public bool returningToBasePosition;
	public Vector3 startingPosition;
	public Quaternion startingRotation;
	public Combatant targetEnemy;
	public float range;

	public bool isReadyToEndTurn()
	{
		if (isAnimating == false && isAnimating == false && trueAnimating == false) 
		{
			return true;
		}
		else
		{
			return false;
		}
		   
	}

	public virtual void Start () 
	{

	}

	public int getDamage ()
	{
		return damage;
	}

	public virtual void playAnimation (Combatant target, Combatant User)
	{
		user = User;
		user.isWaitingOnAnimation = true;
		targetEnemy = target;
		isAnimating = true;
		startingPosition = user.transform.position;
		startingRotation = user.transform.rotation;
	}

	public virtual void beginTrueAnimation()
	{
		trueAnimating = true;
	}

	public virtual void MoveToPosition(Vector3 targetPosition)
	{
		Quaternion currentRotation = user.transform.rotation;
		user.transform.LookAt(targetPosition);
		user.transform.position += user.transform.forward*15*Time.deltaTime;
		user.transform.rotation = currentRotation;
	}

	// Update is called once per frame
	public virtual void Update ()
	{
		if (isAnimating == true && trueAnimating == false) 
		{
			Quaternion currentRot = user.transform.rotation; //Remember current roation
			user.transform.LookAt (targetEnemy.transform.position);//set current rotation to desired rotation
			user.transform.rotation = Quaternion.Lerp (currentRot, user.transform.rotation, 2 * Time.deltaTime);//Interpolates towards target.
			if (isMelee == true) 
			{
				MoveToPosition(targetEnemy.transform.position);

				if (Vector3.Distance(user.transform.position, targetEnemy.transform.position) <= range) 
				{
					beginTrueAnimation ();
				}
			} 
			else 
			{
				beginTrueAnimation ();
			}
		} 
		else if (returningToBasePosition == true) 
		{
			if(user.transform.position == startingPosition && user.transform.rotation == startingRotation)
			{
				isAnimating = false;
				trueAnimating = false;
				returningToBasePosition = false;
				targetEnemy = null;
				user.endTurn();
			}
			else
			{
				bool positionReset = false;
				if(user.transform.position != startingPosition)
				{
					MoveToPosition(startingPosition);
					if(Vector3.Distance(user.transform.position, startingPosition) < 0.5f)
					{
						user.transform.position = startingPosition;
						positionReset = true;
					}
				}

				if(user.transform.rotation != startingRotation)
				{
					user.transform.rotation = Quaternion.Lerp (user.transform.rotation, startingRotation, 5 * Time.deltaTime);
					if(positionReset == true)
					{
						user.transform.rotation = startingRotation;
					}
				}
			}
		}
	}
}
















