using UnityEngine;
using System.Collections;

public class AIStateMachine : MonoBehaviour {

	// Use this for initialization 
	public enum enemyStates{
		IDLE,
		RECENTLY_ENGAGED,
		ATTACK,
		ENRAGED
	}
	public float distanceToAttack = 13; //The distance at which the enemy switches from IDLE to ATTACK
	public float health = 100; 
	public float timeToBeEnraged = 5; //Amount of seconds he will be in ENRAGED state
	float distanceFromPlayer;
	float startTime;
	GameObject player; 
	public enemyStates currentState = enemyStates.IDLE;
	
	void Start () { 
		player = GameObject.Find("SoldierPlayer"); 
	}
	 
	void Update () { 
		distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);  
		
		switch(currentState)
		{
		case enemyStates.IDLE :  
			//From Idle, the enemy can ONLY go from IDLE to ATTACK.    
			if (distanceFromPlayer <= distanceToAttack)
			{
				changeStates(enemyStates.ATTACK);	
			}
			else
			{
				Idle();
			}
			break;
		case enemyStates.RECENTLY_ENGAGED:
			/*In this state, there are two possible states that can be switched to.  The enemy can go either to 
			 * IDLE if 10 seconds has passed, or back to attack if the player comes back within range. 
			*/ 
			if (distanceFromPlayer <= distanceToAttack)
			{
				changeStates(enemyStates.ATTACK);	
			}
			else if (Time.time - startTime >= timeToBeEnraged)
			{
				changeStates(enemyStates.IDLE);	
			}
			else
			{
				RecentlyEngaged();	
			} 
			break;
		case enemyStates.ATTACK:
			/*In this state, the enemy can switch to two possible states.  The first switch that is 
			 * possible is RECENTLY_ENGAGED.  This will trigger when the player goes out of range of the enemy.
			 * 
			 * The second possible state is ENRAGED, in which the enemy will switch to if his life goes below a 
			 * certain threshold.
			*/
			if (distanceFromPlayer > distanceToAttack)
			{
				startTime = Time.time; //Start the 10 second RECENTLY ENGAGED timer
				changeStates(enemyStates.RECENTLY_ENGAGED);	
			}
			else if (health <= 20)
			{
				changeStates(enemyStates.ENRAGED);	
			}
			else
			{
				Attack();
			} 
			break;
		case enemyStates.ENRAGED:
			//Once in the enraged state, he will no longer switch to any other states, will fight until the end.
			Enraged();
			break;
		} 
	}
	
	void Idle()
	{
		print ("Current State: Idle");
	}
	
	void RecentlyEngaged()
	{
		print ("Current State: Recently Engaged");
	}
	
	void Attack()
	{
		print ("Current State: Attack");
	}
	
	void Enraged()
	{
		print ("Current State: Enraged");
	}
	
	void changeStates(enemyStates es)
	{
		currentState = es;
	}
}
