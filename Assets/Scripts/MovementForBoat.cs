using UnityEngine;

public class MovementForBoat : MonoBehaviour
{
    private Rigidbody rb;
    private bool moveForward;

    private bool turnRight;

    private bool turnLeft;

    private bool moveBackwards;

    void Start()
    {
       rb = GetComponent<Rigidbody>();

       rb.isKinematic = false;
    }

    // Read inputs here to prevent FixedUpdate from dropping keystrokes
    void Update()
    {
        moveForward = Input.GetKey(KeyCode.W);
        
        moveBackwards = Input.GetKey(KeyCode.S);

        turnLeft = Input.GetKey(KeyCode.A);

        turnRight = Input.GetKey(KeyCode.D);
    }

    void FixedUpdate()
    {
        if (moveForward)
        {
            rb.AddForce(transform.forward * 45f, ForceMode.Force);
            
            // Proves if the Rigidbody is actually gaining speed
            Debug.Log("Current Physics Velocity: " + rb.linearVelocity);
        }

        if (turnRight)
        {
            transform.Rotate(Vector3.up * 50f * Time.deltaTime);
        }

        if (turnLeft)
        {
            transform.Rotate(-Vector3.up * 50f * Time.deltaTime);
        }

        if (moveBackwards)
        {
            rb.AddForce(-transform.forward * 45f, ForceMode.Force);
        }
    }
}