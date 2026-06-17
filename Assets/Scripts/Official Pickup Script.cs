using UnityEditor;
using UnityEngine;
using Unity.Netcode;

public class OfficialPickupScript : NetworkBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 2f;
    public float throwForce = 8f;
    public GameObject heldObject;
    private bool canPickup = true;

    public ItemData itemInfo;

    public float dropForce = 2f;
    Rigidbody rb;
    public Camera mainCamera;

    private InteractableItem currentTarget;

    public GameObject currentHeldObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptToPickup();
            }
    
        else if (heldObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }
        }
    }
    void FixedUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
                var hitItem = hit.collider.GetComponent<InteractableItem>();
                
                if(hitItem != null)
                {
                    if(currentTarget != hitItem)
                    {
                        if(currentTarget != null) currentTarget.HidePrompt();

                        currentTarget = hitItem;
                        currentTarget.ShowPrompt();

                    }
                }
                else
                {
                    if(currentTarget != null)
                    {
                        currentTarget.HidePrompt();
                        currentTarget = null;
                    }
                }
        
        }
    }
    void AttemptToPickup()
    {
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange, layerMask))
        {
            Debug.Log("My Raycast hit: " + hit.collider.name);

            if (hit.transform != null && hit.transform.CompareTag("PickUp"))
            {
                NetworkObject networkObject = hit.transform.GetComponent<NetworkObject>();
                heldObject = hit.transform.gameObject;

                var itemProp = heldObject.GetComponent<ItemProperties>();
                var inventory = this.gameObject.GetComponent<InventoryManager>();

                if(inventory != null && itemProp != null && networkObject != null)
                {
                    itemInfo = itemProp.referenceData;
                    
                    bool wasPickedUp = inventory.AddItem(itemInfo);

                    if (wasPickedUp)
                    {
                        RequestPickUpServerRpc(NetworkObject.NetworkObjectId);
                    }
                    else
                    {
                        heldObject = null;
                    }

                }
                
            }

            
        }
    }
    [ServerRpc]   
     void RequestPickUpServerRpc(ulong itemId, ServerRpcParams rpcParams = default)
    {
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemId, out NetworkObject item))
        {
            item.TrySetParent(holdPosition);

            item.ChangeOwnership(rpcParams.Receive.SenderClientId);

            PerformPickUpClientRpc(itemId);
        }
    }
    [ClientRpc]
    void PerformPickUpClientRpc(ulong itemId)
    {
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemId, out NetworkObject item))
        {
            heldObject = item.gameObject;

             if(currentHeldObject != null)
                        {
                            currentHeldObject.SetActive(false);
                        }
                        
                        PerformPickup();
        }
    }

    void PerformPickup()
    {
        currentHeldObject = heldObject;
        var itemSettings = heldObject.GetComponent<ItemPickupPosition>();
        if(itemSettings != null)
        {
            heldObject.transform.localPosition = itemSettings.positionOffset;
            heldObject.transform.localRotation = Quaternion.Euler(itemSettings.rotationOffset);
        }

        else
        {
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
        }
       
        
        Rigidbody objectRb = heldObject.GetComponent<Rigidbody>();
        objectRb.isKinematic = true;
        objectRb.useGravity = false;
    }

    void Drop()
    {
        StartCoroutine(DropCooldown());
        var inventory = this.gameObject.GetComponent<InventoryManager>();
        itemInfo = heldObject.GetComponent<ItemProperties>().referenceData;
        inventory.RemoveItem(itemInfo);

        NetworkObject networkObject = heldObject.GetComponent<NetworkObject>();
        if(networkObject != null)
        {
            RequestDropServerRpc(networkObject.NetworkObjectId, mainCamera.transform.forward);
        }   

    }

    [ServerRpc]
    void RequestDropServerRpc(ulong itemId, Vector3 dropDirection, ServerRpcParams rpcparams = default)
    {
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemId, out NetworkObject item))
        {
            item.TryRemoveParent();

            item.RemoveOwnership();

            PerformDropClientRpc(itemId, dropDirection);
        }
    }

    [ClientRpc]
    void PerformDropClientRpc(ulong itemId, Vector3 dropDirection)
    {
        
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemId, out NetworkObject item))
        {
            GameObject droppedItem = item.gameObject;
            
            Rigidbody objectRb = droppedItem.GetComponent<Rigidbody>();
            if(objectRb != null)
            {
                objectRb.isKinematic = false;
                objectRb.useGravity = true;
                objectRb.AddForce(dropDirection * dropForce, ForceMode.VelocityChange);
            }

            if(heldObject == droppedItem)
            {
                heldObject = null;
                currentHeldObject = null;
            }
        }
    }

    System.Collections.IEnumerator DropCooldown()
    {
        canPickup = false;
        yield return new WaitForSeconds(0.5f); 
        canPickup = true;
    }

    
    
}

