using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Items/ConsumableItem")]
public class ConsumableItem : Item, IUsable, IEquipable
{
    //are gonna affect stats
    public Rarity rarity;

    public void Equip()
    {
        throw new System.NotImplementedException();
    }

    public void UnEquip()
    {
        throw new System.NotImplementedException();
    }

    public void Use()
    {
        Debug.Log("hello");
    }
}
