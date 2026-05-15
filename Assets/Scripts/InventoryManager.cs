using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> itemList = new List<ItemData>();
    public int space = 10;
   
    public bool AddItem(ItemData itemToAdd)
    {
        if(itemList.Count >= space)
        {
            return false;
        }

        itemList.Add(itemToAdd);

        return true;
    }
}
