using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Item/Potion")]
public class Potion : Item, IUsable, IEquipable
{
    //are gonna affect stats
    public Rarity rarity;
    private GameObject target;
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
        while (target == null)
        {
            
        }
        //CALL TO UIMANAGER TO PULL UP LIST OF ENEMIES/ALLIES IN RANGE FOR THING
        //get target ->pulls up list of allies or enemies in range to use
        //once player picks target->target=target
        //then run itemEffects
        foreach (var item in itemEffects)
        {
            item.ExecuteEffects(target);
        }
    }
}
