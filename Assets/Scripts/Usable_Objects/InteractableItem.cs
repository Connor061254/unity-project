using Unity.Netcode;
using UnityEngine;

public class InteractableItem : NetworkBehaviour
{
    public GameObject popUpCanvas;
  public void ShowPrompt()
    {
        Debug.Log("showPrompt is running!");
        if (popUpCanvas != null)
        {
            Debug.Log("popupcanvas is being set to active");
            popUpCanvas.GetComponent<BillBoardUI>().GetObjectPosition(gameObject.transform);
            popUpCanvas.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(false);
        }
    }

    public override void OnNetworkSpawn()
    {
        if(popUpCanvas != null)
        {
            if (!popUpCanvas.gameObject.scene.IsValid())
            {
                Debug.LogError("🚨 THIS PREFAB HAS THE BROKEN REFERENCE: " + gameObject.name + " 🚨", gameObject);
            }
            else
            {
                 popUpCanvas.transform.SetParent(null);
                 popUpCanvas.SetActive(false);
            }
                
        }

        if (IsServer)
        {
            NetworkObject.DestroyWithScene = true;
        }
    }
}
