using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class Chest : MonoBehaviour
{
    //public bool isLooted = false;
    public static event Action<List<InventoryItem>> OnChestOpen;
    public List<Item> itemsInChest = new List<Item>();
    public List<int> amounts;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (isLooted) return;
        List<InventoryItem> iList = new List<InventoryItem>();
        for (int i = 0; i < itemsInChest.Count; i++)
        {
            itemsInChest[i].amount = amounts[i];
            InventoryItem newInvItem = new InventoryItem(itemsInChest[i], itemsInChest[i].amount);
            iList.Add(newInvItem);
        }
        OnChestOpen?.Invoke(iList);
        //isLooted = true;
    }
}
