using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemyPrefab, enemyBossPrefab;
    public GameObject player;
    GameObject enemy, boss;
    float timeSpawned;
    float timePerSpawn = 8.0f;

    TerrainData terrain;
    float terrainWidth, terrainLength;

	// Use this for initialization
    void Awake()
    {
        timeSpawned = timePerSpawn;

        terrain = GameObject.Find("Ground").GetComponent<Terrain>().terrainData;
        terrainLength = terrain.size.z;
        terrainWidth = terrain.size.x;
    }

	void Start () {
        SpawnEnemyBoss();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (timeSpawned >= timePerSpawn)
        {
            DestroyEnemy();
            SpawnEnemy();
        }
        timeSpawned += Time.deltaTime;
	}

    GameObject SpawnEnemyBoss()
    {
        boss = Instantiate(enemyBossPrefab, new Vector3(terrainWidth / 2, 15, terrainLength / 2), Quaternion.Euler(new Vector3(0, -180, 0))) as GameObject;
        return boss;
    }

    void SpawnEnemy()
    {
        // Ensuring that target does not spawn within a distance of 10 units from player
        Vector3 spawnPosition = Vector3.zero;
        do
        {
            spawnPosition = new Vector3(Random.Range(5, terrainWidth), 1, Random.Range(5, terrainLength));
        } while (Vector3.Distance(spawnPosition, player.transform.position) < 10);

        // Ensuring that spawn target always faces the player
        Quaternion spawnRotation = Quaternion.LookRotation(player.transform.position - spawnPosition) * Quaternion.Euler(0, 90, 0);
        
        enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation) as GameObject;
        timeSpawned = 0;
    }

    void DestroyEnemy()
    {
        Destroy(enemy.gameObject);
    }
}
