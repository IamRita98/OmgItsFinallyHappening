using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Items/ConsumableItem")]
public class ConsumableItem : Item
{
    //are gonna affect stats
    public Rarity rarity;


    public void DoSomething()
    {
        Debug.Log("hello");
    }
}
