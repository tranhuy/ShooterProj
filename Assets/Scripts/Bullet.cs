using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public GameObject bulletHolePrefab;
    GameObject bulletHole;
    Gun gun;
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
            bulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * distanceFromSurface, Quaternion.LookRotation(hit.normal)) as GameObject;
            bulletHole.transform.parent = hit.transform;
        }
        Destroy(gameObject);
	}
}
