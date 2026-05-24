using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InventoryList : MonoBehaviour
{
    List<InventoryItem> partyInventory = new List<InventoryItem>();

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
        foreach (InventoryItem item in itemsToAdd)
        {
            if (partyInventory.Count > 0)
            {
                for (int i = 0; i < partyInventory.Count; i++)
                {
                    if (partyInventory[i].item == item.item)
                    {
                        //int idx = partyInventory.FindIndex(x => x.Equals(item));
                        partyInventory[i].AddToStack(item.GetStacks());
                        print("Total " + partyInventory[i].GetStacks() + " " + item.item);
                        break;
                    }
                }
            }
            else
            {
                partyInventory.Add(item);
                print("Gained " + item.GetStacks() + " " + item.item);
            }
        }
    }
}
