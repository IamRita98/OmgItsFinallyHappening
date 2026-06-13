using UnityEngine;

[CreateAssetMenu(fileName = "Antidote", menuName = "Item/Antidote")]
public class Antidote : Item, IUsable, IEquipable
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

    public override void Use()
    {
        Debug.Log("Remove Poison");
    }
}
