using UnityEngine;
using System.Collections;

public class BulletCase : MonoBehaviour {

    float life = 0.5f;
    float destroyTime;
    [HideInInspector]
    public Vector3 velocity;
    float gravity = 9.8f;
    float turnSpeed, turnAngle;

	// Use this for initialization
	void Start () {
        destroyTime = Time.time + life;
        turnAngle = Random.value * 360;
        turnSpeed = Random.Range(-360.0f, 360.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > destroyTime)
        {
            Destroy(gameObject);
        }

        transform.LookAt(Camera.main.transform.position);
        turnAngle += turnSpeed * Time.deltaTime;
        transform.Rotate(0, 0, turnAngle);
        velocity.y -= gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * life * 2.0f);
	}
}
