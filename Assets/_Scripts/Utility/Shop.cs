using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public List<Item> shopItems = new List<Item>();
    public List<int> shopPrices = new List<int>();
    Dictionary<Item, int> shopStock = new Dictionary<Item, int>();
    private List<Tuple<string, Item,int>> displayShopItemsList = new List<Tuple<string, Item, int>>();
    public static event Action<List<InventoryItem>> OnItemBought;
    private void Start()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            shopStock.Add(shopItems[i], shopPrices[i]);
        }
    }

    public void ShopItemButtonSetup(Item item)
    {
        //convert item to InventoryItem for player inventory
        //subtract gold from player
        //If item is unique and player bought it or has it already, gray it out/make unbuyable
        InventoryItem inventoryItem = new InventoryItem(item);
        List<InventoryItem> listToPass =new List<InventoryItem>();
        listToPass.Add(inventoryItem);
        if (UnitSelector.Instance.GetPlayerGold() < (uint)shopStock[item])
        {
            print("Not enough money");
            return;
        }
        UnitSelector.Instance.SubtractGold((uint)shopStock[item]);//unsure about this working, just pass in price too, to the button listeners
        OnItemBought?.Invoke(listToPass);
    }
    public void DisplayShop()
    {
        UnitSelector.Instance.DisplayGold();
        MakeShopStock();
        UIManager.Instance.shopStock = displayShopItemsList;
        UIManager.Instance.DisplayInventory(true,gameObject);
        //Tell UI Manager to make shop Components visible
        //Fill shop UI w/ shopstock key + shopstock value as buttons?
        //When player clicks button, subtract gold and add item selected
    }

    private void MakeShopStock()
    {
        /*foreach (var item in shopStock)
        {
            displayShopItemsList.Add(new Tuple<string, Item, int>(item.Key.name,item.Key, item.Value));
        }*/

        for (int i = 0; i < shopItems.Count; i++)
        {
            displayShopItemsList.Add(new Tuple<string, Item, int>(shopItems[i].name, shopItems[i], shopPrices[i]));
        }
    }
}
