using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InventoryList : MonoBehaviour
{
    List<InventoryItem> partyInventory = new List<InventoryItem>();
    Dictionary<string, InventoryItem> partyInv = new Dictionary<string, InventoryItem>();

    private void OnEnable()
    {
        Chest.OnChestOpen += GetItem;
    }
    private void OnDisable()
    {
        Chest.OnChestOpen -= GetItem;
    }
    public void SavePlayerInventory()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "InventoryTestSave.json");
    }
    public void LoadPlayerInventory() 
    { 
    
    }
    void GetItem(List<InventoryItem> itemsToAdd)
    {
        bool newItem = true;
        foreach (InventoryItem item in itemsToAdd)
        {
            print($"Gained {item.item.name} : {item.GetStacks()}");
            if (partyInv.ContainsKey(item.item.name))
            {
                partyInv[item.item.name].AddToStack(item.GetStacks());
            }
            else
            {
                partyInv[item.item.name] = item;
            }
             foreach(var k in partyInv)
            {
                print($"Inv: {k.Key}-{k.Value.GetStacks()}");
            }   
            
            //if (partyInventory.Count > 0)
            //{
            //    for (int i = 0; i < partyInventory.Count; i++)
            //    {
            //        if (partyInventory[i].item == item.item)
            //        {
            //            //int idx = partyInventory.FindIndex(x => x.Equals(item));
            //            partyInventory[i].AddToStack(item.GetStacks());
            //            print("Total " + partyInventory[i].GetStacks() + " " + item.item);
            //            newItem = false;
            //            break;
            //        }
            //        else
            //        {
            //            newItem = true;
            //        }
            //    }
            //}
            //else if(newItem)
            //{
            //    partyInventory.Add(item);
            //    newItem = false;
            //}

        }
    }
}
