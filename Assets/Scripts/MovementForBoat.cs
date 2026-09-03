using Unity.Netcode;
using UnityEngine;

public class MovementForBoat : NetworkBehaviour
{
    private Rigidbody rb;
    private bool moveForward;

    private bool turnRight;

    private bool turnLeft;

    private bool moveBackwards;

    public Transform seatPosition;

    public NetworkVariable<bool> isControllingBoat = new NetworkVariable<bool>(false);

    private GameObject player;

    [SerializeField] private float rightTurnSpeed = 100f;

    [SerializeField] private float leftTurnSpeed = -100f;

    [SerializeField] private float forwardSpeed = 4000f;

    [SerializeField] private float backwardSpeed = 2000f;


    void Start()
    {
       rb = GetComponent<Rigidbody>();

       rb.isKinematic = false;
    }

    // Read inputs here to prevent FixedUpdate from dropping keystrokes
    void Update()
    {
        if(!IsOwner || !isControllingBoat.Value) return;
        moveForward = Input.GetKey(KeyCode.W);
        
        moveBackwards = Input.GetKey(KeyCode.S);

        turnLeft = Input.GetKey(KeyCode.A);

        turnRight = Input.GetKey(KeyCode.D);
    }

    void FixedUpdate()
    {
        if(!IsOwner || !isControllingBoat.Value) return;

            if (moveForward)
            {
                rb.AddForce(transform.forward * forwardSpeed, ForceMode.Force);
            }

            if (turnRight) rb.AddTorque(Vector3.up * rightTurnSpeed, ForceMode.VelocityChange);

            if (turnLeft) rb.AddTorque(Vector3.up * leftTurnSpeed, ForceMode.VelocityChange);

            if (moveBackwards)
            {
                rb.AddForce(-transform.forward * backwardSpeed, ForceMode.Force);
            }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RequestBoardBoatRpc(RpcParams rpcParams = default)
    {
        if (isControllingBoat.Value) return;
        
        ulong clientId = rpcParams.Receive.SenderClientId;
        GetComponent<NetworkObject>().ChangeOwnership(clientId);
        isControllingBoat.Value = true;
    }

    [Rpc(SendTo.Server)]
    public void RequestExitBoatRpc()
    {
        GetComponent<NetworkObject>().RemoveOwnership();
        isControllingBoat.Value = false;
    }

    public void BoardBoat(GameObject boatPlayer)
    {

        player = boatPlayer;

        if( player.GetComponent<PlayerController>() && player.GetComponent<playerCrouch>())
        {
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<playerCrouch>().enabled = false;
        }
    }

    void LateUpdate()
    {
        if (isControllingBoat.Value && IsOwner)
        {
            player.transform.position = seatPosition.position;

            player.transform.rotation = seatPosition.rotation;
        }
    }

    public void LocalExitBoat()
    {
        player = null;
    }
}