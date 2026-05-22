using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEngine;

public class InventoryManager : MonoBehaviour
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
        for(int i = 0; i < inventoryKeys.Length; i++)
        {
            if (Input.GetKeyDown(inventoryKeys[i]))
            {
                SwapItem(i);
            }
        }
    }

    public void SwapItem(int slotIndex)
    {
        var pickupScript = this.gameObject.GetComponent<OfficialPickupScript>();
        var spawnPosition = pickupScript.holdPosition;

       if(pickupScript.currentHeldObject != null)
        {
             Destroy(pickupScript.currentHeldObject);
        }
      

        ItemData targetItem = inventorySlots[slotIndex];
        objectToSpawn = targetItem.itemGameObject;

        GameObject heldItem = Instantiate(objectToSpawn, spawnPosition);
        heldItem.GetComponent<Rigidbody>().useGravity = false;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;

        heldItem.transform.localPosition = Vector3.zero;

        heldItem.transform.localRotation = Quaternion.identity;

        pickupScript.heldObject = heldItem;
        pickupScript.currentHeldObject = heldItem;

    }
}
