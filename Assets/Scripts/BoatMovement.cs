// BoatController.cs - Attach this to your BOAT
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    public float speed = 1000f;
    public float turnSpeed = 800f;
    public Transform seatPosition; // An empty object showing where the player sits

    private Rigidbody rb;
    private bool isBeingControlled = false;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Physics logic goes in FixedUpdate
    void FixedUpdate()
    {
        // Only move if a player is controlling the boat
        if (isBeingControlled)
        {
            // Get input
            float forwardInput = Input.GetAxis("Vertical"); // W/S
            float turnInput = Input.GetAxis("Horizontal"); // A/D

            // Apply forces
            rb.AddRelativeForce(Vector3.forward * forwardInput * speed * Time.fixedDeltaTime);
            rb.AddTorque(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime);
        }
    }

    // This is a public method that the player script can call
    public void BoardBoat(GameObject incomingPlayer)
    {
        player = incomingPlayer;
        isBeingControlled = true;

        // Disable player's own controller if they have one (e.g. a PlayerController script)
        player.GetComponent<PlayerController>().enabled = false;

        // Parent the player and move them to the seat
        player.transform.SetParent(this.transform);
        player.transform.position = seatPosition.position;
        player.transform.rotation = seatPosition.rotation;
    }

    public void ExitBoat()
    {
        isBeingControlled = false;

        // Re-enable player's controls
        player.GetComponent<PlayerController>().enabled = true;

        // Un-parent the player
        player.transform.SetParent(null);
        // You might want to move the player to a safe exit spot here
        player = null;
    }
}