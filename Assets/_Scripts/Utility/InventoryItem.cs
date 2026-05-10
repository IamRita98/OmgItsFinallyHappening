using UnityEngine;

public class InventoryItem
{
    Item item;
    private int stacks;

    public InventoryItem(Item itemP)
    {
        item = itemP;
    }
    public void AddToStack()
    {
        stacks++;
    }
    public void RemoveFromStack()
    {
        stacks--;
    }
    public int GetStacks()
    {
        return stacks;
    }
}
