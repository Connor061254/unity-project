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
    public Camera mainCamera;

    private InteractableItem currentTarget;
    public GameObject currentHeldObject;

    void Start() { }

    void Update()
    {
        if(!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.E) && canPickup)
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

    // THE VIRTUAL PARENT: This glues the item to your hand every single frame!
    void LateUpdate()
    {
        if (heldObject != null && currentHeldObject != null)
        {
            var itemSettings = heldObject.GetComponent<ItemPickupPosition>();
            if (itemSettings != null)
            {
                heldObject.transform.position = holdPosition.TransformPoint(itemSettings.positionOffset);
                heldObject.transform.rotation = holdPosition.rotation * Quaternion.Euler(itemSettings.rotationOffset);
            }
            else
            {
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
            }
        }
    }

    void FixedUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            var hitItem = hit.collider.GetComponentInParent<InteractableItem>();
            
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
            if (hit.transform != null && hit.transform.CompareTag("PickUp"))
            {
                NetworkObject networkObject = hit.transform.GetComponentInParent<NetworkObject>();
                
                if (networkObject != null)
                {
                    heldObject = networkObject.gameObject;
                }

                if(heldObject.GetComponent<SpecialAbility>() != null)
                {
                    PassIdentity();
                }

                if (heldObject != null)
                {
                    var itemProp = heldObject.GetComponent<ItemProperties>();
                    var inventory = this.gameObject.GetComponent<InventoryManager>();

                    if(inventory != null && itemProp != null && networkObject != null)
                    {
                        itemInfo = itemProp.referenceData;
                        bool wasPickedUp = inventory.AddItem(itemInfo);

                        if (wasPickedUp)
                        {
                            Rigidbody[] allRbs = heldObject.GetComponentsInChildren<Rigidbody>();
                            foreach (Rigidbody r in allRbs)
                            {
                                r.isKinematic = true;
                                r.useGravity = false;
                            }

                            Collider[] allCols = heldObject.GetComponentsInChildren<Collider>();
                            foreach (Collider c in allCols)
                            {
                                c.enabled = false;
                            }
                            
                            RequestPickUpServerRpc(networkObject.NetworkObjectId);
                        }
                        else
                        {
                            heldObject = null;
                        }
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
            // DELETED the SetParent code here. We only need to give the player ownership!
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
        // We just set this variable so LateUpdate() knows it is time to start gluing it to the hand
        currentHeldObject = heldObject;

        Rigidbody[] allRbs = heldObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in allRbs)
        {
            r.isKinematic = true;
            r.useGravity = false;
        }

        Collider[] allCols = heldObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in allCols)
        {
            c.enabled = false;
        }
    }

    void Drop()
    {
        StartCoroutine(DropCooldown());
        var inventory = this.gameObject.GetComponent<InventoryManager>();
        
        var itemProp = heldObject.GetComponent<ItemProperties>();
        if (itemProp != null && inventory != null)
        {
            itemInfo = itemProp.referenceData;
            inventory.RemoveItem(itemInfo);
        }

        NetworkObject networkObject = heldObject.GetComponent<NetworkObject>();
        if(networkObject != null)
        {
            RequestDropServerRpc(networkObject.NetworkObjectId, mainCamera.transform.forward);
        }

        if (heldObject.GetComponent<SpecialAbility>())
        {
            heldObject.GetComponent<SpecialAbility>().ReduceSpeed();
        }
    }

    [ServerRpc]
    void RequestDropServerRpc(ulong itemId, Vector3 dropDirection, ServerRpcParams rpcparams = default)
    {
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemId, out NetworkObject item))
        {
            // Take ownership back for the Server
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
            
            Rigidbody[] allRbs = droppedItem.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody r in allRbs)
            {
                r.isKinematic = false;
                r.useGravity = true;
                r.AddForce(dropDirection * dropForce, ForceMode.VelocityChange);
            }
            
            Collider[] allCols = droppedItem.GetComponentsInChildren<Collider>();
            Debug.Log($"Found {allCols.Length} colliders on {droppedItem.name}");
            foreach (Collider c in allCols)
            {
                c.enabled = true;
                Debug.Log($"Successfully enabled: {c.gameObject.name}");

            }

            // Setting these to null instantly tells LateUpdate() to stop holding the item
            if(heldObject == droppedItem)
            {
                heldObject = null;
                currentHeldObject = null;
            }
        }
    }

    public void PassIdentity()
    {
        CharacterType identity = this.GetComponent<Identification>().type;

        heldObject.GetComponent<SpecialAbility>().InitalizeRock(identity);
    }

    System.Collections.IEnumerator DropCooldown()
    {
        canPickup = false;
        yield return new WaitForSeconds(0.5f); 
        canPickup = true;
    }
}