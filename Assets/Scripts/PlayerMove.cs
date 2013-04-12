using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    // PLAYER MOVE DATA
    CharacterController controller;
    public float walkSpeed, runSpeed, strafeSpeed;
    Vector3 walkComponent, strafeComponent;
    bool isRunning;
    float gravity = -9.8f;
    Vector3 moveDirection = Vector3.zero;

    // PLAYER ANIMATIONS
    AnimationState idle_anim, walk_anim, walkBack_anim, strafeRight_anim, strafeLeft_anim, run_anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Animation Setup
        idle_anim = animation["Combat_mode"];
        walk_anim = animation["Walking"];
        walkBack_anim = animation["Walking_back"];
        strafeRight_anim = animation["Moving_right"];       
        strafeLeft_anim = animation["Moving_left"];
        run_anim = animation["Run"];
    }
	
	// Update is called once per frame
    void Update()
    {
    }

	void FixedUpdate () 
    {
        moveDirection = walkComponent + strafeComponent;
        GetUserMoveInput();

        if (moveDirection != Vector3.zero)
        {
            if (isRunning)
            {
                Run();
            }
            else
            {
                Walk();
            }
        }
        else 
        {
            Idle();
        }

        moveDirection.y = gravity;
        controller.Move(moveDirection * Time.deltaTime);        
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
        else if (transform.InverseTransformDirection(strafeComponent).x < 0)
        {
            animation.CrossFade(strafeLeft_anim.name);
        }
    }

    void Idle()
    {
        animation.CrossFade(idle_anim.name);
    }
}
