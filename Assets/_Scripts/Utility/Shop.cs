using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public List<Item> shopItems = new List<Item>();
    public List<int> shopPrices = new List<int>();
    Dictionary<Item, int> shopStock = new Dictionary<Item, int>();

    private void Start()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            shopStock.Add(shopItems[i], shopPrices[i]);
        }
    }

    public void DisplayShop()
    {
        //Tell UI Manager to make shop Components visible
        //Fill shop UI w/ shopstock key + shopstock value as buttons?
        //When player clicks button, subtract gold and add item selected
    }
}
