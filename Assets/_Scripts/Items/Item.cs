using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
public class Item : ScriptableObject, IUsable, IEquipable
{
    //WeaponItems,ArmourItems,QuestItems,ConsumableItems
    public string name;
    public int amount;
    public string equipEffectText; //Like "+2 str" or "Auto-consume potion when hp reaches 50%"
    public string flavorText;
    //Image menuSprite;
    public bool isKeyItem; //We don't want to show players the option to discard key item. I think this could just be a tag tho tbh
    //I was thinking about it a bit, I feel like rarity is better as an enum instead of a tag.
    //Rarities are mutually exclusive while tags are not-- you can only ever have 1 rarity but theres no limit to how many/which tags you can have
    //I don't think scriptable objects can be changed during runtime. I think things that
    //Reference it just pull the data on start and thats it. So the quantity as part of the class thing won't work w/ SO's
    //I guess we could set up like a dictionary in the inv class of like {Item, Amount} to check that still
    public List<string> tags;
    public Sprite sprite;
    public virtual void Equip() // I don't think you can change the interface uses on scriptable objects... We might have to use inheritence
    {
        
    }

    public virtual void UnEquip()
    {
        
    }

    public virtual void Use()
    {
        Debug.Log("We're idiots");
    }
    
}
public enum Rarity
{
    Common,
    Rare,
    Legendary,
}
