using Unity.Netcode;
using UnityEngine;

public class BuoyancyForPlayer : NetworkBehaviour
{
    private CharacterController controller;

    public float forceStrength = 2f;

    private float verticalVelocity;

    public override void OnNetworkSpawn()
    {
        if (this.gameObject != null)
        {
            controller = gameObject.GetComponent<CharacterController>();

            if (controller != null)
            {
                Debug.Log("rigidbody aqcuired");
            }
            else
            {
                Debug.Log("No Rigidbody found");
            }
            
        }
    }
    void Update()
    {
        if (controller== null) return;

        if(gameObject.transform.position.y <= 0)
        {
            verticalVelocity = forceStrength * Time.deltaTime;

            if (verticalVelocity > forceStrength) verticalVelocity = forceStrength;
            Debug.Log("applying force to player");
        }
        else
        {
            Debug.Log("the player is too high up");
        }
    }
}
