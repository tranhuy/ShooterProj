using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    // ENEMY RESISTANCE
    public enum Armor
    {
        WEAK = 5,
        MEDIUM = 10,
        STRONG = 15,
        ELITE = 25,
        BOSS = 30
    }
    public Armor resistance = Armor.MEDIUM;

    // DAMAGE MODIFIERS
    float bodyDamageModifier = 1.0f;
    float legDamageModifier = 0.8f;
    float armDamageModifier = 0.6f;

    // ENEMY STATE INFO
    bool isActive = false;

    // ENEMY HEALTH
    Transform healthBar;
    GUITexture healthBarTexture;
    int healthBarLength = 100;
    public float maxHealth = 100;
    float currentHealth;

    // ENEMY MOVEMENT
    public float runSpeed = 6.0f;

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
    Gun targetGun;
    Quaternion targetOffset;       // will store the difference in rotation between enemy boss and his gun
    PlayerStats playerStatus;
    float rotateToPlayerSpeed = 70.0f;  

    // Animations
    public AnimationClip land_anim, idle_anim, walk_anim, run_anim, shoot__anim, spinLeft_anim, spinRight_anim, death_anim;

	// Use this for initialization
	void Start () 
    {
        InitializeAnimations();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerStatus = target.GetComponent<PlayerStats>();
        targetGun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();

        gun = GameObject.FindGameObjectWithTag("LaserGun").GetComponent<EnemyGun>();
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
                // Updating enemy state
                transform.SendMessage("UpdateState", SendMessageOptions.DontRequireReceiver);
                //if ((int)Vector3.Distance(transform.position, GameObject.Find("MoveToTarget").transform.position) > 0)
                //{
                //    MoveTo(GameObject.Find("MoveToTarget").transform.position);
                //}
                //else
                //{
                //    ShootAttack();
                //}
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
        currentHealth = (currentHealth < 0) ? 0 : currentHealth;
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

    // Method returns true if enemy is facing target and false otherwise
    bool TurnTowardsTarget(Vector3 target, Transform start)
    {
        // Finding out if player target is on right or left side 
        Vector3 relativePosition = target - start.position;
        Vector3 cross = Vector3.Cross(start.forward, relativePosition.normalized);
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
            return true;            // enemy is facing player
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePosition) * targetOffset, rotateToPlayerSpeed * Time.deltaTime);
        return false;
    }

    public void MoveTo(Vector3 location)
    {
        targetOffset = Quaternion.identity;
        if (TurnTowardsTarget(location, transform))
        {
            location.y = transform.position.y;
            animation.Play("run");
            transform.position = Vector3.MoveTowards(transform.position, location, runSpeed * Time.deltaTime);
        }
    }

    public void ShootAttack()
    {
        targetOffset = Quaternion.Euler(0, -3.6f, 0);               // difference in rotation between enemy boss and his gun
        if (TurnTowardsTarget(targetGun.transform.position, gun.transform))
        {
            animation.CrossFade("shoot");
            gun.Shoot();
        }
    }

    void ApplyDamage(string bodyPart)
    {
        if (currentHealth > 0)
        {
            // applying different amounts of damage based on body part hit
            switch (bodyPart)
            {
                case "head":
                    currentHealth -= currentHealth;
                    break;
                case "armorBody":
                    currentHealth -= bodyDamageModifier * (targetGun.damage / (int)resistance);
                    break;
                case "Bip01 L Thigh":
                case "Bip01 R Thigh":
                    currentHealth -= legDamageModifier * (targetGun.damage / (int)resistance);
                    break;
                case "Bip01 L UpperArm":
                case "Bip01 R UpperArm":
                case "Bip01 L Forearm":
                case "Bip01 R Forearm":
                    currentHealth -= armDamageModifier * (targetGun.damage / (int)resistance);
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
