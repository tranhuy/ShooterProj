using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    [HideInInspector]
    public enum PlayerStates
    {
        IDLE_COMBAT,
        AIMING,
        FIRING,
        RELOADING, 
        WALKING,
        RUNNING,
    };

    // STATE DATA
    public PlayerStates currentState = PlayerStates.IDLE_COMBAT;
    public PlayerStates prevState;
    public int health;
    bool aim, fire, reload;

    // PLAYER MOVE DATA
    CharacterController controller;
    public float walkSpeed, runSpeed, strafeSpeed;
    Vector3 walkComponent, strafeComponent;
    bool isRunning;
    float gravity = -9.8f;
    Vector3 moveDirection = Vector3.zero;

    // GUN DATA
    Gun gun;
    public float timeTilNextShot, timeTilNextReload;

    // CAMERA MANIPULATION DATA
    CameraControl playerCamera;
    Quaternion aimToTargetOffset = Quaternion.Euler(0, 5, 0);
    public float rotateToTargetSpeed = 5.0f;

    // PLAYER ANIMATIONS
    AnimationState aim_anim, shoot_anim, reload_anim;
    AnimationState idle_anim, walk_anim, walkBack_anim, strafeRight_anim, strafeLeft_anim, run_anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
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

        idle_anim = animation["Combat_mode"];
        walk_anim = animation["Walking"];
        walkBack_anim = animation["Walking_back"];
        strafeRight_anim = animation["Moving_right"];       
        strafeLeft_anim = animation["Moving_left"];
        run_anim = animation["Run"];
    }
	
	// Update is called once per frame
    // Gun states
    void Update()
    {
        GetUserShootInput();
        if (aim)
        {
            RotateToTarget();
            playerCamera.AimDownSights();   // zooming camera in and out depending on whether right-mouse button is being held down
        }
        else
        {
            playerCamera.AimFromHip();
        }

        switch (currentState)
        {
            
            case PlayerStates.AIMING:
                if (!aim)
                {
                    ChangeState(PlayerStates.IDLE_COMBAT);
                }
                if (fire)
                {
                    ChangeState(PlayerStates.FIRING);
                }
                if (reload && gun.ammoCount < gun.ammoCapacity)
                {
                    ChangeState(PlayerStates.RELOADING);
                }
                Aim();
                break;
            case PlayerStates.FIRING:
                if (timeTilNextShot <= 0)
                {
                    Fire();
                }
                if (!fire)
                {
                    ChangeState(prevState);
                }
                break;
            case PlayerStates.RELOADING:
                if (timeTilNextReload <= 0)
                {
                    Reload();
                }
                if (!animation.IsPlaying(reload_anim.name))
                {
                    ChangeState(prevState);
                }
                break;
        }

        timeTilNextShot -= Time.deltaTime * gun.fireRate;
        timeTilNextReload -= Time.deltaTime * gun.reloadSpeed;
    }


    // Player Movement
	void FixedUpdate () 
    {
        moveDirection = walkComponent + strafeComponent;
        GetUserMoveInput();

        switch (currentState)
        {
            case PlayerStates.IDLE_COMBAT:
                if (moveDirection != Vector3.zero)
                {
                    ChangeState(PlayerStates.WALKING);
                }
                if (isRunning)
                {
                    ChangeState(PlayerStates.RUNNING);
                }
                if (reload && gun.ammoCount < gun.ammoCapacity)
                {
                    ChangeState(PlayerStates.RELOADING);
                }
                if (aim)
                {
                    ChangeState(PlayerStates.AIMING);
                }
                if (fire)
                {
                    ChangeState(PlayerStates.FIRING);
                }
                Idle();
                break;
            case PlayerStates.WALKING:
                if (moveDirection == Vector3.zero)
                {
                    ChangeState(prevState);
                }
                if (isRunning)
                {
                    ChangeState(PlayerStates.RUNNING);
                }
                Walk();
                break;
            case PlayerStates.RUNNING:
                if (!isRunning)
                {
                    if (moveDirection == Vector3.zero)
                    {
                        ChangeState(PlayerStates.IDLE_COMBAT);
                    }
                    else
                    {
                        ChangeState(prevState);
                    }
                }
                Run();
                break;
        }
        moveDirection.y = gravity;
        controller.Move(moveDirection * Time.deltaTime);        
    }

    void GetUserShootInput()
    {
        reload = Input.GetButtonDown("Reload");
        aim = Input.GetButton("Aim");
        fire = Input.GetButton("Fire");
    }

    void GetUserMoveInput()
    {
        if (controller.isGrounded)
        {
            walkComponent = transform.forward * Input.GetAxis("Vertical") * walkSpeed;
            strafeComponent = transform.right * Input.GetAxis("Horizontal") * strafeSpeed;
            isRunning = Input.GetButton("Run") && Input.GetAxis("Vertical") > 0;
        }      
    }

    void ChangeState(PlayerStates newState)
    {
        prevState = currentState;
        currentState = newState;
    }

    void Run()
    {
        moveDirection *= runSpeed;
        animation.CrossFade(run_anim.name);
    }

    void Walk()
    {
        if (transform.InverseTransformDirection(walkComponent).z > 0)
        {
            animation.CrossFade(walk_anim.name);
        }
        else if (transform.InverseTransformDirection(walkComponent).z < 0)
        {
            animation.CrossFade(walkBack_anim.name);
        }
        else if (transform.InverseTransformDirection(strafeComponent).x > 0)
        {
            animation.CrossFade(strafeRight_anim.name);
        }
        else
        {
            animation.CrossFade(strafeLeft_anim.name);
        }
    }

    void Idle()
    {
        animation.CrossFade(idle_anim.name);
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
