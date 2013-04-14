using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour {
    public GameObject bulletPrefab;
    public AudioClip gunShot;
    public int damage = 2;
    public float fireRate = 4.0f;
    float timeTilNextShot;   

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        timeTilNextShot -= fireRate * Time.deltaTime;
	}

    public void Shoot()
    {
        if (timeTilNextShot <= 0)
        {
            AudioSource.PlayClipAtPoint(gunShot, transform.position);
            Instantiate(bulletPrefab, transform.FindChild("bulletSpawn").position, transform.rotation);
            timeTilNextShot = 1;
        }  
    }
}
