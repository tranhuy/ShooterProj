using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour {
    public GameObject healthPackPrefab;
    public Transform player;
    GameObject healthPack;
    PlayerStats playerVitals;

    float lowHealthThreshold;
    float dropHeight = 15.0f;
    float dropDistFromPlayer = 2.0f;

	// Use this for initialization
	void Start () 
    {
        playerVitals = player.GetComponent<PlayerStats>();
        lowHealthThreshold = 0.50f * playerVitals.maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
        // drop healthpack when player's health dips below lowHealthThreshold and when there are unused healthpacks in the scene
        if (!healthPack && playerVitals.GetHealth() < lowHealthThreshold)     
        {
            DropHealth();
        }   
	}

    void DropHealth()
    {
        // Dropping healthpack from above and in front of player
        Vector3 dropLocation = new Vector3(player.transform.position.x, dropHeight, player.transform.position.z) + (dropDistFromPlayer * player.transform.forward);
        healthPack = (GameObject)Instantiate(healthPackPrefab, dropLocation, Quaternion.identity);
    }
}
