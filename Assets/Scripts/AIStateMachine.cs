using UnityEngine;
using System.Collections;

public class AIStateMachine : MonoBehaviour {
	public enum EnemyState 
    {
        EVADE,
		ATTACK
	}

    Terrain terrain;
	GameObject player;

    public EnemyState currentState;
    EnemyAI enemy;
    float healthLastFrame;

    Vector3 rndPosition;
    bool isEvading = false;         // used to ensure that a random position is generated only once per evasion attempt
	
	void Start () 
    { 
		player = GameObject.FindGameObjectWithTag("Player");
        terrain = GameObject.Find("Ground").GetComponent<Terrain>();

        enemy = GetComponent<EnemyAI>();
        currentState = EnemyState.ATTACK;
        healthLastFrame = enemy.GetHealth();
	}
	 
	void Update () 
    {
	}
	
    void UpdateState()
    {
        // Enemy State Machine
        switch (currentState)
        {
            case EnemyState.ATTACK:
                Attack();
                if (enemy.GetHealth() != healthLastFrame)            // enemy taking damage
                {
                    changeStates(EnemyState.EVADE);
                }              
                break;
            case EnemyState.EVADE:
                Evade();
                // Switch to attacking player when enemy reaches new position
                if ((int)Vector3.Distance(transform.position, rndPosition) <= 0)
                {
                    changeStates(EnemyState.ATTACK);
                }
                break;
        } 
    }

    void changeStates(EnemyState state)
    {
        currentState = state;
    }

    void Evade()
    {
        if (!isEvading)
        {
            generateRandomPosition();
        }
        enemy.MoveTo(rndPosition);
        healthLastFrame = enemy.GetHealth();
        //print("Current State: Evade");
    }

	void Attack()
	{
        isEvading = false;
        enemy.ShootAttack();
		//print ("Current State: Attack");
	}
	 
    /* This method generates a random Vector3 that meets the following 3 criteria:
    *  1) within enemy firing range
    *  2) within terrain dimensions
    *  3) at a minimum distance away from the player
    */
	void generateRandomPosition()
    {  
		float furthestXValue = terrain.terrainData.size.x;
		float furthestZValue = terrain.terrainData.size.z; 
		System.Random rnd = new System.Random();
		int xVal, zVal;

		do {
            xVal = rnd.Next((int)player.transform.position.x - 20, (int)player.transform.position.x + 20);
            zVal = rnd.Next((int)player.transform.position.z - 20, (int)player.transform.position.z + 20); 
		} while((xVal > furthestXValue || xVal < 0) || ( zVal > furthestZValue || zVal < 0));
        rndPosition = new Vector3(xVal, transform.position.y, zVal);

        isEvading = true;
	}
} 
