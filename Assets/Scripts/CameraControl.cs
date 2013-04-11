using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public Transform player;
    Transform playerCamera;

    // Mouse Orbit
    public float horizontalLookSensitivity = 1.0f;
    public float verticalLookSensitivity = 1.0f;
    float currentXRotation, currentYRotation;
    float xRotateAmount, yRotateAmount;
    float rotateVelocityX, rotateVelocityY;
    float damping = 0.2f;

    // Aiming
    public float xAimOffsetToPlayer = 0.6f;
    public float yAimOffsetToPlayer = 1.6f;
    public float zAimOffsetToPlayer = -0.7f;
    Vector3 playerAimOffset;
    float aimXVel, aimYVel, aimZVel;
    float hipToAimTime = 0.2f;
    bool isAiming = false;

	// Use this for initialization
	void Start () {
        playerCamera = player.FindChild("PlayerCamera");
	}
	
	// Update is called once per frame
	void Update () {
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue);

        MouseOrbit();
        if (!isAiming)
        {
            transform.position = playerCamera.position;
        }
	}

    void MouseOrbit()
    {
        xRotateAmount += Input.GetAxis("Mouse X") * horizontalLookSensitivity;
        yRotateAmount -= Input.GetAxis("Mouse Y") * verticalLookSensitivity;

        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotateAmount, ref rotateVelocityX, damping);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotateAmount, ref rotateVelocityY, damping);
        currentYRotation = Mathf.Clamp(currentYRotation, -45, 10);
        transform.rotation = Quaternion.Euler(currentYRotation, currentXRotation, 0);
    }

    public void AimDownSights()
    {
        isAiming = true;

        // maintaining same relative camera position to player during aiming
        playerAimOffset = player.position + (player.right * xAimOffsetToPlayer + player.up * yAimOffsetToPlayer + player.forward * zAimOffsetToPlayer);  
        float aimX = Mathf.SmoothDamp(transform.position.x, playerAimOffset.x, ref aimXVel, hipToAimTime);
        float aimY = Mathf.SmoothDamp(transform.position.y, playerAimOffset.y, ref aimYVel, hipToAimTime);
        float aimZ = Mathf.SmoothDamp(transform.position.z, playerAimOffset.z, ref aimZVel, hipToAimTime);
        transform.position = new Vector3(aimX, aimY, aimZ);
    }

    public void AimFromHip()
    {
        float hipX = Mathf.SmoothDamp(transform.position.x, playerCamera.position.x, ref aimXVel, hipToAimTime);
        float hipY = Mathf.SmoothDamp(transform.position.y, playerCamera.position.y, ref aimYVel, hipToAimTime);
        float hipZ = Mathf.SmoothDamp(transform.position.z, playerCamera.position.z, ref aimZVel, hipToAimTime);
        transform.position = new Vector3(hipX, hipY, hipZ);
        isAiming = (transform.position == playerCamera.position) ? false : true;
    }
}
