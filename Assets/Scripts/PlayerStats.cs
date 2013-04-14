using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    public float maxHealth = 100;
    float currentHealth;
    float healthBarLength;

    AnimationState death_anim;

	// Use this for initialization
	void Start () 
    {
        death_anim = animation["Die_knocked_backward"];
        death_anim.wrapMode = WrapMode.ClampForever;
        death_anim.speed /= 2;
        death_anim.layer = 1;

        currentHealth = maxHealth;
        healthBarLength = 250;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public float GetHealth()
    {
        return currentHealth;
    }

    void ApplyDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    void Death()
    {
        // Disable player scripts
        transform.GetComponent<PlayerMove>().enabled = false;
        transform.GetComponent<PlayerShoot>().enabled = false;
        animation.CrossFade(death_anim.name);
    }

    // Player heath bar
    public GUIStyle healthBarStyle;
    int rightMargin = 10;
    void OnGUI() 
    {
        GUI.Box(new Rect(Screen.width - healthBarLength - rightMargin, 30, healthBarLength, 20), "");
        GUI.Box(new Rect(Screen.width - healthBarLength - rightMargin, 30, healthBarLength * (currentHealth / maxHealth), 20), "", healthBarStyle);     
    }
}
