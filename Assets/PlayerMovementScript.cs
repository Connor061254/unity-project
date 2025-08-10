using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -15.32f;
    public float jumpHeight = 3f;

    public CharacterController controller;

    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float groundCheckRadius = 0.2f; 

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
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
}
