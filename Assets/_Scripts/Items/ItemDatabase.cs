using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ItemDatabase/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items=new List<Item>();
}
