using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public GameObject bulletHoleTargetPrefab, bulletHoleGroundPrefab, bloodSplatterPrefab, bulletHoleEnemyPrefab, dustCloudPrefab;
    GameObject bulletHole;
    Gun gun;
    float bulletLife = 0.5f;
    public float distanceFromSurface;

	// Use this for initialization
	void Start () {
        gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (!bulletHole && Physics.Raycast(transform.position, transform.forward, out hit, gun.fireRange))
        {
            Vector3 spawnPos = hit.point + hit.normal * distanceFromSurface;
            Quaternion spawnRotation = Quaternion.LookRotation(hit.normal);

            if (hit.transform.name.Equals("Ground"))
            {
                bulletHole = (GameObject)Instantiate(bulletHoleGroundPrefab, spawnPos, spawnRotation);
                Instantiate(dustCloudPrefab, spawnPos, spawnRotation);
            }
            if (hit.transform.CompareTag("Target"))
            {
                bulletHole = (GameObject)Instantiate(bulletHoleTargetPrefab, spawnPos, spawnRotation);
                bulletHole.transform.parent = hit.transform;
            }            
            if (hit.transform.CompareTag("Enemy"))           // HEADSHOT = instant kill
            {
                print("Bullet hit: " + hit.collider.name);
                if (hit.collider.name.Equals("head"))
                {
                    bulletHole = (GameObject)Instantiate(bloodSplatterPrefab, spawnPos, spawnRotation);
                }
                else
                {
                    bulletHole = (GameObject)Instantiate(bulletHoleEnemyPrefab, spawnPos, spawnRotation);
                }
                hit.transform.SendMessage("ApplyDamage", hit.collider.name, SendMessageOptions.DontRequireReceiver);
            }
        }
        Destroy(gameObject, bulletLife);
	}
}
