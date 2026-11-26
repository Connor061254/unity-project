using Unity.Netcode;
using UnityEngine;

public class CapybaraMovement : NetworkBehaviour
{
    public GameObject player;
    NetworkVariable<bool> isRidingCapybara = new NetworkVariable<bool>(false);

    private bool movingForward;
    private bool movingBackward;
    private bool turningLeft;
    private bool turningRight;

    private Rigidbody rb;
    public float forwardSpeed;
    public float backwardSpeed;
    public float leftTurnSpeed;
    public float rightTurnSpeed;

    public Transform seatPosition;
    private bool hasMovement;
    public float movingDrag;

    public float stoppingDrag;


    void Update()
    {
        if(!IsOwner && !isRidingCapybara.Value) return;

        movingForward = Input.GetKey(KeyCode.W);
        movingBackward = Input.GetKey(KeyCode.S);
        turningLeft = Input.GetKey(KeyCode.A);
        turningRight = Input.GetKey(KeyCode.D);
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(!IsOwner) return;

        if(!isRidingCapybara.Value) return;

        hasMovement = movingForward || movingBackward;

        rb.linearDamping = hasMovement ? movingDrag : stoppingDrag;

        if(movingForward) rb.AddForce(transform.forward * forwardSpeed, ForceMode.Force);
        if(movingBackward) rb.AddForce(-transform.forward * backwardSpeed, ForceMode.Force);
        if(turningLeft) rb.AddTorque(Vector3.up * leftTurnSpeed, ForceMode.VelocityChange);
        if(turningRight) rb.AddTorque(Vector3.up * rightTurnSpeed, ForceMode.VelocityChange);
    }

    void LateUpdate()
    {
        if(!IsOwner) return; 
        if(!isRidingCapybara.Value) return;
        if(player == null) return;

        player.transform.position = seatPosition.position;
    }
    public void RideCaybara(GameObject incomingPlayer)
    {
        player = incomingPlayer;
        isRidingCapybara.Value = true;

        if( player.GetComponent<PlayerController>() && player.GetComponent<playerCrouch>())
        {
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<playerCrouch>().enabled = false;
            player.GetComponentInChildren<CameraBob>().enabled = false;

            Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), true);

            var netAnimator = player.GetComponent<OwnerNetworkAnimator>();

            if (netAnimator)
            {
                netAnimator.Animator.SetBool("StopAnimation", true);
            }
        }
    }

    public void StopRidingCapybara()
    {
        isRidingCapybara.Value = false;
    }
}
