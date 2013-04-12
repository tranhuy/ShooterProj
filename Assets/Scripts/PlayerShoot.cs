using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {
    
    // GUN DATA
    Gun gun;
    public float timeTilNextShot, timeTilNextReload;
    bool aim, fire, reload;

    // CAMERA MANIPULATION DATA
    CameraControl playerCamera;
    Quaternion aimToTargetOffset = Quaternion.Euler(0, 5, 0);
    public float rotateToTargetSpeed = 5.0f;

    // PLAYER ANIMATIONS
    AnimationState aim_anim, shoot_anim, reload_anim;

	// Use this for initialization
	void Start () 
    {
        gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
        playerCamera = Camera.main.GetComponent<CameraControl>();

        aim_anim = animation["FireReady_standing"];
        aim_anim.wrapMode = WrapMode.ClampForever;

        shoot_anim = animation["Fire_standing"];
        shoot_anim.speed = gun.fireRate / 2;

        reload_anim = animation["Reload_standing"];
        reload_anim.layer = 1;
        reload_anim.wrapMode = WrapMode.Once;
        reload_anim.speed = reload_anim.length * gun.reloadSpeed;
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetUserShootInput();
        if (aim)
        {
            RotateToTarget();
            playerCamera.AimDownSights();   // zooming camera in and out depending on whether right-mouse button is being held down
            Aim();
        }
        else
        {
            playerCamera.AimFromHip();
        }

        if (fire && timeTilNextShot <= 0 && !animation.IsPlaying(reload_anim.name))
        {
            Fire();
        }

        if (reload && gun.ammoCount < gun.ammoCapacity && timeTilNextReload <= 0)
        {
            Reload();
        }

        timeTilNextShot -= Time.deltaTime * gun.fireRate;
        timeTilNextReload -= Time.deltaTime * gun.reloadSpeed;	
	}

    void GetUserShootInput()
    {
        reload = Input.GetButtonDown("Reload");
        aim = Input.GetButton("Aim");
        fire = Input.GetButton("Fire");
    }

    void Reload()
    {
        gun.Reload();
        animation.Play(reload_anim.name);
        animation.CrossFade(aim_anim.name);
        timeTilNextReload = 1.05f;
    }

    void Aim()
    {
        animation.CrossFade(aim_anim.name);
    }

    void Fire()
    {
        gun.Shoot();
        if (gun.ammoCount > 0)
        {
            animation.Play(shoot_anim.name);
        }
        else
        {
            animation.Stop(shoot_anim.name);
        }
        timeTilNextShot = 1;
    }

    void RotateToTarget()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 target = gun.fireRange * ray.direction + ray.origin;
        Vector3 source = gun.transform.FindChild("Muzzle").position;
        Quaternion lookDirection = Quaternion.LookRotation(target - source, Vector3.up) * aimToTargetOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * rotateToTargetSpeed);
    }
}
