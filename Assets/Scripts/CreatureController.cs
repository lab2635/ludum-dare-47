using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(CharacterController))]
public class CreatureController : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce = 5;
    public Vector3 gravity = new Vector3(0, -10, 0);
    public Vector3 cameraOffset = new Vector3(0, 10, 0);
    public float rotationSpeed = 270f;
    public float dashCooldown = 1f;
    public float dashAcceleration = 200f;
    public Transform handTransform = null;
    public GameObject attachment = null;

    public AudioClip RunningLoopSFX;
    public AudioClip DashSFX;

    private AudioSource audioSource;
    private AudioSource runningAudioSource;
    private Gun gun;
    private Animator animator;
    private CharacterController controller;
    private bool grounded;
    private Vector3 velocity;
    private Vector3 facing;
    private float dashAccumulator;
    private Vector3 moveDirection = Vector3.zero;


    private void Start()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        runningAudioSource = this.gameObject.AddComponent<AudioSource>();        
        runningAudioSource.playOnAwake = false;
        runningAudioSource.loop = true;
        runningAudioSource.clip = this.RunningLoopSFX;

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        gun = GetComponentInChildren<Gun>();
        gun.gunEnabled = false;
        dashAccumulator = dashCooldown;

        if (GameManager.Instance.CheckpointList[(int)Checkpoints.GunRoomComplete])
        {
            AcquireGun();
        }
    }

    public void AcquireGun()
    {
	    if (gun == null) 
			gun = GetComponentInChildren<Gun>();
	    
	    gun.gunEnabled = true;
    }
    
    public void RemoveAttachment()
    {
	    attachment = null;
    }


    private bool CanDash() => dashAccumulator >= dashCooldown;

    void Dash()
    {
        if (CanDash())
        {
            this.audioSource.clip = this.DashSFX;
            this.audioSource.Play();

            velocity += transform.forward * dashAcceleration;
            dashAccumulator = 0;
        }
    }
    
    void Update()
    {
		if (GameManager.Instance.IsPlayerControllerEnabled)
        {
	        dashAccumulator += Time.deltaTime;
        
	        var input = Vector3.zero;
	        var x = Input.GetAxisRaw("Horizontal");
	        var y = Input.GetAxisRaw("Vertical");

	        input.x = x;
	        input.z = y;
	        input = input.normalized;
        
	        var damping = speed * 3;

	        velocity.x = Mathf.Lerp(velocity.x, input.x * speed, damping * Time.deltaTime);
	        velocity.z = Mathf.Lerp(velocity.z, input.z * speed, damping * Time.deltaTime);

	        if (Input.GetButtonDown("Fire1") && gun != null)
	        {
	            gun.Trigger();
	        }

	        if (Input.GetButtonDown("Jump"))
	        {
	            Dash();
	        }
        
	        if (!controller.isGrounded)
	        {
	            velocity += gravity * Time.deltaTime;
        
	            // if (velocity.y < 0)
	            // {
	            //     velocity.y -= (50 * Time.deltaTime);
	            // }
	        }

	        // animator.SetBool("jump", !controller.isGrounded);
	        // animator.SetFloat("vx", input.x);
	        // animator.SetFloat("vy", input.z);

	        controller.Move(velocity * Time.deltaTime);
        
	        var pos = transform.position;
	        var mousePos = Input.mousePosition;
	        var playerPos = Camera.main.WorldToScreenPoint(pos);
        
	        mousePos.x = mousePos.x - playerPos.x;
	        mousePos.y = mousePos.y - playerPos.y;
        
	        var angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
	        var rotation = Quaternion.Euler(Vector3.up * angle);

	        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
	            rotationSpeed * Mathf.Deg2Rad * Time.deltaTime);

	        Camera.main.transform.position = pos + cameraOffset;

            this.Animate(x, y);
		}
    }

    private void Animate(float x, float y)
    {
        moveDirection = new Vector3(x, 0, y);

        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection = moveDirection.normalized;
        }

        if(moveDirection != Vector3.zero && !this.runningAudioSource.isPlaying && this.grounded)
        {
            this.runningAudioSource.Play();
        }
        else if(moveDirection == Vector3.zero || !this.grounded)
        {
            this.runningAudioSource.Stop();
        }

        moveDirection = transform.InverseTransformDirection(moveDirection);

        animator.SetFloat("X", moveDirection.x);//, 0.05f, Time.deltaTime);
        animator.SetFloat("Y", moveDirection.z);//, 0.05f, Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;
        
        if (body == null || body.isKinematic)
            return;
        
        var direction = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DisableTimerZone")
        {
            GameManager.Instance.NeverLose = true;
        }

        if (other.gameObject.name == "WinTrigger")
        {
            GameManager.Instance.WinGame();
        }
    }

    private void CheckGround()
    {
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), 1f))
        {
            grounded = true;
        }
    }

}
