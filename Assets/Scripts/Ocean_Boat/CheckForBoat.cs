// PlayerInteraction.cs - Attach this to your PLAYER
using Unity.Netcode;
using UnityEngine;

public class CheckForBoat : NetworkBehaviour
{
    public Camera mainCamera;
    private MovementForBoat currentBoat;

    void Update()
    {
        if(!IsOwner) return;
        // Check if we are trying to board or exit a boat
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBoat == null) // If we are NOT on a boat, try to board one
            {
                TryBoardBoat();
            }
            else // If we ARE on a boat, exit it
            {
                currentBoat.RequestExitBoatRpc();
                currentBoat.LocalExitBoat();
                GetComponent<PlayerController>().enabled = true;
                GetComponent<playerCrouch>().enabled = true;
                currentBoat = null;
            }
        }
    }

    void TryBoardBoat()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;

        // Look for a boat within 5 units
        if (Physics.Raycast(ray, out hit, 5f) && hit.transform.CompareTag("Boat"))
        {
            var boat = hit.transform.GetComponent<MovementForBoat>();
            if (boat != null)
            {
                currentBoat = boat;
              
                currentBoat.RequestBoardBoatRpc();
                currentBoat.BoardBoat(this.gameObject);
            }
        }
    }
}