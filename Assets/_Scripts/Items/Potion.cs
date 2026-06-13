using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Item/Potion")]
public class Potion : Item, IUsable, IEquipable
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
        Debug.Log("Heal 10 HP");
    }
}
