using Unity.Netcode;
using UnityEngine;

public class CheckForCapybara : NetworkBehaviour
{
    public Camera mainCamera;
    private CapybaraMovement currentCapybara;

    void Update()
    {
        if(!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentCapybara == null)
            {
                TryRideCapybara();
            }
            else
            {
                currentCapybara.StopRidingCapybara();
                GetComponent<PlayerController>().enabled = true;
                GetComponent<playerCrouch>().enabled = true;
                GetComponent<CharacterControllerWater>().enabled = true;
                currentCapybara = null;
            }
        }
    }

    void TryRideCapybara()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;

        // Look for a boat within 5 units
        if (Physics.Raycast(ray, out hit, 5f) && hit.transform.GetComponent<CapybaraMovement>())
        {
            var cappybara = hit.transform.GetComponent<CapybaraMovement>();
            if (cappybara != null && cappybara.player == null)
            {
                currentCapybara = cappybara;
              
                currentCapybara.RideCaybara(this.gameObject);
            }
        }
    }
}
