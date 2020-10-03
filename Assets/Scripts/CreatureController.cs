using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CreatureController : MonoBehaviour
{
    [SerializeField] public float speed = 10;
    [SerializeField]
    public float jumpForce = 5;
    [SerializeField]
    public Vector3 gravity = new Vector3(0, -10, 0);

    [SerializeField] public Vector3 cameraOffset = new Vector3(0, 10, 0);
    [SerializeField]
    public float rotationSpeed = 270f;
    
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
