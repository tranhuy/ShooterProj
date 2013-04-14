using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    // ENEMY STATES
    enum EnemyStates
    {
        PURSUE,
        ATTACK,
        RAGE
    }
    bool isActive = false;

    // ENEMY OBJECTS
    EnemyGun gun;

    // ENEMY TARGET
    Transform target;
    Transform targetGun;
    Quaternion targetOffset = Quaternion.Euler(0, -3, 0);
    PlayerStats playerStatus;
    float rotateToPlayerSpeed = 15.0f;

    // ENEMY HEALTH
    Transform healthBar;
    GUITexture healthBarTexture;
    int healthBarLength = 100;
    public float maxHealth = 100;
    float currentHealth;

    // Animations
    public AnimationClip land_anim, idle_anim, walk_anim, run_anim, shoot__anim, spinLeft_anim, spinRight_anim;

	// Use this for initialization
	void Start () 
    {
        InitializeAnimations();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetGun = GameObject.FindGameObjectWithTag("Gun").transform;
        gun = GameObject.FindGameObjectWithTag("LaserGun").GetComponent<EnemyGun>();
        playerStatus = target.GetComponent<PlayerStats>();    
        healthBar = transform.FindChild("EnemyHealth");
        healthBarTexture = healthBar.guiTexture;
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive && !animation.IsPlaying("land"))
        {      
            animation.CrossFade("idle");
            if (playerStatus.GetHealth() > 0)
            {
                ShootAttack();                      // ATTACK STATE    
            }
            UpdateHealthBar();   
        }
	}

    void InitializeAnimations()
    {
        animation.AddClip(land_anim, "land");
        animation.AddClip(idle_anim, "idle");
        animation.AddClip(walk_anim, "walk");
        animation.AddClip(run_anim, "run");
        animation.AddClip(shoot__anim, "shoot");
        animation.AddClip(spinLeft_anim, "spinLeft");
        animation.AddClip(spinRight_anim, "spinRight");
    }

    void OnCollisionEnter(Collision collideObj)
    {
        animation.CrossFade("land");
        isActive = true;
    }

    void UpdateHealthBar()
    {
        float xOffset = -healthBarTexture.pixelInset.width / 2;
        float barWidth = healthBarLength * (currentHealth / maxHealth);
        float barHeight = 10;
        healthBarTexture.pixelInset = new Rect(xOffset, 0, barWidth, barHeight);
        healthBar.position = Camera.main.WorldToViewportPoint(transform.position + new Vector3(0, 2 * transform.localScale.y, 0));    // position healthbar over enemy
    }

    // Method returns true if enemy is facing player and false otherwise
    bool TurnTowardsTarget()
    {
        // Finding out if player target is on right or left side 
        Vector3 relativePosition = targetGun.position - gun.transform.position;
        Vector3 cross = Vector3.Cross(gun.transform.forward, relativePosition.normalized);
        int angleBetween = (int)(Mathf.Asin(cross.y) * Mathf.Rad2Deg);

        if (angleBetween > 0)
        {
            animation.CrossFade("spinRight");
        }
        else if (angleBetween < 0)
        {
            animation.CrossFade("spinLeft");
        }
        else
        {
            return true;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePosition) * targetOffset, rotateToPlayerSpeed * Time.deltaTime);
        return false;
    }

    void ShootAttack()
    {
        if (TurnTowardsTarget())
        {
            animation.CrossFade("shoot");
            gun.Shoot();   
        }      
    }

    void ApplyDamage(int damage)
    {
        currentHealth -= damage;
    }
}
