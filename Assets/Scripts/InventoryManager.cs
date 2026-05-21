using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> itemList = new List<ItemData>();
    public int space = 10;

    public GameObject objectToDelete;

    public GameObject objectToSpawn;
   
    public bool AddItem(ItemData itemToAdd)
    {
        if(itemList.Count >= space)
        {
            return false;
        }

        itemList.Add(itemToAdd);

        return true;
    }

    public void RemoveItem(ItemData itemToRemove)
    {
        itemList.Remove(itemToRemove);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapItem();
        }
    }

    public void SwapItem()
    {
        var pickupScript = this.gameObject.GetComponent<OfficialPickupScript>();
        var spawnPosition = pickupScript.holdPosition;

        Destroy(pickupScript.currentHeldObject);

        ItemData secondItem = itemList[1];
        objectToSpawn = secondItem.itemGameObject;

        GameObject heldItem = Instantiate(objectToSpawn, spawnPosition);

        heldItem.transform.localPosition = Vector3.zero;

        heldItem.transform.localRotation = Quaternion.identity;

        pickupScript.heldObject = heldItem;
        pickupScript.currentHeldObject = heldItem;
    }
}
