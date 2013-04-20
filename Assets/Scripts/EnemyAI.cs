using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    // ENEMY RESISTANCE
    public enum Armor
    {
        WEAK = 10,
        MEDIUM = 20,
        STRONG = 30,
        ELITE = 40,
        BOSS = 50
    }
     public Armor resistance = Armor.MEDIUM;

    // ENEMY HEALTH
    Transform healthBar;
    GUITexture healthBarTexture;
    int healthBarLength = 100;
    public float maxHealth = 100;
    float currentHealth;

    bool isActive = false;

    // ENEMY OBJECTS
    EnemyGun gun;                                   // WEAPON
    public GameObject bloodPrefab;                  // BLOOD 
    GameObject blood;
    public float maxBloodSize = 1.2f;
    public float bloodSpreadTime = 5.0f;
    float bloodSpreadVel;
    Vector3 currentBloodSize;   

    // ENEMY TARGET
    Transform target;
    Transform targetGun;
    Quaternion targetOffset = Quaternion.Euler(0, -3.6f, 0);    // difference in rotation between enemy boss and his gun
    PlayerStats playerStatus;
    float rotateToPlayerSpeed = 15.0f;  

    // Animations
    public AnimationClip land_anim, idle_anim, walk_anim, run_anim, shoot__anim, spinLeft_anim, spinRight_anim, death_anim;

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
        if (blood && currentBloodSize.x < maxBloodSize)
        {
            SpreadBlood();
        }
	}

    void InitializeAnimations()
    {
        animation.AddClip(land_anim, "land");
        animation.AddClip(idle_anim, "idle");
        animation.AddClip(walk_anim, "walk");
        animation.AddClip(run_anim, "run");
        animation.AddClip(death_anim, "die");
        animation.AddClip(shoot__anim, "shoot");
        animation.AddClip(spinLeft_anim, "spinLeft");
        animation.AddClip(spinRight_anim, "spinRight");
    }

    void OnCollisionEnter(Collision collideObj)
    {
        if (collideObj.transform.name.Equals("Ground"))
        {
            animation.CrossFade("land");
            isActive = true;
        }
    }

    void UpdateHealthBar()
    {
        float xOffset = -healthBarTexture.pixelInset.width / 2;
        float barWidth = healthBarLength * (currentHealth / maxHealth);
        float barHeight = 10;
        healthBarTexture.pixelInset = new Rect(xOffset, 0, barWidth, barHeight);
        healthBar.position = Camera.main.WorldToViewportPoint(transform.position + new Vector3(0, 2 * transform.localScale.y, 0));    // position healthbar over enemy
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    void SpreadBlood()
    {
        float deltaSize = Mathf.SmoothDamp(currentBloodSize.x, maxBloodSize, ref bloodSpreadVel, bloodSpreadTime);
        currentBloodSize.x = deltaSize;
        currentBloodSize.z = deltaSize;
        blood.transform.localScale = currentBloodSize;
    }

    void ShootAttack()
    {
        if (TurnTowardsTarget())
        {
            animation.CrossFade("shoot");
            gun.Shoot();
        }
    }

    // Method returns true if enemy is facing player and false otherwise
    bool TurnTowardsTarget()
    {
        // Finding out if player target is on right or left side 
        Vector3 relativePosition = targetGun.position - gun.transform.position;
        Vector3 cross = Vector3.Cross(gun.transform.forward, relativePosition.normalized);
        int angleBetween = (int)(Mathf.Asin(cross.y) * Mathf.Rad2Deg);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePosition) * targetOffset, rotateToPlayerSpeed * Time.deltaTime);
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
            return true;            // enemy is facing player
        }
        
        return false;
    } 

    void ApplyDamage(string bodyPart)
    {
        if (currentHealth > 0)
        {
            switch (bodyPart)
            {
                case "head":
                    currentHealth -= currentHealth;
                    break;
                case "armorBody":
                    currentHealth -= gun.damage / (int)resistance;
                    break;
            }
            if (currentHealth <= 0)
            {
                StartCoroutine("Death");
            }
        }
    }

    IEnumerator Death()
    {
        isActive = false;
        UpdateHealthBar();
        Destroy(rigidbody);
        Destroy(collider);           
        animation.CrossFade("die");
        yield return new WaitForSeconds(death_anim.length);
        blood = Instantiate(bloodPrefab, new Vector3(transform.position.x, 0, transform.position.z) - transform.forward, Quaternion.identity) as GameObject;
        currentBloodSize = blood.transform.localScale;
    }
}
