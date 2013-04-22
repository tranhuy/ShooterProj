using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour {
    public GameObject medkitPrefab;
    public Transform player;
    GameObject medkit;
    PlayerStats playerVitals;

    float lowHealthThreshold;
    float dropHeight = 15.0f;
    float distFromPlayer = 2.0f;

	// Use this for initialization
	void Start () 
    {
        playerVitals = player.GetComponent<PlayerStats>();
        lowHealthThreshold = 0.50f * playerVitals.maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Ensures that there is only one medkit in the scene at any given time
        if (!medkit)
        {
            if (playerVitals.GetHealth() < lowHealthThreshold)    // drop medkit when player's health dips below lowHealthThreshold
            {
                DropMedKit();
            }
        }
        else
        {
            if (medkit.transform.position.y < -5)
            {
                Destroy(medkit.gameObject);                      // destroy medkit if dropped outside bounds of terrain
            }
        }
	}

    void DropMedKit()
    {
        // Dropping medkit from above and in front of player
        Vector3 dropLocation = new Vector3(player.transform.position.x, dropHeight, player.transform.position.z) + (distFromPlayer * player.transform.forward);
        medkit = (GameObject)Instantiate(medkitPrefab, dropLocation, Quaternion.identity);
    }
}
