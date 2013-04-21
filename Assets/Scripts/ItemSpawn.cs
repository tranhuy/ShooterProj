using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour {
    public GameObject medkitPrefab;
    public Transform player;
    GameObject medkit;
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
        if (!medkit && playerVitals.GetHealth() < lowHealthThreshold)     
        {
            DropHealth();
        }   
	}

    void DropHealth()
    {
        // Dropping healthpack from above and in front of player
        Vector3 dropLocation = new Vector3(player.transform.position.x, dropHeight, player.transform.position.z) + (dropDistFromPlayer * player.transform.forward);
        medkit = (GameObject)Instantiate(medkitPrefab, dropLocation, Quaternion.identity);
    }
}
