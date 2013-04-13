using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    public int maxHealth = 100;
    int currentHealth;
    float healthBarLength;

	// Use this for initialization
	void Start () 
    {
        currentHealth = maxHealth;
        healthBarLength = 250;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
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
