using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerController : MonoBehaviour
{
   public float walkSpeed = 5f; 
   public float runSpeed = 10f;

   private float currentSpeed;

   public float gravity = -9.81f;

   public float jumpHeight = 2f;



   private CharacterController controller;

   private Vector2 moveInput;
   private Vector3 playerVelocity;

   private bool isGrounded;
   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        controller.Move(transform.TransformDirection(move) * currentSpeed * Time.deltaTime);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}

