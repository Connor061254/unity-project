using Unity.Netcode;
using UnityEngine;

public class InteractableItem : NetworkBehaviour
{
    public GameObject popUpCanvas;

    public GameObject openCanvas;
  public void ShowPopUpPrompt()
    {
        Debug.Log("showPrompt is running!");
        if (popUpCanvas != null)
        {
            Debug.Log("popupcanvas is being set to active");
            popUpCanvas.GetComponent<BillBoardUI>().GetObjectPosition(gameObject.transform);
            popUpCanvas.SetActive(true);
        }
    }

    public void ShowOpenPrompt()
    {
         Debug.Log("showOpenPrompt is running!");
        if (openCanvas != null)
        {
            Debug.Log("opencanvas is being set to active");
            openCanvas.GetComponent<BillBoardUI>().GetObjectPosition(gameObject.transform);
            openCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("OpenCanvas is null");
        }
    }

    public void HidePrompt()
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(false);
        }

        if(openCanvas != null)
        {
            openCanvas.SetActive(false);
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

        if(openCanvas != null)
        {
            openCanvas.transform.SetParent(null);
            openCanvas.SetActive(false);
        }

        if (IsServer)
        {
            NetworkObject.DestroyWithScene = true;
        }
    }
}
