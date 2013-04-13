using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public GameObject bulletHoleTargetPrefab, bulletHoleGroundPrefab, dustCloudPrefab;
    GameObject bulletHole;
    Gun gun;
    float bulletLife = 1.0f;
    public float distanceFromSurface;

	// Use this for initialization
	void Start () {
        gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, gun.fireRange))
        {
            Vector3 spawnPos = hit.point + hit.normal * distanceFromSurface;
            Quaternion spawnRotation = Quaternion.LookRotation(hit.normal);

            if (hit.transform.CompareTag("Enemy"))
            {
                bulletHole = Instantiate(bulletHoleTargetPrefab, spawnPos, spawnRotation) as GameObject;
                bulletHole.transform.parent = hit.transform;
            }
            if (hit.transform.name.Equals("Ground"))
            {
                Instantiate(bulletHoleGroundPrefab, spawnPos, spawnRotation);
                Instantiate(dustCloudPrefab, spawnPos, spawnRotation);
            }
        }
        Destroy(gameObject, bulletLife);
	}
}
