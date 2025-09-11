// PlayerInteraction.cs - Attach this to your PLAYER
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCamera;
    private BoatController currentBoat;

    void Update()
    {
        // Check if we are trying to board or exit a boat
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBoat == null) // If we are NOT on a boat, try to board one
            {
                TryBoardBoat();
            }
            else // If we ARE on a boat, exit it
            {
                currentBoat.ExitBoat();
                currentBoat = null; // We are no longer on a boat
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
            // Get the boat's controller script and tell it to start
            BoatController boat = hit.transform.GetComponent<BoatController>();
            if (boat != null)
            {
                currentBoat = boat;
                // We pass this player GameObject to the boat so it knows who is driving
                currentBoat.BoardBoat(this.gameObject);
            }
        }
    }
}