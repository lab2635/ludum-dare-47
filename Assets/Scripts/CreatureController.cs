using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CreatureController : MonoBehaviour
{
    public float speed;
    public float jumpForce = 50;
    public Vector3 gravity = new Vector3(0, -10, 0);

    private Animator animator;
    private CharacterController controller;
    private bool grounded;
    private Vector3 velocity;
    private Vector3 facing;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        var input = Vector3.zero;
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        input.x = x;
        input.z = y;
        input = input.normalized;

        if (Input.GetKey(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = jumpForce;
        }

        var damping = speed * 3;

        velocity.x = Mathf.Lerp(velocity.x, input.x * speed, damping * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, input.z * speed, damping * Time.deltaTime);

        if (!controller.isGrounded)
        {
            velocity += gravity * Time.deltaTime;

            if (velocity.y < 0)
            {
                velocity.y -= (50 * Time.deltaTime);
            }
        }

        animator.SetBool("jump", !controller.isGrounded);
        animator.SetFloat("vx", input.x);
        animator.SetFloat("vy", input.z);

        controller.Move(velocity * Time.deltaTime);

        if (input.sqrMagnitude > 0)
        {
            facing = input;
        }

        var rotation = Quaternion.LookRotation(facing, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, rotation, 360f * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;
        
        if (body == null || body.isKinematic)
            return;
        
        var direction = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = direction * speed;
    }

    private void CheckGround()
    {
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), 1f))
        {
            grounded = true;
        }
    }

}
