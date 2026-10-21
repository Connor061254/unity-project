using System;
using System.Collections.Generic;
using Mono.CSharp;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class InventoryManager : NetworkBehaviour
{
    public ItemData[] inventorySlots = new ItemData[10];
    public int space = 10;

    public GameObject objectToDelete;

    public GameObject objectToSpawn;

    private OfficialPickupScript pickupScript;

    private readonly KeyCode[] inventoryKeys = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
    };
   

   void Start()
    {
        pickupScript = GetComponent<OfficialPickupScript>();
    }
    public bool AddItem(ItemData itemToAdd)
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if(inventorySlots[i] == null)
            {
                inventorySlots[i] = itemToAdd;

                return true;
            }

        }

        return false;
    }

    public void RemoveItem(ItemData itemToRemove)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if(inventorySlots[i] == itemToRemove)
            {
                inventorySlots[i] = null;
                break;
            }
        }
    }

    void Update()
    {
        if(!IsOwner) return;

        for(int i = 0; i < inventoryKeys.Length; i++)
        {
            if (Input.GetKeyDown(inventoryKeys[i]))
            {
                RequestSwapItemRpc(i);
            }
        }
    }

    [Rpc(SendTo.Server)]
    public void RequestSwapItemRpc(int slotIndex)
    {
        var pickupScript = gameObject.GetComponent<OfficialPickupScript>();

        if(pickupScript.currentHeldObject != null)
        {
            var oldNetworkObject = pickupScript.currentHeldObject.GetComponent<NetworkObject>();
            if(oldNetworkObject != null && oldNetworkObject.IsSpawned)
            {
                oldNetworkObject.Despawn();
            }
        }

        var targetItem = inventorySlots[slotIndex];

        if(targetItem == null || targetItem.itemGameObject == null)
        {
            return;
        }

        var spawnPosition = pickupScript.holdPosition;
        GameObject heldItem = Instantiate(targetItem.itemGameObject, spawnPosition.position, Quaternion.identity);

        var netObjHeldItem = heldItem.GetComponent<NetworkObject>();
        netObjHeldItem.SpawnWithOwnership(OwnerClientId);

        PerformSwapItemRpc(netObjHeldItem.NetworkObjectId);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void PerformSwapItemRpc(ulong newItemId)
    {
        if(!IsServer) return;

        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(newItemId, out var spawnedNetObj))
        {
            GameObject heldItem = spawnedNetObj.gameObject;
            var pickupScript = this.gameObject.GetComponent<OfficialPickupScript>();

            if(heldItem.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }

            if(heldItem.TryGetComponentsInChildren<Collider>(out Collider[] colliders))
            {
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }
            }

            if (heldItem.GetComponent<SpecialAbility>() != null)
            {
                PlayerController playerController = GetComponent<PlayerController>();
                CharacterType identity = GetComponent<Identification>().type;
                heldItem.GetComponent<SpecialAbility>().InitalizeRock(identity, playerController);
            }

            pickupScript.heldObject = heldItem;
            pickupScript.currentHeldObject = heldItem;
        }
    }
}
   
