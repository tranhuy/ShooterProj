using UnityEngine;
using System.Collections;

public class BulletTrace : MonoBehaviour {
    EnemyGun gun;

    float bulletLife = 0.5f;
    float bulletSpeed = 50.0f;
    Vector3 velocity;

    float gravity = 9.8f;

	// Use this for initialization
	void Start () 
    {
        gun = GameObject.FindGameObjectWithTag("LaserGun").GetComponent<EnemyGun>();
        velocity += transform.forward * bulletSpeed;
	}
	
	// Update is called once per frame
	void Update () 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletSpeed * Time.deltaTime))
        {
            hit.transform.SendMessage("ApplyDamage", gun.damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
        velocity.y -= gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime, Space.World);
        Destroy(gameObject, bulletLife);
	}
}
