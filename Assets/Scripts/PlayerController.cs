using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float walkSpeed = 9f;

    public float standingHeight;

    public float sprintSpeed = 18f;

    public float currentSpeed = 0f;
    public float gravity = -15.32f;
    public float jumpHeight = 3f;

    public CharacterController controller;

    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [SerializeField] private Camera playerCamera;

    [SerializeField] private AudioListener playerListener;
    private Animator animator;

    Vector3 velocity;
    bool isGrounded;

    public float groundCheckRadius = 0.2f; 

    void Start()
    {
        currentSpeed = walkSpeed;
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                currentSpeed = walkSpeed;
            }

            else
            {
                currentSpeed = sprintSpeed;
            }
            
            
        }

        else
        {
            currentSpeed = walkSpeed;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0f, z).normalized;

        bool isMoving = movement.magnitude > 0.1f;
        
             Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * currentSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

        if (isMoving && isGrounded)
        {
            animator.SetBool("isWalking", true);

            float animationMultiplier = currentSpeed / walkSpeed;
            animator.SetFloat("animSpeed", animationMultiplier);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
    
    // This function draws a helpful visual sphere in the Scene view
    // so you can see your groundCheck area.
    void OnDrawGizmos()
    {
        if (groundcheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundcheck.position, groundCheckRadius);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            if(playerCamera != null)
            {
                playerCamera.enabled = false;
            }

            if (playerListener != null)
            {
                playerListener.enabled = false;
            }
        }
    }
    
}
